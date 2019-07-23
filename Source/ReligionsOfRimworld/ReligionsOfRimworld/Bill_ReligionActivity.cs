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
        private ReligionActivityProperty property;
        private Pawn materialPawn;

        public Bill_ReligionActivity()
        { }

        public Bill_ReligionActivity(ReligionActivityProperty property) : base(property.Recipe)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                this.property = property;
        }

        public Pawn MaterialPawn { get => materialPawn; set => materialPawn = value; }
        public ReligionActivityProperty Property => property;

        public override bool ShouldDoNow()
        {
            return !suspended;
        }

        public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
        {
            base.Notify_IterationCompleted(billDoer, ingredients);
            this.billStack.Delete(this);
        }

        public override void ValidateSettings()
        {
            base.ValidateSettings();
            if(this.pawnRestriction != null && pawnRestriction.GetReligionComponent().Religion != ((Building_ReligiousBuildingFacility)billStack.billGiver).AssignedReligion)
                this.pawnRestriction = (Pawn)null;
            if (this.materialPawn != null && materialPawn.Dead)
                this.materialPawn = null;
        }

        protected override void DoConfigInterface(Rect baseRect, Color baseColor)
        {
            Rect rect = new Rect(28f, 32f, 100f, 30f);
            GUI.color = new Color(1f, 1f, 1f, 0.65f);
            Widgets.Label(rect, this.RepeatInfoText);
            GUI.color = baseColor;
            WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
            if (widgetRow.ButtonText("Details".Translate() + "...", (string)null, true, false))
                Find.WindowStack.Add((Window)new Dialog_Bill_ReligionActivityConfig(this, ((Thing)this.billStack.billGiver).Position));
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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ReligionActivityProperty>(ref this.property, "property");
            Scribe_References.Look<Pawn>(ref this.materialPawn, "materialPawn");
        }
    }
}
