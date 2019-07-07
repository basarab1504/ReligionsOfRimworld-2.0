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
    public static class FindThingsForActivityUtility
    {
        private static List<Thing> relevantThings = new List<Thing>();
        private static Dictionary<ThingDef, int> defCount = new Dictionary<ThingDef, int>();

        public static bool TryFindBestBillIngredients(ReligionActivityTask task, Pawn pawn, Building_ReligiousBuildingFacility facility, List<ThingCount> chosenThings)
        {
            //chosen.Clear();
            //WorkGiver_DoBill.newRelevantThings.Clear();
            if (task.FixedFilter.AllowedDefCount == 0)
                return true;
            IntVec3 rootCell = GetRootPosition(facility);
            Region rootReg = rootCell.GetRegion(pawn.Map, RegionType.Set_Passable);
            if (rootReg == null)
                return false;
            FindThingsForActivityUtility.relevantThings.Clear();
            //WorkGiver_DoBill.processedThings.Clear();
            bool foundAll = false;
            Predicate<Thing> baseValidator = (Predicate<Thing>)(t =>
            {
                if (t.Spawned && !t.IsForbidden(pawn) && ((double)(t.Position - facility.Position).LengthHorizontalSquared < (double)task.ThingSearchRadius * (double)task.ThingSearchRadius && task.FixedFilter.Allows(t)))
                    return pawn.CanReserve((LocalTargetInfo)t, 1, -1, (ReservationLayerDef)null, false);
                return false;
            });
            //bool billGiverIsPawn = facility is Pawn;
            //if (billGiverIsPawn)
            //{
            //    WorkGiver_DoBill.AddEveryMedicineToRelevantThings(pawn, facility, WorkGiver_DoBill.FindThingsForActivityUtility.relevantThings, baseValidator, pawn.Map);
            //    if (WorkGiver_DoBill.TryFindBestBillIngredientsInSet(WorkGiver_DoBill.FindThingsForActivityUtility.relevantThings, task, chosen))
            //    {
            //        WorkGiver_DoBill.FindThingsForActivityUtility.relevantThings.Clear();
            //        WorkGiver_DoBill.ingredientsOrdered.Clear();
            //        return true;
            //    }
            //}
            TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
            RegionEntryPredicate entryCondition = (RegionEntryPredicate)((from, r) => r.Allows(traverseParams, false));
            int adjacentRegionsAvailable = rootReg.Neighbors.Count<Region>((Func<Region, bool>)(region => entryCondition(rootReg, region)));
            int regionsProcessed = 0;
            //WorkGiver_DoBill.processedThings.AddRange<Thing>(WorkGiver_DoBill.FindThingsForActivityUtility.relevantThings);
            RegionProcessor regionProcessor = (RegionProcessor)(r =>
            {
                List<Thing> thingList = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
                for (int index = 0; index < thingList.Count; ++index)
                {
                    Thing thing = thingList[index];
                    if (!FindThingsForActivityUtility.relevantThings.Contains(thing) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, pawn) && baseValidator(thing))
                    {
                        FindThingsForActivityUtility.relevantThings.Add(thing);
                        //WorkGiver_DoBill.newRelevantThings.Add(thing);
                        //WorkGiver_DoBill.processedThings.Add(thing);
                    }
                }
                ++regionsProcessed;
                if (FindThingsForActivityUtility.relevantThings.Count > 0 && regionsProcessed > adjacentRegionsAvailable)
                {
                    Comparison<Thing> comparison = (Comparison<Thing>)((t1, t2) => ((float)(t1.Position - rootCell).LengthHorizontalSquared).CompareTo((float)(t2.Position - rootCell).LengthHorizontalSquared));
                    FindThingsForActivityUtility.relevantThings.Sort(comparison);
                    //WorkGiver_DoBill.FindThingsForActivityUtility.relevantThings.AddRange((IEnumerable<Thing>)WorkGiver_DoBill.newRelevantThings);
                    //WorkGiver_DoBill.newRelevantThings.Clear();
                    if (TryFindBestTaskThingsInSet(task, chosenThings))
                    {
                        foundAll = true;
                        return true;
                    }
                }
                return false;
            });
            RegionTraverser.BreadthFirstTraverse(rootReg, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
            FindThingsForActivityUtility.relevantThings.Clear();
            //WorkGiver_DoBill.FindThingsForActivityUtility.relevantThings.Clear();
            //WorkGiver_DoBill.newRelevantThings.Clear();
            //WorkGiver_DoBill.processedThings.Clear();
            //WorkGiver_DoBill.ingredientsOrdered.Clear();
            return foundAll;
        }

        private static bool TryFindBestTaskThingsInSet(ReligionActivityTask task, List<ThingCount> chosenThings)
        {
            InitializeSetsByThingDefs();
            chosenThings.Clear();

            for (int index = 0; index < defCount.Count; ++index)
            {
                ThingDef temp = defCount.Keys.ElementAt(index);

                int f = task.GetCount(temp);

                if (f <= defCount[temp]/*&& task.FixedFilter.Allows(FindThingsForActivityUtility.relevantThings[index])*/)
                {
                    bool flag = false;
                    for (int index2 = 0; index2 < FindThingsForActivityUtility.relevantThings.Count; ++index2)
                    {
                        if (FindThingsForActivityUtility.relevantThings[index2].def == temp)
                        {
                            int b = FindThingsForActivityUtility.relevantThings[index2].stackCount - ThingCountUtility.CountOf(chosenThings, FindThingsForActivityUtility.relevantThings[index2]);
                            if (b > 0)
                            {
                                int countToAdd = Mathf.Min(Mathf.FloorToInt(f), b);
                                ThingCountUtility.AddToList(chosenThings, FindThingsForActivityUtility.relevantThings[index2], countToAdd);
                                f -= countToAdd;
                                if (f == 0)
                                {
                                    defCount[temp] -= task.GetCount(temp);
                                    break;
                                }
                            }
                        }
                    }
                    if (flag)
                        break;
                }
                if (!flag)
                    return false;
            }
            return true;
        }

        private static void InitializeSetsByThingDefs()
        {
            defCount.Clear();
            foreach (Thing thing in FindThingsForActivityUtility.relevantThings)
            {
                if (!defCount.ContainsKey(thing.def))
                    defCount.Add(thing.def, thing.stackCount);
                else
                    defCount[thing.def] += thing.stackCount;
            }
        }

        private static IntVec3 GetRootPosition(Building building)
        {
            if (building.def.hasInteractionCell)
                return building.InteractionCell;
            return building.Position;
        }
    }
}
