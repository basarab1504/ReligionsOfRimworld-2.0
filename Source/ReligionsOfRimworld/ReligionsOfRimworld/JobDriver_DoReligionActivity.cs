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

        private List<LocalTargetInfo> targetsCopy;

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
            Pawn pawn = this.pawn;
            LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
            Job job = this.job;
            if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
            {
                return false;
            }
            targetsCopy = new List<LocalTargetInfo>(this.job.GetTargetQueue(TargetIndex.B));
            this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
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
            yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, findPlaceTarget, false);
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
                    ActivityUtility.StartActivity(religion, organizer, ((ActivityJob)job).activityTask, targetsCopy);
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

        //private Toil DoRecipeWork()
        //{
        //    Toil toil = new Toil();
        //    toil.initAction = (Action)(() =>
        //    {
        //        Pawn actor = toil.actor;
        //        Job curJob = actor.jobs.curJob;
        //        JobDriver_DoReligionActivity curDriver = (JobDriver_DoReligionActivity)actor.jobs.curDriver;
        //        UnfinishedThing thing = curJob.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
        //        if (thing != null && thing.Initialized)
        //        {
        //            curDriver.workLeft = thing.workLeft;
        //        }
        //        else
        //        {
        //            curDriver.workLeft = curJob.bill.recipe.WorkAmountTotal(thing == null ? (ThingDef)null : thing.Stuff);
        //            if (thing != null)
        //                thing.workLeft = curDriver.workLeft;
        //        }
        //        curDriver.billStartTick = Find.TickManager.TicksGame;
        //        curDriver.ticksSpentDoingRecipeWork = 0;
        //        curJob.bill.Notify_DoBillStarted(actor);
        //    });
        //    toil.tickAction = (Action)(() =>
        //    {
        //        Pawn actor = toil.actor;
        //        Job curJob = actor.jobs.curJob;
        //        JobDriver_DoReligionActivity curDriver = (JobDriver_DoReligionActivity)actor.jobs.curDriver;
        //        UnfinishedThing thing1 = curJob.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
        //        if (thing1 != null && thing1.Destroyed)
        //        {
        //            actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
        //        }
        //        else
        //        {
        //            ++curDriver.ticksSpentDoingRecipeWork;
        //            curJob.bill.Notify_PawnDidWork(actor);
        //            IBillGiverWithTickAction thing2 = toil.actor.CurJob.GetTarget(TargetIndex.A).Thing as IBillGiverWithTickAction;
        //            if (thing2 != null)
        //                thing2.UsedThisTick();
        //            if (curJob.RecipeDef.workSkill != null && curJob.RecipeDef.UsesUnfinishedThing)
        //                actor.skills.Learn(curJob.RecipeDef.workSkill, 0.1f * curJob.RecipeDef.workSkillLearnFactor, false);
        //            float num1 = curJob.RecipeDef.workSpeedStat != null ? actor.GetStatValue(curJob.RecipeDef.workSpeedStat, true) : 1f;
        //            if (curJob.RecipeDef.workTableSpeedStat != null)
        //            {
        //                Building_ReligiousBuildingFacility facility = curDriver.TargetFacility as Building_ReligiousBuildingFacility;
        //                if (facility != null)
        //                    num1 *= facility.GetStatValue(curJob.RecipeDef.workTableSpeedStat, true);
        //            }
        //            if (DebugSettings.fastCrafting)
        //                num1 *= 30f;
        //            curDriver.workLeft -= num1;
        //            if (thing1 != null)
        //                thing1.workLeft = curDriver.workLeft;
        //            actor.GainComfortFromCellIfPossible();
        //            if ((double)curDriver.workLeft <= 0.0)
        //                curDriver.ReadyForNextToil();
        //            if (!curJob.bill.recipe.UsesUnfinishedThing)
        //                return;
        //            int num2 = Find.TickManager.TicksGame - curDriver.billStartTick;
        //            if (num2 < 3000 || num2 % 1000 != 0)
        //                return;
        //            actor.jobs.CheckForJobOverride();
        //        }
        //    });
        //    toil.defaultCompleteMode = ToilCompleteMode.Never;
        //    toil.WithEffect((Func<EffecterDef>)(() => toil.actor.CurJob.bill.recipe.effectWorking), TargetIndex.A);
        //    toil.PlaySustainerOrSound((Func<SoundDef>)(() => toil.actor.CurJob.bill.recipe.soundWorking));
        //    toil.WithProgressBar(TargetIndex.A, (Func<float>)(() =>
        //    {
        //        Pawn actor = toil.actor;
        //        Job curJob = actor.CurJob;
        //        UnfinishedThing thing = curJob.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
        //        return (float)(1.0 - (double)((JobDriver_DoReligionActivity)actor.jobs.curDriver).workLeft / (double)curJob.bill.recipe.WorkAmountTotal(thing == null ? (ThingDef)null : thing.Stuff));
        //    }), false, -0.5f);
        //    toil.FailOn<Toil>((Func<bool>)(() => toil.actor.CurJob.bill.suspended));
        //    toil.activeSkill = (Func<SkillDef>)(() => toil.actor.CurJob.bill.recipe.workSkill);
        //    return toil;
        //}

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
