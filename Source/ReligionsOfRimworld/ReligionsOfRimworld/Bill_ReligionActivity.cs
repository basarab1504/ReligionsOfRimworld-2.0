using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class Bill_ReligionActivity : Bill_Production
    {
        private ThingFilter fixedingredientFilter;
        private Religion assignedReligion;

        public Bill_ReligionActivity(RecipeDef recipe, Religion religion, ReligionSettings_ReligionActivity religionSettings) : base(recipe)
        {
            if (Scribe.mode == LoadSaveMode.Inactive && religionSettings.ActivityRelics != null)
            {
                assignedReligion = religion;
                fixedingredientFilter = new ThingFilter();
                foreach (ThingDef def in AllThingDefsFromSettings(religionSettings))
                    fixedingredientFilter.SetAllow(def, true);
            }

        }

        public ThingFilter FixedingredientFilter => fixedingredientFilter;
        public Religion AssignedReligion => assignedReligion;

        public override bool ShouldDoNow()
        {
            return !suspended;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Religion>(ref this.assignedReligion, "assignedReligion");
            Scribe_Deep.Look<ThingFilter>(ref this.fixedingredientFilter, "fixedingredientFilter");
        }

        protected override void DoConfigInterface(Rect baseRect, Color baseColor)
        {
            Rect rect = new Rect(28f, 32f, 100f, 30f);
            GUI.color = new Color(1f, 1f, 1f, 0.65f);
            Widgets.Label(rect, this.RepeatInfoText);
            GUI.color = baseColor;
            WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
            //if (widgetRow.ButtonText("Details".Translate() + "...", (string)null, true, false))
                //Find.WindowStack.Add((Window)new Dialog_ReligionActivityTaskConfig(this, ((Thing)this.billStack.billGiver).Position));
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

        private IEnumerable<ThingDef> AllThingDefsFromSettings(ReligionSettings_ReligionActivity religionSettings)
        {
            foreach (ReligionProperty property in religionSettings.ActivityRelics)
                if (property.GetObject() != null)
                    yield return (ThingDef)property.GetObject();
        }
    }
}
