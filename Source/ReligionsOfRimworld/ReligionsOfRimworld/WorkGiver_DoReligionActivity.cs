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
        private static readonly IntRange ReCheckFailedBillTicksRange = new IntRange(500, 600);
        private static List<Thing> relevantThings = new List<Thing>();
        private static HashSet<Thing> processedThings = new HashSet<Thing>();
        private static List<Thing> newRelevantThings = new List<Thing>();
        private static List<IngredientCount> ingredientsOrdered = new List<IngredientCount>();
        private static List<Thing> tmpMedicine = new List<Thing>();
        private static WorkGiver_DoReligionActivity.DefCountList availableCounts = new WorkGiver_DoReligionActivity.DefCountList();
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

        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
            }
        }

        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            if(!(thing is Building_ReligiousBuildingFacility))
                return (Job)null;

            IBillGiver giver = thing as IBillGiver;

            if ((!giver.BillStack.AnyShouldDoNow || !giver.UsableForBillsAfterFueling()) || (!pawn.CanReserve((LocalTargetInfo)thing, 1, -1, (ReservationLayerDef)null, forced) || thing.IsBurning() || thing.IsForbidden(pawn)))
                return (Job)null;
            giver.BillStack.RemoveIncompletableBills();
            return this.StartOrResumeBillJob(pawn, giver);
        }

        //private static UnfinishedThing ClosestUnfinishedThingForBill(Pawn pawn, Bill_ProductionWithUft bill)
        //{
        //    Predicate<Thing> validator = (Predicate<Thing>)(t =>
        //    {
        //        if (!t.IsForbidden(pawn) && ((UnfinishedThing)t).Recipe == bill.recipe && (((UnfinishedThing)t).Creator == pawn && ((UnfinishedThing)t).ingredients.TrueForAll((Predicate<Thing>)(x => bill.IsFixedOrAllowedIngredient(x.def)))))
        //            return pawn.CanReserve((LocalTargetInfo)t, 1, -1, (ReservationLayerDef)null, false);
        //        return false;
        //    });
        //    return (UnfinishedThing)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(bill.recipe.unfinishedThingDef), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false), 9999f, validator, (IEnumerable<Thing>)null, 0, -1, false, RegionType.Set_Passable, false);
        //}

        //private static Job FinishUftJob(Pawn pawn, UnfinishedThing uft, Bill_ProductionWithUft bill)
        //{
        //    if (uft.Creator != pawn)
        //    {
        //        Log.Error("Tried to get FinishUftJob for " + (object)pawn + " finishing " + (object)uft + " but its creator is " + (object)uft.Creator, false);
        //        return (Job)null;
        //    }
        //    Job job = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, bill.billStack.billGiver, (Thing)uft);
        //    if (job != null && job.targetA.Thing != uft)
        //        return job;
        //    return new Job(JobDefOf.DoBill, (LocalTargetInfo)((Thing)bill.billStack.billGiver))
        //    {
        //        bill = (Bill)bill,
        //        targetQueueB = new List<LocalTargetInfo>()
        //{
        //  (LocalTargetInfo) ((Thing) uft)
        //},
        //        countQueue = new List<int>() { 1 },
        //        haulMode = HaulMode.ToCellNonStorage
        //    };
        //}

        private Job StartOrResumeBillJob(Pawn pawn, IBillGiver giver)
        {
            for (int index = 0; index < giver.BillStack.Count; ++index)
            {
                Bill bill1 = giver.BillStack[index];
                if ((bill1.recipe.requiredGiverWorkType == null || bill1.recipe.requiredGiverWorkType == this.def.workType) && (Find.TickManager.TicksGame >= bill1.lastIngredientSearchFailTicks + WorkGiver_DoReligionActivity.ReCheckFailedBillTicksRange.RandomInRange || FloatMenuMakerMap.makingFor == pawn))
                {
                    bill1.lastIngredientSearchFailTicks = 0;
                    if (bill1.ShouldDoNow() && bill1.PawnAllowedToStartAnew(pawn))
                    {
                        Religion religion = ((Building_ReligiousBuildingFacility)giver).AssignedReligion;
                        if (pawn.GetReligionComponent().Religion != religion)
                        {
                            JobFailReason.Is("Religion_WrongReligion".Translate(pawn.GetReligionComponent().Religion.Label));
                        }
                        else
                        {
                            SkillRequirement skillRequirement = bill1.recipe.FirstSkillRequirementPawnDoesntSatisfy(pawn);
                            if (skillRequirement != null)
                            {
                                JobFailReason.Is("UnderRequiredSkill".Translate((NamedArgument)skillRequirement.minLevel), bill1.Label);
                            }
                            else
                            {
                                //Bill_ProductionWithUft bill2 = bill1 as Bill_ProductionWithUft;
                                //if (bill2 != null)
                                //{
                                //    if (bill2.BoundUft != null)
                                //    {
                                //        if (bill2.BoundWorker == pawn && pawn.CanReserveAndReach((LocalTargetInfo)((Thing)bill2.BoundUft), PathEndMode.Touch, Danger.Deadly, 1, -1, (ReservationLayerDef)null, false) && !bill2.BoundUft.IsForbidden(pawn))
                                //            return WorkGiver_ff.FinishUftJob(pawn, bill2.BoundUft, bill2);
                                //        continue;
                                //    }
                                //    UnfinishedThing uft = WorkGiver_ff.ClosestUnfinishedThingForBill(pawn, bill2);
                                //    if (uft != null)
                                //        return WorkGiver_ff.FinishUftJob(pawn, uft, bill2);
                                //}
                                if (!WorkGiver_DoReligionActivity.TryFindBestBillIngredients(bill1, pawn, (Thing)giver, this.chosenIngThings))
                                {
                                    if (FloatMenuMakerMap.makingFor != pawn)
                                        bill1.lastIngredientSearchFailTicks = Find.TickManager.TicksGame;
                                    else
                                        JobFailReason.Is("MissingMaterials".Translate(), bill1.Label);
                                    this.chosenIngThings.Clear();
                                }
                                else
                                {
                                    Job job = this.TryStartNewDoBillJob(pawn, bill1, giver);
                                    this.chosenIngThings.Clear();
                                    return job;
                                }
                            }
                        }
                    }
                }
            }
            this.chosenIngThings.Clear();
            return (Job)null;
        }

        private Job TryStartNewDoBillJob(Pawn pawn, Bill bill, IBillGiver giver)
        {
            Job job1 = WorkGiverUtility.HaulStuffOffBillGiverJob(pawn, giver, (Thing)null);
            if (job1 != null)
                return job1;
            Job job2 = new Job(MiscDefOf.DoReligionActivity, (LocalTargetInfo)((Thing)giver));
            job2.targetQueueB = new List<LocalTargetInfo>(this.chosenIngThings.Count);
            job2.countQueue = new List<int>(this.chosenIngThings.Count);
            for (int index = 0; index < this.chosenIngThings.Count; ++index)
            {
                job2.targetQueueB.Add((LocalTargetInfo)this.chosenIngThings[index].Thing);
                job2.countQueue.Add(this.chosenIngThings[index].Count);
            }
            job2.haulMode = HaulMode.ToCellNonStorage;
            job2.bill = bill;
            return job2;
        }

        private static bool TryFindBestBillIngredients(Bill bill, Pawn pawn, Thing billGiver, List<ThingCount> chosen)
        {
            chosen.Clear();
            WorkGiver_DoReligionActivity.newRelevantThings.Clear();
            if (bill.recipe.ingredients.Count == 0)
                return true;
            IntVec3 rootCell = WorkGiver_DoReligionActivity.GetBillGiverRootCell(billGiver, pawn);
            Region rootReg = rootCell.GetRegion(pawn.Map, RegionType.Set_Passable);
            if (rootReg == null)
                return false;
            WorkGiver_DoReligionActivity.MakeIngredientsListInProcessingOrder(WorkGiver_DoReligionActivity.ingredientsOrdered, bill);
            WorkGiver_DoReligionActivity.relevantThings.Clear();
            WorkGiver_DoReligionActivity.processedThings.Clear();
            bool foundAll = false;
            Predicate<Thing> baseValidator = (Predicate<Thing>)(t =>
            {
                if (t.Spawned && !t.IsForbidden(pawn) && ((double)(t.Position - billGiver.Position).LengthHorizontalSquared < (double)bill.ingredientSearchRadius * (double)bill.ingredientSearchRadius && bill.IsFixedOrAllowedIngredient(t) && bill.recipe.ingredients.Any<IngredientCount>((Predicate<IngredientCount>)(ingNeed => ingNeed.filter.Allows(t)))))
                    return pawn.CanReserve((LocalTargetInfo)t, 1, -1, (ReservationLayerDef)null, false);
                return false;
            });
            bool billGiverIsPawn = billGiver is Pawn;
            if (billGiverIsPawn)
            {
                WorkGiver_DoReligionActivity.AddEveryMedicineToRelevantThings(pawn, billGiver, WorkGiver_DoReligionActivity.relevantThings, baseValidator, pawn.Map);
                if (WorkGiver_DoReligionActivity.TryFindBestBillIngredientsInSet(WorkGiver_DoReligionActivity.relevantThings, bill, chosen))
                {
                    WorkGiver_DoReligionActivity.relevantThings.Clear();
                    WorkGiver_DoReligionActivity.ingredientsOrdered.Clear();
                    return true;
                }
            }
            TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
            RegionEntryPredicate entryCondition = (RegionEntryPredicate)((from, r) => r.Allows(traverseParams, false));
            int adjacentRegionsAvailable = rootReg.Neighbors.Count<Region>((Func<Region, bool>)(region => entryCondition(rootReg, region)));
            int regionsProcessed = 0;
            WorkGiver_DoReligionActivity.processedThings.AddRange<Thing>(WorkGiver_DoReligionActivity.relevantThings);
            RegionProcessor regionProcessor = (RegionProcessor)(r =>
            {
                List<Thing> thingList = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
                for (int index = 0; index < thingList.Count; ++index)
                {
                    Thing thing = thingList[index];
                    if (!WorkGiver_DoReligionActivity.processedThings.Contains(thing) && ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, pawn) && baseValidator(thing) && (!thing.def.IsMedicine || !billGiverIsPawn))
                    {
                        WorkGiver_DoReligionActivity.newRelevantThings.Add(thing);
                        WorkGiver_DoReligionActivity.processedThings.Add(thing);
                    }
                }
                ++regionsProcessed;
                if (WorkGiver_DoReligionActivity.newRelevantThings.Count > 0 && regionsProcessed > adjacentRegionsAvailable)
                {
                    Comparison<Thing> comparison = (Comparison<Thing>)((t1, t2) => ((float)(t1.Position - rootCell).LengthHorizontalSquared).CompareTo((float)(t2.Position - rootCell).LengthHorizontalSquared));
                    WorkGiver_DoReligionActivity.newRelevantThings.Sort(comparison);
                    WorkGiver_DoReligionActivity.relevantThings.AddRange((IEnumerable<Thing>)WorkGiver_DoReligionActivity.newRelevantThings);
                    WorkGiver_DoReligionActivity.newRelevantThings.Clear();
                    if (WorkGiver_DoReligionActivity.TryFindBestBillIngredientsInSet(WorkGiver_DoReligionActivity.relevantThings, bill, chosen))
                    {
                        foundAll = true;
                        return true;
                    }
                }
                return false;
            });
            RegionTraverser.BreadthFirstTraverse(rootReg, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
            WorkGiver_DoReligionActivity.relevantThings.Clear();
            WorkGiver_DoReligionActivity.newRelevantThings.Clear();
            WorkGiver_DoReligionActivity.processedThings.Clear();
            WorkGiver_DoReligionActivity.ingredientsOrdered.Clear();
            return foundAll;
        }

        private static IntVec3 GetBillGiverRootCell(Thing billGiver, Pawn forPawn)
        {
            Building building = billGiver as Building;
            if (building == null)
                return billGiver.Position;
            if (building.def.hasInteractionCell)
                return building.InteractionCell;
            Log.Error("Tried to find bill ingredients for " + (object)billGiver + " which has no interaction cell.", false);
            return forPawn.Position;
        }

        private static void AddEveryMedicineToRelevantThings(Pawn pawn, Thing billGiver, List<Thing> relevantThings, Predicate<Thing> baseValidator, Map map)
        {
            MedicalCareCategory medicalCareCategory = WorkGiver_DoReligionActivity.GetMedicalCareCategory(billGiver);
            List<Thing> thingList = map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine);
            WorkGiver_DoReligionActivity.tmpMedicine.Clear();
            for (int index = 0; index < thingList.Count; ++index)
            {
                Thing thing = thingList[index];
                if (medicalCareCategory.AllowsMedicine(thing.def) && baseValidator(thing) && pawn.CanReach((LocalTargetInfo)thing, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
                    WorkGiver_DoReligionActivity.tmpMedicine.Add(thing);
            }
            WorkGiver_DoReligionActivity.tmpMedicine.SortBy<Thing, float, int>((Func<Thing, float>)(x => -x.GetStatValue(StatDefOf.MedicalPotency, true)), (Func<Thing, int>)(x => x.Position.DistanceToSquared(billGiver.Position)));
            relevantThings.AddRange((IEnumerable<Thing>)WorkGiver_DoReligionActivity.tmpMedicine);
            WorkGiver_DoReligionActivity.tmpMedicine.Clear();
        }

        private static MedicalCareCategory GetMedicalCareCategory(Thing billGiver)
        {
            Pawn pawn = billGiver as Pawn;
            if (pawn != null && pawn.playerSettings != null)
                return pawn.playerSettings.medCare;
            return MedicalCareCategory.Best;
        }

        private static void MakeIngredientsListInProcessingOrder(List<IngredientCount> ingredientsOrdered, Bill bill)
        {
            ingredientsOrdered.Clear();
            if (bill.recipe.productHasIngredientStuff)
                ingredientsOrdered.Add(bill.recipe.ingredients[0]);
            for (int index = 0; index < bill.recipe.ingredients.Count; ++index)
            {
                if (!bill.recipe.productHasIngredientStuff || index != 0)
                {
                    IngredientCount ingredient = bill.recipe.ingredients[index];
                    if (ingredient.IsFixedIngredient)
                        ingredientsOrdered.Add(ingredient);
                }
            }
            for (int index = 0; index < bill.recipe.ingredients.Count; ++index)
            {
                IngredientCount ingredient = bill.recipe.ingredients[index];
                if (!ingredientsOrdered.Contains(ingredient))
                    ingredientsOrdered.Add(ingredient);
            }
        }

        private static bool TryFindBestBillIngredientsInSet(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
        {
            if (bill.recipe.allowMixingIngredients)
                return WorkGiver_DoReligionActivity.TryFindBestBillIngredientsInSet_AllowMix(availableThings, bill, chosen);
            return WorkGiver_DoReligionActivity.TryFindBestBillIngredientsInSet_NoMix(availableThings, bill, chosen);
        }

        private static bool TryFindBestBillIngredientsInSet_NoMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
        {
            RecipeDef recipe = bill.recipe;
            chosen.Clear();
            WorkGiver_DoReligionActivity.availableCounts.Clear();
            WorkGiver_DoReligionActivity.availableCounts.GenerateFrom(availableThings);
            for (int index1 = 0; index1 < WorkGiver_DoReligionActivity.ingredientsOrdered.Count; ++index1)
            {
                IngredientCount ingredient = recipe.ingredients[index1];
                bool flag = false;
                for (int index2 = 0; index2 < WorkGiver_DoReligionActivity.availableCounts.Count; ++index2)
                {
                    float f = (float)ingredient.CountRequiredOfFor(WorkGiver_DoReligionActivity.availableCounts.GetDef(index2), bill.recipe);
                    if ((double)f <= (double)WorkGiver_DoReligionActivity.availableCounts.GetCount(index2) && ingredient.filter.Allows(WorkGiver_DoReligionActivity.availableCounts.GetDef(index2)) && (ingredient.IsFixedIngredient || bill.ingredientFilter.Allows(WorkGiver_DoReligionActivity.availableCounts.GetDef(index2))))
                    {
                        for (int index3 = 0; index3 < availableThings.Count; ++index3)
                        {
                            if (availableThings[index3].def == WorkGiver_DoReligionActivity.availableCounts.GetDef(index2))
                            {
                                int b = availableThings[index3].stackCount - ThingCountUtility.CountOf(chosen, availableThings[index3]);
                                if (b > 0)
                                {
                                    int countToAdd = Mathf.Min(Mathf.FloorToInt(f), b);
                                    ThingCountUtility.AddToList(chosen, availableThings[index3], countToAdd);
                                    f -= (float)countToAdd;
                                    if ((double)f < 1.0 / 1000.0)
                                    {
                                        flag = true;
                                        float val = WorkGiver_DoReligionActivity.availableCounts.GetCount(index2) - (float)ingredient.CountRequiredOfFor(WorkGiver_DoReligionActivity.availableCounts.GetDef(index2), bill.recipe);
                                        WorkGiver_DoReligionActivity.availableCounts.SetCount(index2, val);
                                        break;
                                    }
                                }
                            }
                        }
                        if (flag)
                            break;
                    }
                }
                if (!flag)
                    return false;
            }
            return true;
        }

        private static bool TryFindBestBillIngredientsInSet_AllowMix(List<Thing> availableThings, Bill bill, List<ThingCount> chosen)
        {
            chosen.Clear();
            for (int index1 = 0; index1 < bill.recipe.ingredients.Count; ++index1)
            {
                IngredientCount ingredient = bill.recipe.ingredients[index1];
                float baseCount = ingredient.GetBaseCount();
                for (int index2 = 0; index2 < availableThings.Count; ++index2)
                {
                    Thing availableThing = availableThings[index2];
                    if (ingredient.filter.Allows(availableThing) && (ingredient.IsFixedIngredient || bill.ingredientFilter.Allows(availableThing)))
                    {
                        float num = bill.recipe.IngredientValueGetter.ValuePerUnitOf(availableThing.def);
                        int countToAdd = Mathf.Min(Mathf.CeilToInt(baseCount / num), availableThing.stackCount);
                        ThingCountUtility.AddToList(chosen, availableThing, countToAdd);
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

        private class DefCountList
        {
            private List<ThingDef> defs = new List<ThingDef>();
            private List<float> counts = new List<float>();

            public int Count
            {
                get
                {
                    return this.defs.Count;
                }
            }

            public float this[ThingDef def]
            {
                get
                {
                    int index = this.defs.IndexOf(def);
                    if (index < 0)
                        return 0.0f;
                    return this.counts[index];
                }
                set
                {
                    int index = this.defs.IndexOf(def);
                    if (index < 0)
                    {
                        this.defs.Add(def);
                        this.counts.Add(value);
                        index = this.defs.Count - 1;
                    }
                    else
                        this.counts[index] = value;
                    this.CheckRemove(index);
                }
            }

            public float GetCount(int index)
            {
                return this.counts[index];
            }

            public void SetCount(int index, float val)
            {
                this.counts[index] = val;
                this.CheckRemove(index);
            }

            public ThingDef GetDef(int index)
            {
                return this.defs[index];
            }

            private void CheckRemove(int index)
            {
                if ((double)this.counts[index] != 0.0)
                    return;
                this.counts.RemoveAt(index);
                this.defs.RemoveAt(index);
            }

            public void Clear()
            {
                this.defs.Clear();
                this.counts.Clear();
            }

            public void GenerateFrom(List<Thing> things)
            {
                this.Clear();
                for (int index = 0; index < things.Count; ++index)
                    this[things[index].def] += (float)things[index].stackCount;
            }
        }
    }
}
