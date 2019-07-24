using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ReligionsOfRimworld
{
    public class Dialog_Bill_ReligionActivityConfig : Window
    {
        private IntVec3 billGiverPos;

        private Bill_ReligionActivity bill;

        private Religion Religion => ((Building_ReligiousBuildingFacility)bill.billStack.billGiver).AssignedReligion;

        private Vector2 thingFilterScrollPosition;

        private string repeatCountEditBuffer;

        private string targetCountEditBuffer;

        private string unpauseCountEditBuffer;

        [TweakValue("Interface", 0f, 400f)]
        private static int RepeatModeSubdialogHeight = 324;

        [TweakValue("Interface", 0f, 400f)]
        private static int StoreModeSubdialogHeight = 30;

        [TweakValue("Interface", 0f, 400f)]
        private static int WorkerSelectionSubdialogHeight = 85;

        [TweakValue("Interface", 0f, 400f)]
        private static int IngredientRadiusSubdialogHeight = 50;

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(800f, 634f);
            }
        }

        public Dialog_Bill_ReligionActivityConfig(Bill_ReligionActivity bill, IntVec3 billGiverPos)
        {
            this.billGiverPos = billGiverPos;
            this.bill = bill;
            this.forcePause = true;
            this.doCloseX = true;
            this.doCloseButton = true;
            this.absorbInputAroundWindow = true;
            this.closeOnClickedOutside = true;
        }

        private void AdjustCount(int offset)
        {
            if (offset > 0)
            {
                SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
            }
            else
            {
                SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
            }
            this.bill.repeatCount += offset;
            if (this.bill.repeatCount < 1)
            {
                this.bill.repeatCount = 1;
            }
        }

        public override void WindowUpdate()
        {
            this.bill.TryDrawIngredientSearchRadiusOnMap(this.billGiverPos);
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            Rect rect = new Rect(0f, 0f, 400f, 50f);
            Widgets.Label(rect, this.bill.LabelCap);
            float width = (float)((int)((inRect.width - 34f) / 3f));
            Rect rect2 = new Rect(0f, 80f, width, inRect.height - 80f);
            Rect rect3 = new Rect(rect2.xMax + 17f, 50f, width, inRect.height - 50f - this.CloseButSize.y);
            Rect rect4 = new Rect(rect3.xMax + 17f, 50f, 0f, inRect.height - 50f - this.CloseButSize.y);
            rect4.xMax = inRect.xMax;
            Text.Font = GameFont.Small;
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect3);
            Listing_Standard listing_Standard2 = listing_Standard.BeginSection((float)Dialog_Bill_ReligionActivityConfig.RepeatModeSubdialogHeight);
            if (listing_Standard2.ButtonText(this.bill.repeatMode.LabelCap, null))
            {
                BillRepeatModeUtility.MakeConfigFloatMenu(this.bill);
            }
            listing_Standard2.Gap(12f);
            if (this.bill.repeatMode == BillRepeatModeDefOf.RepeatCount)
            {
                listing_Standard2.Label("RepeatCount".Translate(this.bill.repeatCount), -1f, null);
                listing_Standard2.IntEntry(ref this.bill.repeatCount, ref this.repeatCountEditBuffer, 1);
            }
            else if (this.bill.repeatMode == BillRepeatModeDefOf.TargetCount)
            {
                string text = "CurrentlyHave".Translate() + ": ";
                text += this.bill.recipe.WorkerCounter.CountProducts(this.bill);
                text += " / ";
                text += ((this.bill.targetCount >= 999999) ? "Infinite".Translate().ToLower() : this.bill.targetCount.ToString());
                string str = this.bill.recipe.WorkerCounter.ProductsDescription(this.bill);
                if (!str.NullOrEmpty())
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        "\n",
                        "CountingProducts".Translate(),
                        ": ",
                        str.CapitalizeFirst()
                    });
                }
                listing_Standard2.Label(text, -1f, null);
                int targetCount = this.bill.targetCount;
                listing_Standard2.IntEntry(ref this.bill.targetCount, ref this.targetCountEditBuffer, this.bill.recipe.targetCountAdjustment);
                this.bill.unpauseWhenYouHave = Mathf.Max(0, this.bill.unpauseWhenYouHave + (this.bill.targetCount - targetCount));
                ThingDef producedThingDef = this.bill.recipe.ProducedThingDef;
                if (producedThingDef != null)
                {
                    if (producedThingDef.IsWeapon || producedThingDef.IsApparel)
                    {
                        listing_Standard2.CheckboxLabeled("IncludeEquipped".Translate(), ref this.bill.includeEquipped, null);
                    }
                    if (producedThingDef.IsApparel && producedThingDef.apparel.careIfWornByCorpse)
                    {
                        listing_Standard2.CheckboxLabeled("IncludeTainted".Translate(), ref this.bill.includeTainted, null);
                    }
                    Widgets.Dropdown<Bill_ReligionActivity, Zone_Stockpile>(listing_Standard2.GetRect(30f), this.bill, (Bill_ReligionActivity b) => b.includeFromZone, (Bill_ReligionActivity b) => this.GenerateStockpileInclusion(), (this.bill.includeFromZone != null) ? "IncludeSpecific".Translate(this.bill.includeFromZone.label) : "IncludeFromAll".Translate(), null, null, null, null, false);
                    Widgets.FloatRange(listing_Standard2.GetRect(28f), 975643279, ref this.bill.hpRange, 0f, 1f, "HitPoints", ToStringStyle.PercentZero);
                    if (producedThingDef.HasComp(typeof(CompQuality)))
                    {
                        Widgets.QualityRange(listing_Standard2.GetRect(28f), 1098906561, ref this.bill.qualityRange);
                    }
                    if (producedThingDef.MadeFromStuff)
                    {
                        listing_Standard2.CheckboxLabeled("LimitToAllowedStuff".Translate(), ref this.bill.limitToAllowedStuff, null);
                    }
                }
            }
            if (this.bill.repeatMode == BillRepeatModeDefOf.TargetCount)
            {
                listing_Standard2.CheckboxLabeled("PauseWhenSatisfied".Translate(), ref this.bill.pauseWhenSatisfied, null);
                if (this.bill.pauseWhenSatisfied)
                {
                    listing_Standard2.Label("UnpauseWhenYouHave".Translate() + ": " + this.bill.unpauseWhenYouHave.ToString("F0"), -1f, null);
                    listing_Standard2.IntEntry(ref this.bill.unpauseWhenYouHave, ref this.unpauseCountEditBuffer, this.bill.recipe.targetCountAdjustment);
                    if (this.bill.unpauseWhenYouHave >= this.bill.targetCount)
                    {
                        this.bill.unpauseWhenYouHave = this.bill.targetCount - 1;
                        this.unpauseCountEditBuffer = this.bill.unpauseWhenYouHave.ToStringCached();
                    }
                }
            }
            listing_Standard.EndSection(listing_Standard2);
            listing_Standard.Gap(12f);
            Listing_Standard listing_Standard3 = listing_Standard.BeginSection((float)Dialog_Bill_ReligionActivityConfig.StoreModeSubdialogHeight);
            string text3 = string.Format(this.bill.GetStoreMode().LabelCap, (this.bill.GetStoreZone() == null) ? string.Empty : this.bill.GetStoreZone().SlotYielderLabel());
            if (this.bill.GetStoreZone() != null && !this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone()))
            {
                text3 += string.Format(" ({0})", "IncompatibleLower".Translate());
                Text.Font = GameFont.Tiny;
            }
            if (listing_Standard3.ButtonText(text3, null))
            {
                Text.Font = GameFont.Small;
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (BillStoreModeDef current in from bsm in DefDatabase<BillStoreModeDef>.AllDefs
                                                     orderby bsm.listOrder
                                                     select bsm)
                {
                    if (current == BillStoreModeDefOf.SpecificStockpile)
                    {
                        List<SlotGroup> allGroupsListInPriorityOrder = this.bill.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
                        int count = allGroupsListInPriorityOrder.Count;
                        for (int i = 0; i < count; i++)
                        {
                            SlotGroup group = allGroupsListInPriorityOrder[i];
                            Zone_Stockpile zone_Stockpile = group.parent as Zone_Stockpile;
                            if (zone_Stockpile != null)
                            {
                                if (!this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, zone_Stockpile))
                                {
                                    list.Add(new FloatMenuOption(string.Format("{0} ({1})", string.Format(current.LabelCap, group.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null));
                                }
                                else
                                {
                                    list.Add(new FloatMenuOption(string.Format(current.LabelCap, group.parent.SlotYielderLabel()), delegate
                                    {
                                        this.bill.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, (Zone_Stockpile)group.parent);
                                    }, MenuOptionPriority.Default, null, null, 0f, null, null));
                                }
                            }
                        }
                    }
                    else
                    {
                        BillStoreModeDef smLocal = current;
                        list.Add(new FloatMenuOption(smLocal.LabelCap, delegate
                        {
                            this.bill.SetStoreMode(smLocal, null);
                        }, MenuOptionPriority.Default, null, null, 0f, null, null));
                    }
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }
            Text.Font = GameFont.Small;
            listing_Standard.EndSection(listing_Standard3);
            listing_Standard.Gap(12f);

            //if(this.bill.HumanlikeIngredient != null)
            //{
            //    Listing_Standard humanlike = listing_Standard.BeginSection((float)Dialog_Bill_ReligionActivityConfig.WorkerSelectionSubdialogHeight);
            //    Widgets.Dropdown<Bill_ReligionActivity, Pawn>(humanlike.GetRect(30f), this.bill, (Bill_ReligionActivity b) => b.HumanlikeIngredient.ConcretePawn, (Bill_ReligionActivity b) => this.GenerateIngredientPawnOptions(b.HumanlikeIngredient), (this.bill.HumanlikeIngredient.ConcretePawn != null) ? this.bill.HumanlikeIngredient.ConcretePawn.LabelShortCap : "AnyWorker".Translate(), null, null, null, null, false);
            //    Widgets.Dropdown<Bill_ReligionActivity, bool>(humanlike.GetRect(60f), this.bill, (Bill_ReligionActivity b) => b.HumanlikeIngredient.PartOfColony, (Bill_ReligionActivity b) => this.GenerateBoolOptions(b.HumanlikeIngredient), this.bill.HumanlikeIngredient.PartOfColony.ToString(), null, null, null, null, false);
            //    listing_Standard.EndSection(humanlike);
            //    listing_Standard.End();
            //}
            //if(this.bill.AnimalIngredient != null)
            //{
            //    Listing_Standard animal = listing_Standard.BeginSection((float)Dialog_Bill_ReligionActivityConfig.WorkerSelectionSubdialogHeight);
            //    Widgets.Dropdown<Bill_ReligionActivity, Pawn>(animal.GetRect(30f), this.bill, (Bill_ReligionActivity b) => b.AnimalIngredient.ConcretePawn, (Bill_ReligionActivity b) => this.GenerateAnimalOptions(), (this.bill.AnimalIngredient.ConcretePawn != null) ? this.bill.AnimalIngredient.ConcretePawn.LabelShortCap : "AnyWorker".Translate(), null, null, null, null, false);
            //    listing_Standard.EndSection(animal);
            //    listing_Standard.End();
            //}

            Listing_Standard listing_Standard4 = listing_Standard.BeginSection((float)Dialog_Bill_ReligionActivityConfig.WorkerSelectionSubdialogHeight);           
            Widgets.Dropdown<Bill_ReligionActivity, Pawn>(listing_Standard4.GetRect(30f), this.bill, (Bill_ReligionActivity b) => b.pawnRestriction, (Bill_ReligionActivity b) => this.GeneratePawnRestrictionOptions(), (this.bill.pawnRestriction != null) ? this.bill.pawnRestriction.LabelShortCap : "AnyWorker".Translate(), null, null, null, null, false);
            if (this.bill.pawnRestriction == null && this.bill.recipe.workSkill != null)
            {
                listing_Standard4.Label("AllowedSkillRange".Translate(this.bill.recipe.workSkill.label), -1f, null);
                listing_Standard4.IntRange(ref this.bill.allowedSkillRange, 0, 20);
            }
            listing_Standard.EndSection(listing_Standard4);
            listing_Standard.End();
            Rect rect5 = rect4;
            rect5.yMin = rect5.yMax - (float)Dialog_Bill_ReligionActivityConfig.IngredientRadiusSubdialogHeight;
            rect4.yMax = rect5.yMin - 17f;
            bool flag = this.bill.GetStoreZone() == null || this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone());
            Rect rect6 = rect4;
            ThingFilter ingredientFilter = this.bill.ingredientFilter;
            ThingFilter fixedIngredientFilter = this.bill.recipe.fixedIngredientFilter;
            int openMask = 4;
            IEnumerable<ThingDef> forceHiddenDefs = null;
            List<SpecialThingFilterDef> forceHiddenSpecialFilters = this.bill.recipe.forceHiddenSpecialFilters;
            List<ThingDef> premultipliedSmallIngredients = this.bill.recipe.GetPremultipliedSmallIngredients();
            ThingFilterUI.DoThingFilterConfigWindow(rect6, ref this.thingFilterScrollPosition, ingredientFilter, fixedIngredientFilter, 1, null, null, false, null, this.bill.Map);
            bool flag2 = this.bill.GetStoreZone() == null || this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, this.bill.GetStoreZone());
            if (flag && !flag2)
            {
                Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(this.bill.LabelCap, this.bill.billStack.billGiver.LabelShort.CapitalizeFirst(), this.bill.GetStoreZone().label), this.bill.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, false);
            }
            Listing_Standard listing_Standard5 = new Listing_Standard();
            listing_Standard5.Begin(rect5);
            string str2 = "IngredientSearchRadius".Translate().Truncate(rect5.width * 0.6f, null);
            string str3 = (this.bill.ingredientSearchRadius != 999f) ? this.bill.ingredientSearchRadius.ToString("F0") : "Unlimited".Translate().Truncate(rect5.width * 0.3f, null);
            listing_Standard5.Label(str2 + ": " + str3, -1f, null);
            this.bill.ingredientSearchRadius = listing_Standard5.Slider(this.bill.ingredientSearchRadius, 3f, 100f);
            if (this.bill.ingredientSearchRadius >= 100f)
            {
                this.bill.ingredientSearchRadius = 999f;
            }
            listing_Standard5.End();
            //Listing_Standard listing_Standard6 = new Listing_Standard();
            //listing_Standard6.Begin(rect2);
            //if (this.bill.suspended)
            //{
            //    if (listing_Standard6.ButtonText("Suspended".Translate(), null))
            //    {
            //        this.bill.suspended = false;
            //        SoundDefOf.Click.PlayOneShotOnCamera(null);
            //    }
            //}
            //else if (listing_Standard6.ButtonText("NotSuspended".Translate(), null))
            //{
            //    this.bill.suspended = true;
            //    SoundDefOf.Click.PlayOneShotOnCamera(null);
            //}
            //StringBuilder stringBuilder = new StringBuilder();
            //if (this.bill.recipe.description != null)
            //{
            //    stringBuilder.AppendLine(this.bill.recipe.description);
            //    stringBuilder.AppendLine();
            //}
            //stringBuilder.AppendLine("WorkAmount".Translate() + ": " + this.bill.recipe.WorkAmountTotal(null).ToStringWorkAmount());
            //for (int j = 0; j < this.bill.recipe.ingredients.Count; j++)
            //{
            //    IngredientCount ingredientCount = this.bill.recipe.ingredients[j];
            //    if (!ingredientCount.filter.Summary.NullOrEmpty())
            //    {
            //        stringBuilder.AppendLine(this.bill.recipe.IngredientValueGetter.BillRequirementsDescription(this.bill.recipe, ingredientCount));
            //    }
            //}
            //stringBuilder.AppendLine();
            //string text4 = this.bill.recipe.IngredientValueGetter.ExtraDescriptionLine(this.bill.recipe);
            //if (text4 != null)
            //{
            //    stringBuilder.AppendLine(text4);
            //    stringBuilder.AppendLine();
            //}
            //if (!this.bill.recipe.skillRequirements.NullOrEmpty<SkillRequirement>())
            //{
            //    stringBuilder.AppendLine("MinimumSkills".Translate());
            //    stringBuilder.AppendLine(this.bill.recipe.MinSkillString);
            //}
            //Text.Font = GameFont.Small;
            //string text5 = stringBuilder.ToString();
            //if (Text.CalcHeight(text5, rect2.width) > rect2.height)
            //{
            //    Text.Font = GameFont.Tiny;
            //}
            //listing_Standard6.Label(text5, -1f, null);
            //Text.Font = GameFont.Small;
            //listing_Standard6.End();
            //if (this.bill.recipe.products.Count == 1)
            //{
            //    ThingDef thingDef = this.bill.recipe.products[0].thingDef;
            //    Widgets.InfoCardButton(rect2.x, rect4.y, thingDef, GenStuff.DefaultStuffFor(thingDef));
            //}
        }

        private IEnumerable<Widgets.DropdownMenuElement<bool>> GenerateBoolOptions(IngredientPawn ingredientPawn)
        {
            yield return new Widgets.DropdownMenuElement<bool>()
            {
                option = new FloatMenuOption("True".Translate(), delegate
                {
                    ingredientPawn.PartOfColony = true;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = false
            };
            yield return new Widgets.DropdownMenuElement<bool>()
            {
                option = new FloatMenuOption("False".Translate(), delegate
                {
                    ingredientPawn.PartOfColony = false;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = false
            };
        }

        //private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GenerateIngredientPawnOptions(IngredientPawn ingredientPawn)
        //{
        //    yield return new Widgets.DropdownMenuElement<Pawn>
        //    {
        //        option = new FloatMenuOption("Anyone".Translate(), delegate
        //        {
        //            ingredientPawn.ConcretePawn = null;
        //        }, MenuOptionPriority.Default, null, null, 0f, null, null),
        //        payload = null
        //    };
        //    IEnumerable<Pawn> pawns = bill.Map.mapPawns.AllPawns.Where(x => x.RaceProps.Humanlike || x.RaceProps.Animal);
        //    foreach (Pawn pawn in pawns)
        //    {
        //        Log.Message(ingredientPawn.Filter.AllowedDefCount.ToString());
        //        if (ingredientPawn.Filter.Allows(pawn.def))
        //        {
        //            Log.Message("TRUE");
        //            if (ingredientPawn.PartOfColony)
        //            {
        //                if (pawn.RaceProps.Humanlike && pawn.IsPrisonerOfColony || pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)
        //                {
        //                    yield return new Widgets.DropdownMenuElement<Pawn>
        //                    {
        //                        option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate
        //                        {
        //                            ingredientPawn.ConcretePawn = pawn;
        //                        }, MenuOptionPriority.Default, null, null, 0f, null, null),
        //                        payload = pawn
        //                    };
        //                }
        //            }
        //            else
        //            {
        //                yield return new Widgets.DropdownMenuElement<Pawn>
        //                {
        //                    option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate
        //                    {
        //                        ingredientPawn.ConcretePawn = pawn;
        //                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
        //                    payload = pawn
        //                };
        //            }
        //        }
        //    }
        //}

        private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GeneratePawnRestrictionOptions()
        {
            yield return new Widgets.DropdownMenuElement<Pawn>
            {
                option = new FloatMenuOption("AnyWorker".Translate(), delegate
                {
                    this.bill.pawnRestriction = null;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = null
            };
            SkillDef workSkill = this.bill.recipe.workSkill;
            IEnumerable<Pawn> pawns = PawnsFinder.AllMaps_FreeColonists;
            pawns = from pawn in pawns
                    orderby pawn.LabelShortCap
                    select pawn;
            if (workSkill != null)
            {
                pawns = from pawn in pawns
                        orderby pawn.skills.GetSkill(this.bill.recipe.workSkill).Level descending
                        select pawn;
            }
            WorkGiverDef workGiver = GetWorkgiver(this.bill.billStack.billGiver);
            if (workGiver == null)
            {
                Log.ErrorOnce("Generating pawn restrictions for a BillGiver without a Workgiver", 96455148, false);
            }
            else
            {
                pawns = from pawn in pawns
                        orderby pawn.workSettings.WorkIsActive(workGiver.workType) descending
                        select pawn;
                pawns = from pawn in pawns
                        orderby pawn.story.WorkTypeIsDisabled(workGiver.workType)
                        select pawn;
                foreach (Pawn pawn in pawns)
                {
                    if (pawn.GetReligionComponent().Religion != Religion)
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "Religion_WrongReligion".Translate(pawn.GetReligionComponent().Religion.Label)), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    if (pawn.story.WorkTypeIsDisabled(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(workGiver.verb)), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    if (this.bill.recipe.workSkill != null && !pawn.workSettings.WorkIsActive(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", new object[]
                            {
                                pawn.LabelShortCap,
                                pawn.skills.GetSkill(this.bill.recipe.workSkill).Level,
                                this.bill.recipe.workSkill.label,
                                "NotAssigned".Translate()
                            }), delegate
                            {
                                this.bill.pawnRestriction = pawn;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    if (!pawn.workSettings.WorkIsActive(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate
                            {
                                this.bill.pawnRestriction = pawn;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    if (this.bill.recipe.workSkill != null)
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1} {2})", pawn.LabelShortCap, pawn.skills.GetSkill(this.bill.recipe.workSkill).Level, this.bill.recipe.workSkill.label), delegate
                            {
                                this.bill.pawnRestriction = pawn;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    yield return new Widgets.DropdownMenuElement<Pawn>
                    {
                        option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate
                        {
                            this.bill.pawnRestriction = pawn;
                        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                        payload = pawn
                    };
                }
            }
        }

        private WorkGiverDef GetWorkgiver(IBillGiver billGiver)
        {
            Building_ReligiousBuildingFacility facility = billGiver as Building_ReligiousBuildingFacility;
            if (facility == null)
            {
                Log.ErrorOnce(string.Format("Attempting to get the workgiver for a non-Building_ReligiousBuildingFacility IBillGiver {0}", (object)billGiver.ToString()), 57348754);
                return (WorkGiverDef)null;
            }
            List<WorkGiverDef> defsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
            for (int index = 0; index < defsListForReading.Count; ++index)
            {
                WorkGiverDef workGiverDef = defsListForReading[index];
                WorkGiver_DoReligionActivity worker = workGiverDef.Worker as WorkGiver_DoReligionActivity;
                if (worker != null)
                    return workGiverDef;
            }
            Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", (object)facility.ToString()), 57348750, false);
            return (WorkGiverDef)null;
        }

        [DebuggerHidden]
        private IEnumerable<Widgets.DropdownMenuElement<Zone_Stockpile>> GenerateStockpileInclusion()
        {
            yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
            {
                option = new FloatMenuOption("IncludeFromAll".Translate(), delegate
                {
                    this.bill.includeFromZone = null;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = null
            };
            List<SlotGroup> groupList = this.bill.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
            int groupCount = groupList.Count;
            for (int i = 0; i < groupCount; i++)
            {
                SlotGroup group = groupList[i];
                Zone_Stockpile stockpile = group.parent as Zone_Stockpile;
                if (stockpile != null)
                {
                    if (!this.bill.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.bill, stockpile))
                    {
                        yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", "IncludeSpecific".Translate(group.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = stockpile
                        };
                    }
                    else
                    {
                        yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
                        {
                            option = new FloatMenuOption("IncludeSpecific".Translate(group.parent.SlotYielderLabel()), delegate
                            {
                                this.bill.includeFromZone = stockpile;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = stockpile
                        };
                    }
                }
            }
        }
    }
}