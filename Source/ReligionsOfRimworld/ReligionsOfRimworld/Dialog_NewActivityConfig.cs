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
    public class Dialog_ScheduleDayConfig : Window
    {
        private ScheduledDay day;
        private List<FloatMenuOption> menuOptions;
        private Vector2 scrollPosition = new Vector2();

        public Dialog_ScheduleDayConfig(ScheduledDay day, List<FloatMenuOption> menuOptions)
        {
            this.day = day;
            this.menuOptions = menuOptions;
            this.forcePause = true;
            this.doCloseX = true;
            this.doCloseButton = true;
            //this.absorbInputAroundWindow = true;
            this.closeOnClickedOutside = true;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(800f, 634f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            Rect rect = new Rect(0f, 0f, 400f, 50f);
            Widgets.Label(rect, "ReligionInfo_Day".Translate() + " " + day.DayNumber);
            Text.Font = GameFont.Small;
            Rect rect2 = new Rect(0f, 50f, inRect.width, inRect.height - 50f - this.CloseButSize.y);
            DoListing(rect2);
        }

        public void DoListing(Rect rect)
        {
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            WidgetRow widgetRow = new WidgetRow(0.0f, 0.0f, UIDirection.RightThenDown, 99999f, 4f);
            if (widgetRow.ButtonText("ReligionInfo_AddTask".Translate()))
                Find.WindowStack.Add((Window)new FloatMenu(menuOptions));
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            Rect outRect = new Rect(0.0f, 35f, rect.width, rect.height - 35f);
            Rect viewRect = new Rect(0.0f, 0.0f, outRect.width - 16f, 1300f);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
            float y = 0.0f;
            for (int index = 0; index < day.Tasks.Count(); ++index)
            {
                ActivityTask task = day.Tasks.ElementAt(index);
                Rect rect1 = task.DoInterface(0.0f, y, viewRect.width, index);
                Rect deleteRect = new Rect(rect1.width - 24f, y, 24f, 24f);
                if (Widgets.ButtonImage(deleteRect, GraphicsCache.DeleteX, Color.white, Color.white * GenUI.SubtleMouseoverColor))
                {
                    day.Remove(task);
                    SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
                }
                //TooltipHandler.TipRegion(deleteRect, (TipSignal)"DeleteBillTip".Translate());
                y += rect1.height + 6f;
            }
            if (Event.current.type == EventType.Layout)
                viewRect.height = y + 60f;
            Widgets.EndScrollView();
            GUI.EndGroup();
        }
    }
}