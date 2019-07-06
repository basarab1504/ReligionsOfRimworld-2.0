using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ReligionsOfRimworld
{
    public class Dialog_ReligionActivityTaskConfig : Window
    {
        private IntVec3 billGiverPos;

        private ReligionActivityTask task;

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

        public Dialog_ReligionActivityTaskConfig(ReligionActivityTask task, IntVec3 billGiverPos)
        {
            this.billGiverPos = billGiverPos;
            this.task = task;
            this.forcePause = true;
            this.doCloseX = true;
            this.doCloseButton = true;
            this.absorbInputAroundWindow = true;
            this.closeOnClickedOutside = true;
        }

        //private void AdjustCount(int offset)
        //{
        //    if (offset > 0)
        //    {
        //        SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
        //    }
        //    else
        //    {
        //        SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
        //    }
        //    this.task.repeatCount += offset;
        //    if (this.task.repeatCount < 1)
        //    {
        //        this.task.repeatCount = 1;
        //    }
        //}

        public override void WindowUpdate()
        {
            TryDrawIngredientSearchRadiusOnMap(task, this.billGiverPos);
        }

    public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            Rect rect = new Rect(0f, 0f, 400f, 50f);
            Widgets.Label(rect, this.task.Label);
            float width = (float)((int)((inRect.width - 34f) / 3f));
            Rect rect2 = new Rect(0f, 80f, width, inRect.height - 80f);
            Rect rect3 = new Rect(rect2.xMax + 17f, 50f, width, inRect.height - 50f - this.CloseButSize.y);
            Rect rect4 = new Rect(rect3.xMax + 17f, 50f, 0f, inRect.height - 50f - this.CloseButSize.y);
            rect4.xMax = inRect.xMax;
            Text.Font = GameFont.Small;
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect3);
            //Listing_Standard listing_Standard2 = listing_Standard.BeginSection((float)Dialog_ReligionActivityBillConfig.RepeatModeSubdialogHeight);
            //if (listing_Standard2.ButtonText(this.task.repeatMode.LabelCap, null))
            //{
            //    BillRepeatModeUtility.MakeConfigFloatMenu(this.task);
            //}
            //listing_Standard2.Gap(12f);
            //if (this.task.repeatMode == BillRepeatModeDefOf.RepeatCount)
            //{
            //    listing_Standard2.Label("RepeatCount".Translate(this.task.repeatCount), -1f, null);
            //    listing_Standard2.IntEntry(ref this.task.repeatCount, ref this.repeatCountEditBuffer, 1);
            //}
            //else if (this.task.repeatMode == BillRepeatModeDefOf.TargetCount)
            //{
            //    string text = "CurrentlyHave".Translate() + ": ";
            //    text += this.task.recipe.WorkerCounter.CountProducts(this.task);
            //    text += " / ";
            //    text += ((this.task.targetCount >= 999999) ? "Infinite".Translate().ToLower() : this.task.targetCount.ToString());
            //    string str = this.task.recipe.WorkerCounter.ProductsDescription(this.task);
            //    if (!str.NullOrEmpty())
            //    {
            //        string text2 = text;
            //        text = string.Concat(new string[]
            //        {
            //            text2,
            //            "\n",
            //            "CountingProducts".Translate(),
            //            ": ",
            //            str.CapitalizeFirst()
            //        });
            //    }
            //    listing_Standard2.Label(text, -1f, null);
            //    int targetCount = this.task.targetCount;
            //    listing_Standard2.IntEntry(ref this.task.targetCount, ref this.targetCountEditBuffer, this.task.recipe.targetCountAdjustment);
            //    this.task.unpauseWhenYouHave = Mathf.Max(0, this.task.unpauseWhenYouHave + (this.task.targetCount - targetCount));
            //    ThingDef producedThingDef = this.task.recipe.ProducedThingDef;
            //    if (producedThingDef != null)
            //    {
            //        if (producedThingDef.IsWeapon || producedThingDef.IsApparel)
            //        {
            //            listing_Standard2.CheckboxLabeled("IncludeEquipped".Translate(), ref this.task.includeEquipped, null);
            //        }
            //        if (producedThingDef.IsApparel && producedThingDef.apparel.careIfWornByCorpse)
            //        {
            //            listing_Standard2.CheckboxLabeled("IncludeTainted".Translate(), ref this.task.includeTainted, null);
            //        }
            //        Widgets.Dropdown<ReligionActivityTask, Zone_Stockpile>(listing_Standard2.GetRect(30f), this.task, (ReligionActivityTask b) => b.includeFromZone, (ReligionActivityTask b) => this.GenerateStockpileInclusion(), (this.task.includeFromZone != null) ? "IncludeSpecific".Translate(this.task.includeFromZone.label) : "IncludeFromAll".Translate(), null, null, null, null, false);
            //        Widgets.FloatRange(listing_Standard2.GetRect(28f), 975643279, ref this.task.hpRange, 0f, 1f, "HitPoints", ToStringStyle.PercentZero);
            //        if (producedThingDef.HasComp(typeof(CompQuality)))
            //        {
            //            Widgets.QualityRange(listing_Standard2.GetRect(28f), 1098906561, ref this.task.qualityRange);
            //        }
            //        if (producedThingDef.MadeFromStuff)
            //        {
            //            listing_Standard2.CheckboxLabeled("LimitToAllowedStuff".Translate(), ref this.task.limitToAllowedStuff, null);
            //        }
            //    }
            //}
            //if (this.task.repeatMode == BillRepeatModeDefOf.TargetCount)
            //{
            //    listing_Standard2.CheckboxLabeled("PauseWhenSatisfied".Translate(), ref this.task.pauseWhenSatisfied, null);
            //    if (this.task.pauseWhenSatisfied)
            //    {
            //        listing_Standard2.Label("UnpauseWhenYouHave".Translate() + ": " + this.task.unpauseWhenYouHave.ToString("F0"), -1f, null);
            //        listing_Standard2.IntEntry(ref this.task.unpauseWhenYouHave, ref this.unpauseCountEditBuffer, this.task.recipe.targetCountAdjustment);
            //        if (this.task.unpauseWhenYouHave >= this.task.targetCount)
            //        {
            //            this.task.unpauseWhenYouHave = this.task.targetCount - 1;
            //            this.unpauseCountEditBuffer = this.task.unpauseWhenYouHave.ToStringCached();
            //        }
            //    }
            //}
            //listing_Standard.EndSection(listing_Standard2);
            //listing_Standard.Gap(12f);

            //Listing_Standard listing_Standard3 = listing_Standard.BeginSection((float)StoreModeSubdialogHeight);
            //string text3 = string.Format(this.task.GetStoreMode().LabelCap, (this.task.GetStoreZone() == null) ? string.Empty : this.task.GetStoreZone().SlotYielderLabel());
            //if (this.task.GetStoreZone() != null && !this.task.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.task, this.task.GetStoreZone()))
            //{
            //    text3 += string.Format(" ({0})", "IncompatibleLower".Translate());
            //    Text.Font = GameFont.Tiny;
            //}
            //if (listing_Standard3.ButtonText(text3, null))
            //{
            //    Text.Font = GameFont.Small;
            //    List<FloatMenuOption> list = new List<FloatMenuOption>();
            //    foreach (BillStoreModeDef current in from bsm in DefDatabase<BillStoreModeDef>.AllDefs
            //                                         orderby bsm.listOrder
            //                                         select bsm)
            //    {
            //        if (current == BillStoreModeDefOf.SpecificStockpile)
            //        {
            //            List<SlotGroup> allGroupsListInPriorityOrder = this.task.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
            //            int count = allGroupsListInPriorityOrder.Count;
            //            for (int i = 0; i < count; i++)
            //            {
            //                SlotGroup group = allGroupsListInPriorityOrder[i];
            //                Zone_Stockpile zone_Stockpile = group.parent as Zone_Stockpile;
            //                if (zone_Stockpile != null)
            //                {
            //                    if (!this.task.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.task, zone_Stockpile))
            //                    {
            //                        list.Add(new FloatMenuOption(string.Format("{0} ({1})", string.Format(current.LabelCap, group.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null));
            //                    }
            //                    else
            //                    {
            //                        list.Add(new FloatMenuOption(string.Format(current.LabelCap, group.parent.SlotYielderLabel()), delegate
            //                        {
            //                            this.task.SetStoreMode(BillStoreModeDefOf.SpecificStockpile, (Zone_Stockpile)group.parent);
            //                        }, MenuOptionPriority.Default, null, null, 0f, null, null));
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            BillStoreModeDef smLocal = current;
            //            list.Add(new FloatMenuOption(smLocal.LabelCap, delegate
            //            {
            //                this.task.SetStoreMode(smLocal, null);
            //            }, MenuOptionPriority.Default, null, null, 0f, null, null));
            //        }
            //    }
            //    Find.WindowStack.Add(new FloatMenu(list));
            //}
            //Text.Font = GameFont.Small;
            //listing_Standard.EndSection(listing_Standard3);
            //listing_Standard.Gap(12f);

            Listing_Standard listing_Standard4 = listing_Standard.BeginSection((float)WorkerSelectionSubdialogHeight);
            Widgets.Dropdown<ReligionActivityTask, Pawn>(listing_Standard4.GetRect(30f), this.task, (ReligionActivityTask b) => b.PawnRestriction, (ReligionActivityTask b) => this.GeneratePawnRestrictionOptions(), (this.task.PawnRestriction != null) ? this.task.PawnRestriction.LabelShortCap : "AnyWorker".Translate(), null, null, null, null, false);
            //if (this.task.PawnRestriction == null/* && this.task.recipe.workSkill != null*/)
            //{
            //    listing_Standard4.Label("AllowedSkillRange".Translate(this.task.recipe.workSkill.label), -1f, null);
            //    listing_Standard4.IntRange(ref this.task.allowedSkillRange, 0, 20);
            //}
            listing_Standard.EndSection(listing_Standard4);
            listing_Standard.End();

            Rect rect5 = rect4;
            rect5.yMin = rect5.yMax - (float)IngredientRadiusSubdialogHeight;
            rect4.yMax = rect5.yMin - 17f;
            //bool flag = this.task.GetStoreZone() == null || this.task.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.task, this.task.GetStoreZone());
            Rect rect6 = rect4;
            int openMask = 4;
            IEnumerable<ThingDef> forceHiddenDefs = null;
            ThingFilterUI.DoThingFilterConfigWindow(rect6, ref this.thingFilterScrollPosition, this.task.DynamicFilter, this.task.FixedFilter, openMask, forceHiddenDefs, null, false, null, null);
            //bool flag2 = this.task.GetStoreZone() == null || this.task.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.task, this.task.GetStoreZone());
            //if (flag && !flag2)
            //{
            //    Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(this.task.LabelCap, this.task.billStack.billGiver.LabelShort.CapitalizeFirst(), this.task.GetStoreZone().label), this.task.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, false);
            //}
            Listing_Standard listing_Standard5 = new Listing_Standard();
            listing_Standard5.Begin(rect5);
            string str2 = "IngredientSearchRadius".Translate().Truncate(rect5.width * 0.6f, null);
            string str3 = (this.task.ThingSearchRadius != 999f) ? this.task.ThingSearchRadius.ToString("F0") : "Unlimited".Translate().Truncate(rect5.width * 0.3f, null);
            listing_Standard5.Label(str2 + ": " + str3, -1f, null);
            this.task.ThingSearchRadius = listing_Standard5.Slider(this.task.ThingSearchRadius, 3f, 100f);
            if (this.task.ThingSearchRadius >= 100f)
            {
                this.task.ThingSearchRadius = 999f;
            }
            listing_Standard5.End();
            //Listing_Standard listing_Standard6 = new Listing_Standard();
            //listing_Standard6.Begin(rect2);
            //if (this.task.suspended)
            //{
            //    if (listing_Standard6.ButtonText("Suspended".Translate(), null))
            //    {
            //        this.task.suspended = false;
            //        SoundDefOf.Click.PlayOneShotOnCamera(null);
            //    }
            //}
            //else if (listing_Standard6.ButtonText("NotSuspended".Translate(), null))
            //{
            //    this.task.Suspended = true;
            //    SoundDefOf.Click.PlayOneShotOnCamera(null);
            //}
            //StringBuilder stringBuilder = new StringBuilder();
            //if (this.task.Description != null)
            //{
            //    stringBuilder.AppendLine(this.task.Description);
            //    stringBuilder.AppendLine();
            //}
            //stringBuilder.AppendLine("WorkAmount".Translate() + ": " + this.task.recipe.WorkAmountTotal(null).ToStringWorkAmount());
            //for (int j = 0; j < this.task.recipe.ingredients.Count; j++)
            //{
            //    IngredientCount ingredientCount = this.task.recipe.ingredients[j];
            //    if (!ingredientCount.filter.Summary.NullOrEmpty())
            //    {
            //        stringBuilder.AppendLine(this.task.recipe.IngredientValueGetter.BillRequirementsDescription(this.task.recipe, ingredientCount));
            //    }
            //}
            //stringBuilder.AppendLine();
            //string text4 = this.task.recipe.IngredientValueGetter.ExtraDescriptionLine(this.task.recipe);
            //if (text4 != null)
            //{
            //    stringBuilder.AppendLine(text4);
            //    stringBuilder.AppendLine();
            //}
            //if (!this.task.recipe.skillRequirements.NullOrEmpty<SkillRequirement>())
            //{
            //    stringBuilder.AppendLine("MinimumSkills".Translate());
            //    stringBuilder.AppendLine(this.task.recipe.MinSkillString);
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
            //if (this.task.recipe.products.Count == 1)
            //{
            //    ThingDef thingDef = this.task.recipe.products[0].thingDef;
            //    Widgets.InfoCardButton(rect2.x, rect4.y, thingDef, GenStuff.DefaultStuffFor(thingDef));
            //}
        }

        private void TryDrawIngredientSearchRadiusOnMap(ReligionActivityTask task, IntVec3 center)
        {
            if ((double)task.ThingSearchRadius >= (double)GenRadial.MaxRadialPatternRadius)
                return;
            GenDraw.DrawRadiusRing(center, task.ThingSearchRadius);
        }

        private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GeneratePawnRestrictionOptions()
        {
            yield return new Widgets.DropdownMenuElement<Pawn>
            {
                option = new FloatMenuOption("AnyWorker".Translate(), delegate
                {
                    this.task.PawnRestriction = null;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = null
            };
            //SkillDef workSkill = this.task.recipe.workSkill;
            IEnumerable<Pawn> pawns = PawnsFinder.AllMaps_FreeColonists/*.Where(x => x.GetReligionComponent().Religion == task.AssignedReligion)*/;
            pawns = from pawn in pawns
                    orderby pawn.LabelShortCap
                    select pawn;

            //if (workSkill != null)
            //{
            //    pawns = from pawn in pawns
            //            orderby pawn.skills.GetSkill(this.task.recipe.workSkill).Level descending
            //            select pawn;
            //}
            WorkGiverDef workGiver = MiscDefOf.DoBillsReligionActivity;
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
                    if (pawn.story.WorkTypeIsDisabled(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(workGiver.verb)), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                    }
                    //if (this.task.recipe.workSkill != null && !pawn.workSettings.WorkIsActive(workGiver.workType))
                    //{
                    //    yield return new Widgets.DropdownMenuElement<Pawn>
                    //    {
                    //        option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", new object[]
                    //        {
                    //            pawn.LabelShortCap,
                    //            pawn.skills.GetSkill(this.task.recipe.workSkill).Level,
                    //            this.task.recipe.workSkill.label,
                    //            "NotAssigned".Translate()
                    //        }), delegate
                    //        {
                    //            this.task.PawnRestriction = pawn;
                    //        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    //        payload = pawn
                    //    };
                    //}
                    if (!pawn.workSettings.WorkIsActive(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate
                            {
                                this.task.PawnRestriction = pawn;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                    }
                    //if (this.task.recipe.workSkill != null)
                    //{
                    //    yield return new Widgets.DropdownMenuElement<Pawn>
                    //    {
                    //        option = new FloatMenuOption(string.Format("{0} ({1} {2})", pawn.LabelShortCap, pawn.skills.GetSkill(this.task.recipe.workSkill).Level, this.task.recipe.workSkill.label), delegate
                    //        {
                    //            this.task.PawnRestriction = pawn;
                    //        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    //        payload = pawn
                    //    };
                    //}
                    yield return new Widgets.DropdownMenuElement<Pawn>
                    {
                        option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate
                        {
                            this.task.PawnRestriction = pawn;
                        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                        payload = pawn
                    };
                }
            }
        }

        //private IEnumerable<Widgets.DropdownMenuElement<Zone_Stockpile>> GenerateStockpileInclusion()
        //{
        //    yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
        //    {
        //        option = new FloatMenuOption("IncludeFromAll".Translate(), delegate
        //        {
        //            this.task.includeFromZone = null;
        //        }, MenuOptionPriority.Default, null, null, 0f, null, null),
        //        payload = null
        //    };
        //    List<SlotGroup> groupList = this.task.billStack.billGiver.Map.haulDestinationManager.AllGroupsListInPriorityOrder;
        //    int groupCount = groupList.Count;
        //    for (int i = 0; i < groupCount; i++)
        //    {
        //        SlotGroup group = groupList[i];
        //        Zone_Stockpile stockpile = group.parent as Zone_Stockpile;
        //        if (stockpile != null)
        //        {
        //            if (!this.task.recipe.WorkerCounter.CanPossiblyStoreInStockpile(this.task, stockpile))
        //            {
        //                yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
        //                {
        //                    option = new FloatMenuOption(string.Format("{0} ({1})", "IncludeSpecific".Translate(group.parent.SlotYielderLabel()), "IncompatibleLower".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null),
        //                    payload = stockpile
        //                };
        //            }
        //            else
        //            {
        //                yield return new Widgets.DropdownMenuElement<Zone_Stockpile>
        //                {
        //                    option = new FloatMenuOption("IncludeSpecific".Translate(group.parent.SlotYielderLabel()), delegate
        //                    {
        //                        this.task.includeFromZone = stockpile;
        //                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
        //                    payload = stockpile
        //                };
        //            }
        //        }
        //    }
        //}
    }
}
