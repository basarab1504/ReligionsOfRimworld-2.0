using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionsTabUtility
    {
        public static void DoWindowContents(Rect fillRect, ref Vector2 scrollPosition, ref float scrollViewHeight)
        {
            Rect position = new Rect(0.0f, 0.0f, fillRect.width, fillRect.height);
            GUI.BeginGroup(position);
            Verse.Text.Font = GameFont.Small;
            GUI.color = Color.white;
            Rect outRect = new Rect(0.0f, 50f, position.width, position.height - 50f);
            Rect rect = new Rect(0.0f, 0.0f, position.width - 16f, scrollViewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, rect, true);
            float num = 0.0f;
            foreach (Religion religion in ReligionExtensions.GetReligionManager().AllReligions)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.2f);
                Widgets.DrawLineHorizontal(0.0f, num, rect.width);
                GUI.color = Color.white;
                num += DrawReligionRow(religion, num, rect);
            }
            if (Event.current.type == EventType.Layout)
                scrollViewHeight = num;
            Widgets.EndScrollView();
            GUI.EndGroup();
        }

        static float DrawReligionRow(Religion religion, float rowY, Rect fillRect)
        {
            Rect rect1 = new Rect(35f, rowY, 250f, 80f);
            StringBuilder stringBuilder = new StringBuilder();
            string str1 = stringBuilder.ToString();
            float width = fillRect.width - rect1.xMax;
            float num = Verse.Text.CalcHeight(str1, width);
            float height = Mathf.Max(80f, num);
            Rect position = new Rect(10f, rowY + 10f, 15f, 15f);
            Rect rect2 = new Rect(0.0f, rowY, fillRect.width, height);
            if (Mouse.IsOver(rect2))
                GUI.DrawTexture(rect2, (Texture)TexUI.HighlightTex);
            Verse.Text.Font = GameFont.Small;
            Verse.Text.Anchor = TextAnchor.UpperLeft;
            string label1 = religion.Label.CapitalizeFirst();
            Widgets.Label(rect1, label1);
            Rect rect3 = new Rect(rect1.xMax, rowY, 60f, 80f);
            ReligionInfo(rect3.x, rect3.y, religion);
            Rect rect4 = new Rect(rect3.xMax, rowY, 250f, 80f);
            Widgets.Label(new Rect(rect4.xMax, rowY, width, num), str1);
            Verse.Text.Anchor = TextAnchor.UpperLeft;
            return height;
        }

        static bool ReligionInfo(float x, float y, Religion religion)
        {
            if (!InfoCardButtonWorker(x, y))
                return false;
            Find.WindowStack.Add((Window)new Dialog_ReligionInfoMain(religion));
            return true;
        }

        static bool InfoCardButtonWorker(float x, float y)
        {
            Rect rect = new Rect(x, y, 24f, 24f);
            TooltipHandler.TipRegion(rect, (TipSignal)"DefInfoTip".Translate());
            bool flag = Widgets.ButtonImage(rect, GraphicsCache.Info, GUI.color);
            UIHighlighter.HighlightOpportunity(rect, "InfoCard");
            return flag;
        }
    }
}
