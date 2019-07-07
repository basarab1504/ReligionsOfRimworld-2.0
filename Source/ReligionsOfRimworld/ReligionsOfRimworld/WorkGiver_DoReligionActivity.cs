using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class WorkGiver_DoReligionActivity : WorkGiver_Scanner
    {
        private static readonly IntRange reCheckFailedBillTicksRange = new IntRange(500, 600);
        private List<ThingCount> chosenIngThings = new List<ThingCount>();

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.InteractionCell;
            }
        }

        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Some;
        }

        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);

        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            Building_ReligiousBuildingFacility facility = thing as Building_ReligiousBuildingFacility;
            if (facility == null || !facility.TaskStack.AnyShouldDoNow || (!pawn.CanReserve((LocalTargetInfo)thing, 1, -1, (ReservationLayerDef)null, forced) || thing.IsBurning() || thing.IsForbidden(pawn)))
                return (Job)null;
            //facility.TaskStack.RemoveIncompletableBills();
            return this.StartOrResumeBillJob(pawn, facility);
        }

        private Job StartOrResumeBillJob(Pawn pawn, Building_ReligiousBuildingFacility facility)
        {
            for (int index = 0; index < facility.TaskStack.Count; ++index)
            {
                ReligionActivityTask task1 = facility.TaskStack[index];
                if ((Find.TickManager.TicksGame >= task1.LastIngredientSearchFailTicks + reCheckFailedBillTicksRange.RandomInRange || FloatMenuMakerMap.makingFor == pawn))
                {
                    task1.LastIngredientSearchFailTicks = 0;
                    if (task1.ShouldDoNow && task1.PawnAllowedToStartAnew(pawn))
                    {
                        //SkillRequirement skillRequirement = task1.recipe.FirstSkillRequirementPawnDoesntSatisfy(pawn);
                        //if (skillRequirement != null)
                        //{
                        //    JobFailReason.Is("UnderRequiredSkill".Translate((NamedArgument)skillRequirement.minLevel), task1.Label);
                        //}
                        //else
                        //{
                        //Bill_ProductionWithUft bill2 = task1 as Bill_ProductionWithUft;
                        //if (bill2 != null)
                        //{
                        //    if (bill2.BoundUft != null)
                        //    {
                        //        if (bill2.BoundWorker == pawn && pawn.CanReserveAndReach((LocalTargetInfo)((Thing)bill2.BoundUft), PathEndMode.Touch, Danger.Deadly, 1, -1, (ReservationLayerDef)null, false) && !bill2.BoundUft.IsForbidden(pawn))
                        //            return WorkGiver_DoBill.FinishUftJob(pawn, bill2.BoundUft, bill2);
                        //        continue;
                        //    }
                        //    UnfinishedThing uft = WorkGiver_DoBill.ClosestUnfinishedThingForBill(pawn, bill2);
                        //    if (uft != null)
                        //        return WorkGiver_DoBill.FinishUftJob(pawn, uft, bill2);
                        //}
                        if (!FindThingsForActivityUtility.TryFindBestBillIngredients(task1, pawn, facility, this.chosenIngThings))
                        {
                            Log.Message("PLEASENO");
                            if (FloatMenuMakerMap.makingFor != pawn)
                                task1.LastIngredientSearchFailTicks = Find.TickManager.TicksGame;
                            else
                                JobFailReason.Is("MissingMaterials".Translate(), task1.Label);
                            this.chosenIngThings.Clear();
                        }
                        else
                        {
                            Log.Message("WEIRD");
                            Job job = this.TryStartNewDoBillJob(pawn, task1, facility);
                            this.chosenIngThings.Clear();
                            return job;
                        }
                        //}
                    }
                }
            }
            this.chosenIngThings.Clear();
            return (Job)null;
        }

        private Job TryStartNewDoBillJob(Pawn pawn, ReligionActivityTask task, Building_ReligiousBuildingFacility facility)
        {
            //Job job1 = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, facility, (Thing)null);
            //if (job1 != null)
            //    return job1;
            Job job2 = new Job(MiscDefOf.DoReligionActivity, (LocalTargetInfo)((Thing)facility));
            job2.targetQueueB = new List<LocalTargetInfo>(this.chosenIngThings.Count);
            job2.countQueue = new List<int>(this.chosenIngThings.Count);
            for (int index = 0; index < this.chosenIngThings.Count; ++index)
            {
                job2.targetQueueB.Add((LocalTargetInfo)this.chosenIngThings[index].Thing);
                job2.countQueue.Add(this.chosenIngThings[index].Count);
            }
            job2.haulMode = HaulMode.ToCellNonStorage;
            //job2.bill = bill;
            return job2;
        }

        //private static Job HaulStuffOffBillGiverJob(Pawn pawn, Building_ReligiousBuildingFacility facility, Thing thingToIgnore)
        //{
        //    foreach (IntVec3 ingredientStackCell in giver.IngredientStackCells)
        //    {
        //        Thing t = pawn.Map.thingGrid.ThingAt(ingredientStackCell, ThingCategory.Item);
        //        if (t != null && t != thingToIgnore)
        //            return HaulAIUtility.HaulAsideJobFor(pawn, t);
        //    }
        //    return (Job)null;
        //}
    }
}
