using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class ITab_ActivityTasks : ITab
    {
        private static readonly Vector2 WinSize = new Vector2(420f, 480f);
        [TweakValue("Interface", 0.0f, 128f)]
        private static float PasteX = 48f;
        [TweakValue("Interface", 0.0f, 128f)]
        private static float PasteY = 3f;
        [TweakValue("Interface", 0.0f, 32f)]
        private static float PasteSize = 24f;
        private float viewHeight = 1000f;
   
        public ITab_ActivityTasks()
        {
            this.size = ITab_ActivityTasks.WinSize;
            this.labelKey = "Religion_Tasks".Translate();
        }

        protected Building_ReligiousBuildingFacility SelFacility => (Building_ReligiousBuildingFacility)this.SelThing;
        private ActivityTaskManager TaskManager => SelFacility.TaskManager;

        protected override void FillTab()
        {
            if (!SelFacility.IsComplete)
            {
                Widgets.Label(new Rect(0.0f, 0.0f, ITab_ActivityTasks.WinSize.x, ITab_ActivityTasks.WinSize.y).ContractedBy(25f), "Religion_BuildingIsNotAvaliable".Translate());
                return;
            }
            else
            {
                TaskManager.Reorder();
                DrawMonth(new Rect(0.0f, 0.0f, ITab_ActivityTasks.WinSize.x, ITab_ActivityTasks.WinSize.y).ContractedBy(25f));
            }             
        }

        private void DrawMonth(Rect rect)
        {
            float height = rect.height / 3;
            float width = rect.width / 5;
            float curX = 0;
            float curY = 0;

            for (int i = 0; i < 15; ++i)
            {
                if (curX >= rect.width)
                {
                    curY += height;
                    curX = 0;
                }

                DrawDay(new Rect(rect.x + curX, rect.y + curY, width, height).ContractedBy(2), i + 1);
                curX += width;
            }
        }

        private void DrawDay(Rect rect, int dayNumber)
        {
            Widgets.DrawBox(rect);

            Widgets.Label(rect.ContractedBy(6), dayNumber.ToString());
            ScheduledDay day = TaskManager.Schedule.FirstOrDefault(x => x.DayNumber == dayNumber);

            if (day != null)
                DrawDayInterFace(new Rect(rect.x + 6, rect.y + 30f, rect.width, rect.height * .75f), day);

            if (Mouse.IsOver(rect))
            {
                Widgets.DrawBox(rect, 3);
                if (Input.GetMouseButtonDown(0))
                {                 
                    if (day == null)
                        TaskManager.Create(dayNumber);                       
                    Find.WindowStack.Add((Window)new Dialog_ScheduleDayConfig(day, GetFloatMenuOptions(day)));
                }   
            }
        }

        private void DrawDayInterFace(Rect rect, ScheduledDay day)
        {
            float curY = rect.y;
            float height = rect.height / 4;

            Text.Font = GameFont.Tiny;
            for (int i = 0; i < 3; ++i)
            {
                if(i < day.Tasks.Count())
                {
                    ActivityTask task = day.Tasks.ElementAt(i);
                    Widgets.Label(new Rect(rect.x, curY, rect.width, height), task.StartHour.ToString() + ". " + task.Label);                   
                    curY += height;
                }
            }
            if(day.Tasks.Count() > 3)
            {
                Widgets.Label(new Rect(rect.x, curY, rect.width, height), "AndXTasksMore".Translate((NamedArgument)(day.Tasks.Count() - 3).ToString()));
            }
            Text.Font = GameFont.Small;
        }

        private void CreateNoPawnsOfReligionDialog(Religion religion)
        {
            Find.WindowStack.Add((Window)new Dialog_MessageBox("ReligionActivity_NoPawnsOfReligion".Translate((NamedArgument)religion.Label), (string)null, (Action)null, (string)null, (Action)null, (string)null, false, (Action)null, (Action)null));
        }

        private List<FloatMenuOption> GetFloatMenuOptions(ScheduledDay day)
        {
            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (ActivityTaskDef property in SelFacility.AssignedReligion.FindByTag<ReligionSettings_ReligionActivity>(SettingsTagDefOf.ActivityTag).Properties)
            {
                list.Add(new FloatMenuOption(property.label, (Action)(() =>
                {
                    if (!this.SelFacility.Map.mapPawns.FreeColonists.Any<Pawn>(x => x.GetReligionComponent().Religion == SelFacility.AssignedReligion))
                        CreateNoPawnsOfReligionDialog(SelFacility.AssignedReligion);
                    day.Add(new ActivityTask(SelFacility, property));
                    day.Reorder();
                }), MenuOptionPriority.Default, (Action)null, (Thing)null, 29f, (Func<Rect, bool>)(rect => Widgets.InfoCardButton(rect.x + 5f, rect.y + (float)(((double)rect.height - 24.0) / 2.0), property)), (WorldObject)null));
            }
            if (!list.Any<FloatMenuOption>())
                list.Add(new FloatMenuOption("NoneBrackets".Translate(), (Action)null, MenuOptionPriority.Default, (Action)null, (Thing)null, 0.0f, (Func<Rect, bool>)null, (WorldObject)null));
            return list;
        }
    }
}
