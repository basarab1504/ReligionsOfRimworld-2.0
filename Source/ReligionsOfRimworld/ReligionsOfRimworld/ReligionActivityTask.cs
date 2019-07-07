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
    public class ReligionActivityTask : IExposable, ILoadReferenceable
    {
        private int loadID;
        private ReligionActivityTaskList taskStack;
        private SettingsTagDef tag;
        private Dictionary<ThingDef, int> thingDefCount;
        private ThingFilter fixedFilter;
        private ThingFilter dynamicFilter;
        private bool suspended;
        private float thingSearchRadius = 999f;
        private IntVec3 searchCenter;
        private Pawn pawnRestriction;
        private int lastThingSearchFailTicks = -99999;

        public ReligionActivityTask(ReligionActivityTaskList taskStack, ReligionSettings_ReligionActivity religionSettings, IntVec3 giverPosition)
        {
            this.taskStack = taskStack;
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                this.tag = religionSettings.Tag;
                searchCenter = giverPosition;
                this.loadID = Find.UniqueIDsManager.GetNextBillID();
                fixedFilter = new ThingFilter();
                dynamicFilter = new ThingFilter();
                thingDefCount = new Dictionary<ThingDef, int>(); 
                if (religionSettings.ActivityRelics != null)
                    FirstInitilizeFiltersAndCount(religionSettings);
            }
        }

        public ThingFilter FixedFilter => fixedFilter;
        public ThingFilter DynamicFilter => dynamicFilter;
        public bool Suspended => suspended;
        public float ThingSearchRadius { get => thingSearchRadius; set => thingSearchRadius = value; }
        public Pawn PawnRestriction { get => pawnRestriction; set => pawnRestriction = value; }
        public int LastIngredientSearchFailTicks { get => lastThingSearchFailTicks; set => lastThingSearchFailTicks = value; }

        public bool ShouldDoNow => !suspended;
        public string Label => tag.LabelCap;
        public string Description => tag.description;

        public int GetCount(ThingDef thingDef)
        {
            if (thingDefCount.ContainsKey(thingDef))
                return thingDefCount[thingDef];
            return 0;
        }

        public virtual bool PawnAllowedToStartAnew(Pawn p)
        {
            if (this.pawnRestriction != null)
                return this.pawnRestriction == p;
            //if (this.recipe.workSkill != null)
            //{
            //    int level = p.skills.GetSkill(this.recipe.workSkill).Level;
            //    if (level < this.allowedSkillRange.min)
            //    {
            //        JobFailReason.Is("UnderAllowedSkill".Translate((NamedArgument)this.allowedSkillRange.min), this.Label);
            //        return false;
            //    }
            //    if (level > this.allowedSkillRange.max)
            //    {
            //        JobFailReason.Is("AboveAllowedSkill".Translate((NamedArgument)this.allowedSkillRange.max), this.Label);
            //        return false;
            //    }
            //}
            return true;
        }

        public Rect DoInterface(float x, float y, float width, int index)
        {
            Rect rect1 = new Rect(x, y, width, 53f);
            float height = 0.0f;
            //if (!this.StatusString.NullOrEmpty())
            //    height = Mathf.Max(17f, this.StatusLineMinHeight);
            rect1.height += height;
            Color baseColor = Color.white;
            if (!this.ShouldDoNow)
                baseColor = new Color(1f, 0.7f, 0.7f, 0.7f);
            GUI.color = baseColor;
            Text.Font = GameFont.Small;
            if (index % 2 == 0)
                Widgets.DrawAltRect(rect1);
            GUI.BeginGroup(rect1);
            Rect rect2 = new Rect(0.0f, 0.0f, 24f, 24f);
            //if (this.taskStack.IndexOf(this) > 0)
            //{
            //    if (Widgets.ButtonImage(rect2, TexButton.ReorderUp, baseColor))
            //    {
            //        this.taskStack.Reorder(this, -1);
            //        SoundDefOf.Tick_High.PlayOneShotOnCamera((Map)null);
            //    }
            //    TooltipHandler.TipRegion(rect2, (TipSignal)"ReorderTaskUpTip".Translate());
            //}
            //if (this.taskStack.IndexOf(this) < this.taskStack.Count - 1)
            //{
            //    Rect rect3 = new Rect(0.0f, 24f, 24f, 24f);
            //    if (Widgets.ButtonImage(rect3, TexButton.ReorderDown, baseColor))
            //    {
            //        this.taskStack.Reorder(this, 1);
            //        SoundDefOf.Tick_Low.PlayOneShotOnCamera((Map)null);
            //    }
            //    TooltipHandler.TipRegion(rect3, (TipSignal)"ReorderTaskDownTip".Translate());
            //}
            Widgets.Label(new Rect(28f, 0.0f, (float)((double)rect1.width - 48.0 - 20.0), rect1.height + 5f), Label);
            this.DoConfigInterface(rect1.AtZero(), baseColor);
            Rect rect4 = new Rect(rect1.width - 24f, 0.0f, 24f, 24f);
            if (Widgets.ButtonImage(rect4, GraphicsCache.DeleteX, baseColor, baseColor * GenUI.SubtleMouseoverColor))
            {
                this.taskStack.Remove(this);
                SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
            }
            TooltipHandler.TipRegion(rect4, (TipSignal)"RemoveTaskTip".Translate());
            Rect rect5;
            //if (this.CanCopy)
            //{
            //    Rect rect3 = new Rect(rect4);
            //    rect3.x -= rect3.width + 4f;
            //    if (Widgets.ButtonImageFitted(rect3, TexButton.Copy, baseColor))
            //    {
            //        TaskUtility.Clipboard = this.Clone();
            //        SoundDefOf.Tick_High.PlayOneShotOnCamera((Map)null);
            //    }
            //    TooltipHandler.TipRegion(rect3, (TipSignal)"CopyTaskTip".Translate());
            //    rect5 = new Rect(rect3);
            //}
            //else
                rect5 = new Rect(rect4);
            rect5.x -= rect5.width + 4f;
            if (Widgets.ButtonImage(rect5, GraphicsCache.Suspend, baseColor))
            {
                this.suspended = !this.suspended;
                SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
            }
            TooltipHandler.TipRegion(rect5, (TipSignal)"SuspendTaskTip".Translate());
            //if (!this.StatusString.NullOrEmpty())
            //{
            //    Text.Font = GameFont.Tiny;
            //    Rect rect3 = new Rect(24f, rect1.height - height, rect1.width - 24f, height);
            //    Widgets.Label(rect3, this.StatusString);
            //    this.DoStatusLineInterface(rect3);
            //}
            GUI.EndGroup();
            if (this.suspended)
            {
                Text.Font = GameFont.Medium;
                Text.Anchor = TextAnchor.MiddleCenter;
                Rect rect3 = new Rect((float)((double)rect1.x + (double)rect1.width / 2.0 - 70.0), (float)((double)rect1.y + (double)rect1.height / 2.0 - 20.0), 140f, 40f);
                GUI.DrawTexture(rect3, (Texture)TexUI.GrayTextBG);
                Widgets.Label(rect3, "SuspendedCaps".Translate());
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
            }
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            return rect1;
        }

        protected virtual void DoConfigInterface(Rect baseRect, Color baseColor)
        {
            Rect rect = new Rect(28f, 32f, 100f, 30f);
            GUI.color = new Color(1f, 1f, 1f, 0.65f);
            //Widgets.Label(rect, this.RepeatInfoText);
            GUI.color = baseColor;
            WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
            if (widgetRow.ButtonText("Details".Translate() + "...", (string)null, true, false))
                Find.WindowStack.Add((Window)new Dialog_ReligionActivityTaskConfig(this, searchCenter));
            //if (widgetRow.ButtonText(this.repeatMode.LabelCap.PadRight(20), (string)null, true, false))
            //    BillRepeatModeUtility.MakeConfigFloatMenu(this);
            //if (widgetRow.ButtonIcon(TexButton.Plus, (string)null, new Color?()))
            //{
            //    if (this.repeatMode == BillRepeatModeDefOf.Forever)
            //    {
            //        this.repeatMode = BillRepeatModeDefOf.RepeatCount;
            //        this.repeatCount = 1;
            //    }
            //    else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
            //    {
            //        int num = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
            //        this.targetCount += num;
            //        this.unpauseWhenYouHave += num;
            //    }
            //    else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
            //        this.repeatCount += GenUI.CurrentAdjustmentMultiplier();
            //    SoundDefOf.AmountIncrement.PlayOneShotOnCamera((Map)null);
            //    if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
            //        TutorSystem.Notify_Event((EventPack)(this.recipe.defName + "-RepeatCountSetTo-" + (object)this.repeatCount));
            //}
            //if (!widgetRow.ButtonIcon(TexButton.Minus, (string)null, new Color?()))
            //    return;
            //if (this.repeatMode == BillRepeatModeDefOf.Forever)
            //{
            //    this.repeatMode = BillRepeatModeDefOf.RepeatCount;
            //    this.repeatCount = 1;
            //}
            //else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
            //{
            //    int num = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
            //    this.targetCount = Mathf.Max(0, this.targetCount - num);
            //    this.unpauseWhenYouHave = Mathf.Max(0, this.unpauseWhenYouHave - num);
            //}
            //else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
            //    this.repeatCount = Mathf.Max(0, this.repeatCount - GenUI.CurrentAdjustmentMultiplier());
            //SoundDefOf.AmountDecrement.PlayOneShotOnCamera((Map)null);
            //if (!TutorSystem.TutorialMode || this.repeatMode != BillRepeatModeDefOf.RepeatCount)
            //    return;
            //TutorSystem.Notify_Event((EventPack)(this.recipe.defName + "-RepeatCountSetTo-" + (object)this.repeatCount));
        }

        private void FirstInitilizeFiltersAndCount(ReligionSettings_ReligionActivity religionSettings)
        {
            foreach (ReligionActivityProperty property in religionSettings.ActivityRelics)
            {
                fixedFilter.SetAllow(property.Relic, true);
                thingDefCount.Add(property.Relic, property.Count);
            }
            dynamicFilter.SetAllowAll(fixedFilter);
        }

        public string GetUniqueLoadID()
        {
            return "ReligionActivityTask_" + (object)this.loadID;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.loadID, "loadID");
            Scribe_Defs.Look<SettingsTagDef>(ref this.tag, "tag");
            Scribe_Deep.Look<ThingFilter>(ref this.fixedFilter, "fixedFilter");
            Scribe_Deep.Look<ThingFilter>(ref this.dynamicFilter, "dynamicFilter");
            Scribe_Values.Look<bool>(ref this.suspended, "suspended");
            Scribe_Values.Look<float>(ref this.thingSearchRadius, "thingSearchRadius");
            Scribe_Values.Look<IntVec3>(ref this.searchCenter, "searchCenter");
            Scribe_Collections.Look<ThingDef, int>(ref this.thingDefCount, "thingDefCount");
            Scribe_Values.Look<int>(ref this.lastThingSearchFailTicks, "lastThingSearchFailTicks");
        }
    }
}
