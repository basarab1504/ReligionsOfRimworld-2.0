//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using Verse;

//namespace ReligionsOfRimworld
//{
//    public class Dialog_ActivitySchedule : Window
//    {
//        private ActivityTaskManager taskManager;

//        public Dialog_ActivitySchedule(ActivityTaskManager taskManager)
//        {
//            this.taskManager = taskManager;
//            this.forcePause = true;
//            this.doCloseX = true;
//            this.doCloseButton = true;
//            this.absorbInputAroundWindow = true;
//            this.closeOnClickedOutside = true;
//        }

//        public override Vector2 InitialSize
//        {
//            get
//            {
//                return new Vector2(800f, 634f);
//            }
//        }

//        public override void DoWindowContents(Rect inRect)
//        {
//            Text.Font = GameFont.Medium;
//            Rect rect = new Rect(0f, 0f, 400f, 50f);
//            Widgets.Label(rect, "Schedule".Translate());
//            Text.Font = GameFont.Small;
//            Rect rect2 = new Rect(0f, 50f, inRect.width, inRect.height - 50f - this.CloseButSize.y);
//            DrawMonth(rect2);
//        }

//        private void DrawMonth(Rect rect)
//        {
//            float height = rect.height / 3;
//            float width = rect.width / 5;
//            float curX = 0;
//            float curY = 0;

//            for(int i = 0; i < taskManager.Schedule.Count(); ++i)
//            {
//                if (curX >= rect.width)
//                {
//                    curY += height;
//                    curX = 0;
//                }

//                DrawDay(new Rect(rect.x + curX, rect.y + curY, width, height).ContractedBy(2), taskManager.Schedule.ElementAt(i));
//                curX += width;
//            }
//        }

//        private void DrawDay(Rect rect, ScheduledDay day)
//        {
//            Widgets.DrawBox(rect);
//            Widgets.Label(rect.ContractedBy(4), day.DayNumber.ToString());

//            if (Mouse.IsOver(rect))
//            {
//                Widgets.DrawBox(rect, 3);
//                if (Input.GetMouseButtonDown(0))
//                    Find.WindowStack.Add((Window)new Dialog_ScheduleDayConfig(day));
//            }
//        }
//    }
//}
