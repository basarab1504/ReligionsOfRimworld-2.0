using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;

namespace ReligionsOfRimworld
{
    public class Dialog_ReligionInfo : Window
    {
        private Religion religion;
        private static Vector2 scrollPosition;
        private static Vector2 secondScrollPosition;
        private static float listHeight;
        private ReligionInfoEntry selected;

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(1150f, 760f);
            }
        }

        protected override float Margin
        {
            get
            {
                return 0.0f;
            }
        }

        public Dialog_ReligionInfo(Religion religion)
        {
            this.religion = religion;
            this.forcePause = true;
            //this.doCloseButton = true;
            this.doCloseX = true;
            this.absorbInputAroundWindow = true;
            this.closeOnClickedOutside = true;
            this.soundAppear = SoundDefOf.InfoCard_Open;
            this.soundClose = SoundDefOf.InfoCard_Close;
            scrollPosition = new Vector2();
            secondScrollPosition = new Vector2();
            //cachedEntries.Clear();
            selected = null;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect rect1 = new Rect(inRect).ContractedBy(18f);
            rect1.height = 34f;
            Text.Font = GameFont.Medium;
            Widgets.Label(rect1, religion.Label);
            Text.Font = GameFont.Small;
            float y = 0.0f;
            Rect viewRect = new Rect(inRect.x, inRect.y + 34f, inRect.width - 16f, listHeight).ContractedBy(18f).LeftHalf();
            Widgets.BeginScrollView(new Rect(inRect.x, inRect.y + 34f, inRect.width, inRect.height).ContractedBy(18f).LeftHalf(), ref scrollPosition, viewRect, true);
            DrawCategories(viewRect, ref y);
            Widgets.EndScrollView();
            listHeight = y + 100f;
            if (selected != null)
            {
                string explanation = selected.Explanation;
                Rect secondView = new Rect(inRect.x + 10f, inRect.y + 34f, inRect.width - 16f, Verse.Text.CalcHeight(explanation, inRect.width) + 100f).ContractedBy(18f).RightHalf();
                Widgets.BeginScrollView(new Rect(inRect.x + 10f, inRect.y + 34f, inRect.width, inRect.height).ContractedBy(18f).RightHalf(), ref secondScrollPosition, secondView, true);
                DrawExplanation(secondView, explanation);
                Widgets.EndScrollView();
            }
        }

        private void DrawCategories(Rect rect, ref float y)
        {
            GUI.BeginGroup(rect);
            foreach(ReligionInfoCategory info in religion.GetInfo())
            {
                Widgets.ListSeparator(ref y, rect.width, info.Label);
                DrawEntries(rect, ref y, info.GetInfoEntries());
            }
            GUI.EndGroup();
        }

        private void DrawEntries(Rect rect, ref float curY, IEnumerable<ReligionInfoEntry> entries)
        {
            foreach (ReligionInfoEntry entry in entries)
                DrawEntry(rect, ref curY, entry);
        }

        private void DrawEntry(Rect rect, ref float curY, ReligionInfoEntry entry)
        {
            if (entry == null)
                return;

            Rect rect1 = new Rect(0.0f, curY, rect.width, Verse.Text.CalcHeight(entry.Value, rect.width));
            Widgets.Label(rect1, entry.Label);
            if (entry.SubjectOffset != null)
            {
                GUI.color = entry.SubjectOffset.Color;
                Widgets.Label(new Rect(0.0f + 200f, curY, rect.width, rect1.height), entry.SubjectOffset.ToString());
                GUI.color = Color.white;
            }
            if (entry.WitnessOffset != null)
            {
                GUI.color = entry.WitnessOffset.Color;
                Widgets.Label(new Rect(0.0f + 300f, curY, rect.width, rect1.height), entry.WitnessOffset.ToString());
                GUI.color = Color.white;
            }
            Widgets.Label(new Rect(0.0f + 350f, curY, rect.width, rect1.height), entry.Value);
            curY += 24f;
            if (Mouse.IsOver(rect1))
            {
                Widgets.DrawHighlight(rect1);
                selected = entry;
            }
        }

        private void DrawExplanation(Rect rect, string explanation)
        {
            float y = 0.0f;
            GUI.BeginGroup(rect);
            Widgets.ListSeparator(ref y, rect.width, "ReligionInfo_Description".Translate());
            Rect rect1 = new Rect(0.0f, y, rect.width, rect.height);
            Widgets.Label(rect1, explanation);
            GUI.EndGroup();
        }
    }
}
