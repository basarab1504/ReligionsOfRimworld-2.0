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
            this.labelKey = "TabTasks";
        }

        protected Building_ReligiousBuildingFacility SelFacility => (Building_ReligiousBuildingFacility)this.SelThing;
        private ActivityTaskSchedule TaskManager => SelFacility.TaskSchedule;

        protected override void FillTab()
        {
            if (!SelFacility.IsComplete)
            {
                Widgets.Label(new Rect(0.0f, 0.0f, ITab_ActivityTasks.WinSize.x, ITab_ActivityTasks.WinSize.y).ContractedBy(25f), "ReligionInfo_BuildingIsNotAvaliable".Translate());
                return;
            }
            else
            {
                Widgets.Label(new Rect(0.0f, 0.0f, ITab_ActivityTasks.WinSize.x, 75f).ContractedBy(25f), "ReligionInfo_DefaultPreacher".Translate());
                DrawPawnSelector(new Rect(0.0f, 25f, ITab_ActivityTasks.WinSize.x, 80).ContractedBy(25f));
                DrawMonth(new Rect(0.0f, 75f, ITab_ActivityTasks.WinSize.x, ITab_ActivityTasks.WinSize.y - 55f).ContractedBy(25f));
            }             
        }

        private void DrawPawnSelector(Rect rect)
        {
            Widgets.Dropdown<ActivityTaskSchedule, Pawn>(rect, this.TaskManager, (ActivityTaskSchedule b) => b.DefaultPawn, (ActivityTaskSchedule b) => this.GeneratePawnRestrictionOptions(), (this.TaskManager.DefaultPawn != null) ? this.TaskManager.DefaultPawn.LabelShortCap : "AnyWorker".Translate().ToString(), null, null, null, null, false);
        }

        private IEnumerable<Widgets.DropdownMenuElement<Pawn>> GeneratePawnRestrictionOptions()
        {
            yield return new Widgets.DropdownMenuElement<Pawn>
            {
                option = new FloatMenuOption("AnyWorker".Translate(), delegate
                {
                    this.TaskManager.DefaultPawn = null;
                }, MenuOptionPriority.Default, null, null, 0f, null, null),
                payload = null
            };
            //SkillDef workSkill = this.bill.recipe.workSkill;
            IEnumerable<Pawn> pawns = PawnsFinder.AllMaps_FreeColonists;
            //pawns = from pawn in pawns
            //        orderby pawn.LabelShortCap
            //        select pawn;
            //if (workSkill != null)
            //{
            //    pawns = from pawn in pawns
            //            orderby pawn.skills.GetSkill(this.bill.recipe.workSkill).Level descending
            //            select pawn;
            //}
            WorkGiverDef workGiver = GetWorkgiver();
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
                        orderby pawn.WorkTypeIsDisabled(workGiver.workType)
                        select pawn;
                foreach (Pawn pawn in pawns)
                {
                    if (pawn.GetReligionComponent().Religion != SelFacility.AssignedReligion)
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "ReligionInfo_WrongReligion".Translate(pawn.GetReligionComponent().Religion.Label)), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    if (pawn.WorkTypeIsDisabled(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "WillNever".Translate(workGiver.verb)), null, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    //if (this.bill.recipe.workSkill != null && !pawn.workSettings.WorkIsActive(workGiver.workType))
                    //{
                    //    yield return new Widgets.DropdownMenuElement<Pawn>
                    //    {
                    //        option = new FloatMenuOption(string.Format("{0} ({1} {2}, {3})", new object[]
                    //        {
                    //            pawn.LabelShortCap,
                    //            pawn.skills.GetSkill(this.bill.recipe.workSkill).Level,
                    //            this.bill.recipe.workSkill.label,
                    //            "NotAssigned".Translate()
                    //        }), delegate
                    //        {
                    //            this.TaskManager.DefaultPawn = pawn;
                    //        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    //        payload = pawn
                    //    };
                    //    continue;
                    //}
                    if (!pawn.workSettings.WorkIsActive(workGiver.workType))
                    {
                        yield return new Widgets.DropdownMenuElement<Pawn>
                        {
                            option = new FloatMenuOption(string.Format("{0} ({1})", pawn.LabelShortCap, "NotAssigned".Translate()), delegate
                            {
                                this.TaskManager.DefaultPawn = pawn;
                            }, MenuOptionPriority.Default, null, null, 0f, null, null),
                            payload = pawn
                        };
                        continue;
                    }
                    yield return new Widgets.DropdownMenuElement<Pawn>
                    {
                        option = new FloatMenuOption(string.Format("{0}", pawn.LabelShortCap), delegate
                        {
                            this.TaskManager.DefaultPawn = pawn;
                        }, MenuOptionPriority.Default, null, null, 0f, null, null),
                        payload = pawn
                    };
                }
            }
        }

        private WorkGiverDef GetWorkgiver()
        {
            List<WorkGiverDef> defsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
            for (int index = 0; index < defsListForReading.Count; ++index)
            {
                WorkGiverDef workGiverDef = defsListForReading[index];
                WorkGiver_DoActivityTask worker = workGiverDef.Worker as WorkGiver_DoActivityTask;
                if (worker != null)
                    return workGiverDef;
            }
            Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", (object)SelFacility.ToString()), 57348750, false);
            return (WorkGiverDef)null;
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
            ScheduledDay day = TaskManager.ScheduledDays.FirstOrDefault(x => x.DayNumber == dayNumber);

            if (day != null)
                DrawDayInterFace(new Rect(rect.x + 6, rect.y + 30f, rect.width, rect.height * .75f), day);

            if (Mouse.IsOver(rect))
            {
                Widgets.DrawBox(rect, 3);
                if (Input.GetMouseButtonDown(0))
                {
                    if (day == null)
                        TaskManager.AddDay(dayNumber);

                    var options = GetFloatMenuOptions(day);
                    if (!options.NullOrEmpty())
                        Find.WindowStack.Add((Window)new Dialog_ScheduleDayConfig(day, GetFloatMenuOptions(day)));
                }
            }
        }

        private void DrawDayInterFace(Rect rect, ScheduledDay day)
        {
            float curY = rect.y;
            float height = rect.height / 5;

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
                Widgets.Label(new Rect(rect.x, curY, rect.width, height), "ReligionInfo_AndXTasksMore".Translate((NamedArgument)(day.Tasks.Count() - 3).ToString()));
            }
            Text.Font = GameFont.Small;
        }

        private void CreateNoPawnsOfReligionDialog(Religion religion)
        {
            Find.WindowStack.Add((Window)new Dialog_MessageBox("ReligionInfo_NoPawnsOfReligion".Translate((NamedArgument)religion.Label), (string)null, (Action)null, (string)null, (Action)null, (string)null, false, (Action)null, (Action)null));
        }

        private List<FloatMenuOption> GetFloatMenuOptions(ScheduledDay day)
        {
            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (ReligionProperty property in SelFacility.AssignedReligion.GetSettings<ReligionSettings_ReligionActivity>(SettingsTagDefOf.ActivityTag).Properties)
            {
                ActivityTaskDef taskDef = property.GetObject<ActivityTaskDef>();
                list.Add(new FloatMenuOption(taskDef.label, (Action)(() =>
                {
                    if (!this.SelFacility.Map.mapPawns.FreeColonists.Any<Pawn>(x => x.GetReligionComponent().Religion == SelFacility.AssignedReligion))
                        CreateNoPawnsOfReligionDialog(SelFacility.AssignedReligion);
                    ActivityTask task = new ActivityTask(day, property);
                    if(TaskManager.DefaultPawn != null)
                        task.PawnRestriction = TaskManager.DefaultPawn;
                    day.Add(task);
                    day.Reorder();
                }), MenuOptionPriority.Default, (Action)null, (Thing)null, 29f, (Func<Rect, bool>)(rect => Widgets.InfoCardButton(rect.x + 5f, rect.y + (float)(((double)rect.height - 24.0) / 2.0), taskDef)), (WorldObject)null));
            }
            if (!list.Any<FloatMenuOption>())
                list.Add(new FloatMenuOption("NoneBrackets".Translate(), (Action)null, MenuOptionPriority.Default, (Action)null, (Thing)null, 0.0f, (Func<Rect, bool>)null, (WorldObject)null));
            return list;
        }
    }
}
