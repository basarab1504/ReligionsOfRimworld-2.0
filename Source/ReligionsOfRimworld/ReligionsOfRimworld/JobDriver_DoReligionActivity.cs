using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ReligionsOfRimworld
{
    public class JobDriver_DoReligionActivity : JobDriver
    {
        public int billStartTick;

        public const PathEndMode GotoIngredientPathEndMode = PathEndMode.ClosestTouch;

        public const TargetIndex BillGiverInd = TargetIndex.A;

        public const TargetIndex IngredientInd = TargetIndex.B;

        public const TargetIndex IngredientPlaceCellInd = TargetIndex.C;

        public Building_ReligiousBuildingFacility TargetFacility
        {
            get
            {
                Building_ReligiousBuildingFacility targetFacility = this.job.GetTarget(TargetIndex.A).Thing as Building_ReligiousBuildingFacility;
                if (targetFacility == null)
                {
                    throw new InvalidOperationException("DoBill on non-Facility.");
                }
                return targetFacility;
            }
        }

        public override string GetReport()
        {
            if (this.job.RecipeDef != null)
            {
                return base.ReportStringProcessed(this.job.RecipeDef.jobString);
            }
            return base.GetReport();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.billStartTick, "billStartTick", 0, false);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, (ReservationLayerDef)null, errorOnFailed))
                return false;
            this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, (ReservationLayerDef)null);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            base.AddEndCondition(delegate
            {
                Thing thing = this.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
                if (thing is Building && !thing.Spawned)
                {
                    return JobCondition.Incompletable;
                }
                return JobCondition.Ongoing;
            });
            this.FailOnBurningImmobile(TargetIndex.A);
     //       this.FailOn(delegate
     //       {
     //           IBillGiver billGiver = this.job.GetTarget(TargetIndex.A).Thing as IBillGiver;
     //           if (billGiver != null)
     //           {
     //               if (this.job.bill.DeletedOrDereferenced)
					//{
					//	return true;
					//}
					//if (!billGiver.CurrentlyUsableForBills())
					//{
					//	return true;
					//}
     //           }
     //           return false;
     //       });
            Toil gotoBillGiver = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            yield return Toils_Jump.JumpIf(gotoBillGiver, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
            Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
            yield return extract;
            Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
            yield return getToHaulTarget;
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, true, false, true);
            yield return JumpToCollectNextIntoHandsForBill(getToHaulTarget, TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDestroyedOrNull(TargetIndex.B);
            Toil findPlaceTarget = Toils_JobTransforms.SetTargetToIngredientPlaceCell(TargetIndex.A, TargetIndex.B, TargetIndex.C);
            yield return findPlaceTarget;
            yield return PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false);
           
            //Pawn target = (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
            //Log.Message(target.ToString());

            yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, extract);
            yield return gotoBillGiver;
            yield return StartActivity();
            //yield return Waiting();
            //yield return DoRecipeWork();
            //yield return Toils_Recipe.FinishRecipeAndStartStoringProduct();///////////////////////
            //if (!this.job.RecipeDef.products.NullOrEmpty<ThingDefCountClass>() || !this.job.RecipeDef.specialProducts.NullOrEmpty<SpecialProductType>())
            //{
            //    yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
            //    Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
            //    yield return carryToCell;
            //    yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
            //    Toil recount = new Toil();
            //    recount.initAction = delegate
            //    {
            //        Bill_Production bill_Production = recount.actor.jobs.curJob.bill as Bill_Production;
            //        if (bill_Production != null && bill_Production.repeatMode == BillRepeatModeDefOf.TargetCount)
            //        {
            //            this.Map.resourceCounter.UpdateResourceCounts();
            //        }
            //    };
            //    yield return recount;
            //}
        }

        private Toil StartActivity()
        {
            return new Toil()
            {
                initAction = delegate
                {
                    Religion religion = TargetFacility.AssignedReligion;
                    Pawn organizer = pawn;
                    List<LocalTargetInfo> targets = new List<LocalTargetInfo>();
                    if (job.placedThings != null)
                        foreach (ThingCountClass tcc in job.placedThings)
                        {
                            targets.Add(tcc.thing);
                        }
                    ActivityUtility.StartActivity(religion, organizer, ((ActivityJob)job).activityTask, targets);
                }
            };
        }

        private Toil Waiting()
        {
            return new Toil()
            {
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 740,
            }.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
        }

        private static Toil PlaceHauledThingInCell(TargetIndex cellInd, Toil nextToilOnPlaceFailOrIncomplete, bool storageMode)
        {
            Toil toil = new Toil();
            toil.initAction = (Action)(() =>
            {
                Pawn actor = toil.actor;
                Job curJob = actor.jobs.curJob;
                IntVec3 cell = curJob.GetTarget(cellInd).Cell;
                if (actor.carryTracker.CarriedThing == null)
                {
                    Log.Error(actor.ToString() + " tried to place hauled thing in cell but is not hauling anything.", false);
                }
                else
                {
                    SlotGroup slotGroup = actor.Map.haulDestinationManager.SlotGroupAt(cell);
                    if (slotGroup != null && slotGroup.Settings.AllowedToAccept(actor.carryTracker.CarriedThing))
                        actor.Map.designationManager.TryRemoveDesignationOn(actor.carryTracker.CarriedThing, DesignationDefOf.Haul);
                    Action<Thing, int> placedAction = (Action<Thing, int>)null;
                        placedAction = (Action<Thing, int>)((th, added) =>
                        {
                            if (curJob.placedThings == null)
                                curJob.placedThings = new List<ThingCountClass>();
                            ThingCountClass thingCountClass = curJob.placedThings.Find((Predicate<ThingCountClass>)(x => x.thing == th));
                            if (thingCountClass != null)
                                thingCountClass.Count += added;
                            else
                                curJob.placedThings.Add(new ThingCountClass(th, added));
                        });
                    Thing resultingThing;
                    if (actor.carryTracker.TryDropCarriedThing(cell, ThingPlaceMode.Direct, out resultingThing, placedAction))
                        return;
                    if (storageMode)
                    {
                        IntVec3 foundCell;
                        if (nextToilOnPlaceFailOrIncomplete != null && StoreUtility.TryFindBestBetterStoreCellFor(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, out foundCell, true))
                        {
                            if (actor.CanReserve((LocalTargetInfo)foundCell, 1, -1, (ReservationLayerDef)null, false))
                                actor.Reserve((LocalTargetInfo)foundCell, actor.CurJob, 1, -1, (ReservationLayerDef)null, true);
                            actor.CurJob.SetTarget(cellInd, (LocalTargetInfo)foundCell);
                            actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
                        }
                        else
                        {
                            Job job = HaulAIUtility.HaulAsideJobFor(actor, actor.carryTracker.CarriedThing);
                            if (job != null)
                            {
                                curJob.targetA = job.targetA;
                                curJob.targetB = job.targetB;
                                curJob.targetC = job.targetC;
                                curJob.count = job.count;
                                curJob.haulOpportunisticDuplicates = job.haulOpportunisticDuplicates;
                                curJob.haulMode = job.haulMode;
                                actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
                            }
                            else
                            {
                                Log.Error("Incomplete haul for " + (object)actor + ": Could not find anywhere to put " + (object)actor.carryTracker.CarriedThing + " near " + (object)actor.Position + ". Destroying. This should never happen!", false);
                                actor.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
                            }
                        }
                    }
                    else
                    {
                        if (nextToilOnPlaceFailOrIncomplete == null)
                            return;
                        actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
                    }
                }
            });
            return toil;
        }

        private static Toil JumpToCollectNextIntoHandsForBill(Toil gotoGetTargetToil, TargetIndex ind)
        {
            Toil toil = new Toil();
            toil.initAction = delegate
            {
                Pawn actor = toil.actor;
                if (actor.carryTracker.CarriedThing == null)
                {
                    Log.Error("JumpToAlsoCollectTargetInQueue run on " + actor + " who is not carrying something.", false);
                    return;
                }
                if (actor.carryTracker.Full)
                {
                    return;
                }
                Job curJob = actor.jobs.curJob;
                List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
                if (targetQueue.NullOrEmpty<LocalTargetInfo>())
                {
                    return;
                }
                for (int i = 0; i < targetQueue.Count; i++)
                {
                    if (GenAI.CanUseItemForWork(actor, targetQueue[i].Thing))
                    {
                        if (targetQueue[i].Thing.CanStackWith(actor.carryTracker.CarriedThing))
                        {
                            if ((float)(actor.Position - targetQueue[i].Thing.Position).LengthHorizontalSquared <= 64f)
                            {
                                int num = (actor.carryTracker.CarriedThing != null) ? actor.carryTracker.CarriedThing.stackCount : 0;
                                int num2 = curJob.countQueue[i];
                                num2 = Mathf.Min(num2, targetQueue[i].Thing.def.stackLimit - num);
                                num2 = Mathf.Min(num2, actor.carryTracker.AvailableStackSpace(targetQueue[i].Thing.def));
                                if (num2 > 0)
                                {
                                    curJob.count = num2;
                                    curJob.SetTarget(ind, targetQueue[i].Thing);
                                    List<int> countQueue;
                                    int index;
                                    (countQueue = curJob.countQueue)[index = i] = countQueue[index] - num2;
                                    if (curJob.countQueue[i] <= 0)
                                    {
                                        curJob.countQueue.RemoveAt(i);
                                        targetQueue.RemoveAt(i);
                                    }
                                    actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
                                    return;
                                }
                            }
                        }
                    }
                }
            };
            return toil;
        }
    }
}
