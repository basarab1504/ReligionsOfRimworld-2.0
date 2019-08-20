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
    class WorkGiver_DoActivityTask : WorkGiver_Scanner
    {
        private static readonly IntRange ReCheckFailedBillTicksRange = new IntRange(500, 600);
        private static List<Thing> relevantThings = new List<Thing>();
        private static HashSet<Thing> processedThings = new HashSet<Thing>();
        private static List<Thing> newRelevantThings = new List<Thing>();
        private static List<IngredientCount> ingredientsOrdered = new List<IngredientCount>();
        private static List<Thing> tmpMedicine = new List<Thing>();
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

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.listerThings.AllThings.Where(x => x is Building_ReligiousBuildingFacility);
        }

        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            Building_ReligiousBuildingFacility giver = thing as Building_ReligiousBuildingFacility;

            if ((!giver.TaskSchedule.AnyShouldDoNow && (!pawn.CanReserve((LocalTargetInfo)thing, 1, -1, (ReservationLayerDef)null, forced) || thing.IsBurning() || thing.IsForbidden(pawn))))
                return (Job)null;
            //giver.BillStack.RemoveIncompletableBills();
            return this.StartOrResumeBillJob(pawn, giver);
        }

        private Job StartOrResumeBillJob(Pawn pawn, Building_ReligiousBuildingFacility giver)
        {
            for(int day = 0; day < giver.TaskSchedule.ScheduledDays.Count(); ++day)
            {
                for (int i = 0; i < giver.TaskSchedule.ScheduledDays.ElementAt(day).Tasks.Count(); ++i)
                {
                    ActivityTask task = giver.TaskSchedule.ScheduledDays.ElementAt(day).Tasks.ElementAt(i);
                    if (/*(bill1.recipe.requiredGiverWorkType == null || bill1.recipe.requiredGiverWorkType == this.def.workType) && */ (Find.TickManager.TicksGame >= task.LastIngredientSearchFailTicks + ReCheckFailedBillTicksRange.RandomInRange || FloatMenuMakerMap.makingFor == pawn))
                    {
                        task.LastIngredientSearchFailTicks = 0;
                        if(task.ShouldDoNow() && task.PawnAllowedToStartAnew(pawn))
                        {
                            if (!TryFindBestBillIngredients(task, pawn, giver, chosenIngThings))
                            {
                                if (FloatMenuMakerMap.makingFor != pawn)
                                    task.LastIngredientSearchFailTicks = Find.TickManager.TicksGame;
                                else
                                    JobFailReason.Is("MissingMaterials".Translate(), task.Label);
                                this.chosenIngThings.Clear();
                            }
                            else
                            {
                                Job job = this.TryStartNewDoBillJob(pawn, task, giver);
                                this.chosenIngThings.Clear();
                                return job;
                            }
                        }
                    }
                }
            }
            this.chosenIngThings.Clear();
            return (Job)null;
        }

        private Job TryStartNewDoBillJob(Pawn pawn, ActivityTask task, Building_ReligiousBuildingFacility giver)
        {
            Job job1 = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, giver, (Thing)null);
            if (job1 != null)
                return job1;
            ActivityJob job2 = new ActivityJob()
            {
                def = MiscDefOf.DoReligionActivity,
                targetA = (LocalTargetInfo)((Thing)giver)
            };
            job2.targetQueueB = new List<LocalTargetInfo>(this.chosenIngThings.Count);
            job2.countQueue = new List<int>(this.chosenIngThings.Count);
            for (int index = 0; index < this.chosenIngThings.Count; ++index)
            {
                job2.targetQueueB.Add((LocalTargetInfo)this.chosenIngThings[index].Thing);
                job2.countQueue.Add(this.chosenIngThings[index].Count);
            }
            job2.haulMode = HaulMode.ToCellNonStorage;
            job2.activityTask = task;
            return job2;
        }

        private static bool TryFindBestBillIngredients(ActivityTask task, Pawn pawn, Thing giver, List<ThingCount> chosenIngThings)
        {
            chosenIngThings.Clear();
            newRelevantThings.Clear();
            if (task.Property.ThingDefsCount.Count() == 0)
                return true;
            IntVec3 rootCell = GetBillGiverRootCell(giver, pawn);
            Region rootReg = rootCell.GetRegion(pawn.Map, RegionType.Set_Passable);
            if (rootReg == null)
                return false;
            relevantThings.Clear();
            processedThings.Clear();
            bool foundAll = false;
            Predicate<Thing> baseValidator = (Predicate<Thing>)(t =>
            {
                if (t.Spawned && !t.IsForbidden(pawn) && ((double)(t.Position - giver.Position).LengthHorizontalSquared < (double)task.IngredientSearchRadius * (double)task.IngredientSearchRadius && task.ThingFilter.Allows(t.def)))
                    return pawn.CanReserve((LocalTargetInfo)t, 1, -1, (ReservationLayerDef)null, false);
                return false;
            });
            TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
            RegionEntryPredicate entryCondition = (RegionEntryPredicate)((from, r) => r.Allows(traverseParams, false));
            int adjacentRegionsAvailable = rootReg.Neighbors.Count<Region>((Func<Region, bool>)(region => entryCondition(rootReg, region)));
            int regionsProcessed = 0;
            processedThings.AddRange<Thing>(relevantThings);
            RegionProcessor regionProcessor = (RegionProcessor)(r =>
            {
                List<Thing> thingList = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
                for (int index = 0; index < thingList.Count; ++index)
                {
                    Thing thing = thingList[index];
                    if (!processedThings.Contains(thing) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, pawn) && baseValidator(thing))
                    {
                        newRelevantThings.Add(thing);
                        processedThings.Add(thing);
                    }
                }
                ++regionsProcessed;
                if (newRelevantThings.Count > 0 && regionsProcessed > adjacentRegionsAvailable)
                {
                    Comparison<Thing> comparison = (Comparison<Thing>)((t1, t2) => ((float)(t1.Position - rootCell).LengthHorizontalSquared).CompareTo((float)(t2.Position - rootCell).LengthHorizontalSquared));
                    newRelevantThings.Sort(comparison);
                    relevantThings.AddRange((IEnumerable<Thing>)newRelevantThings);
                    newRelevantThings.Clear();
                    if (TryFindBestBillIngredientsInSet(relevantThings, task, chosenIngThings))
                    {
                        foundAll = true;
                        return true;
                    }
                }
                return false;
            });
            RegionTraverser.BreadthFirstTraverse(rootReg, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
            relevantThings.Clear();
            newRelevantThings.Clear();
            processedThings.Clear();
            ingredientsOrdered.Clear();
            return foundAll;
        }

        private static bool TryFindBestBillIngredientsInSet(List<Thing> availableThings, ActivityTask task, List<ThingCount> chosenIngThings)
        {
            IEnumerable<KeyValuePair<ThingDef, int>> thingDefsCount = task.Property.ThingDefsCount;
            chosenIngThings.Clear();

            for (int index1 = 0; index1 < thingDefsCount.Count(); ++index1)
            {
                float baseCount = thingDefsCount.ElementAt(index1).Value;
                for (int index2 = 0; index2 < availableThings.Count; ++index2)
                {
                    Thing availableThing = availableThings[index2];
                    if (task.ThingFilter.Allows(availableThing.def))
                    {
                        float num = task.Property.IngredientValueGetter.ValuePerUnitOf(availableThing.def);
                        int countToAdd = Mathf.Min(Mathf.CeilToInt(baseCount / num), availableThing.stackCount);
                        ThingCountUtility.AddToList(chosenIngThings, availableThing, countToAdd);
                        baseCount -= (float)countToAdd * num;
                        if ((double)baseCount <= 9.99999974737875E-05)
                            break;
                    }
                }
                if ((double)baseCount > 9.99999974737875E-05)
                    return false;
            }
            return true;
        }

        private static IntVec3 GetBillGiverRootCell(Thing giver, Pawn forPawn)
        {
            Building building = giver as Building;
            if (building == null)
                return giver.Position;
            if (building.def.hasInteractionCell)
                return building.InteractionCell;
            Log.Error("Tried to find task ingredients for " + (object)giver + " which has no interaction cell.", false);
            return forPawn.Position;
        }
    }
}
