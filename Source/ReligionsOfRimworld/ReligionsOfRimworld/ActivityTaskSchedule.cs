using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ActivityTaskSchedule : IExposable
    {
        private Building_ReligiousBuildingFacility facility;
        private List<ScheduledDay> scheduledDays;
        private Pawn defaultPawn;

        public ActivityTaskSchedule(Building_ReligiousBuildingFacility facility)
        {
            this.facility = facility;
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                CreateSchedule();
            }     
        }

        public Building_ReligiousBuildingFacility Facility => facility;
        public IEnumerable<ScheduledDay> ScheduledDays => scheduledDays;
        public Pawn DefaultPawn { get => defaultPawn; set => defaultPawn = value; }

        public IEnumerable<ActivityTask> TodayTasks
        {
            get
            {
                ScheduledDay today = scheduledDays[GenLocalDate.DayOfQuadrum(Find.CurrentMap)];
                return today.Tasks;
            }
        }

        public IEnumerable<ActivityTask> AllTasks()
        {
            foreach (ScheduledDay day in scheduledDays)
                foreach (ActivityTask task in day.Tasks)
                    yield return task;
        }

        public void AddDay(int dayNumber)
        {
            if (!scheduledDays.Contains(scheduledDays.FirstOrDefault(x => x.DayNumber == dayNumber)))
                scheduledDays.Add(new ScheduledDay(this, dayNumber));
        }

        public void RemoveDay(int dayNumber)
        {
            scheduledDays.Remove(scheduledDays.FirstOrDefault(x => x.DayNumber == dayNumber));
        }

        public void Reorder()
        {
            scheduledDays.RemoveAll(x => x.Tasks.Count() == 0);

            foreach (ScheduledDay day in scheduledDays)
            {
                if (day.Tasks.Count() == 0)
                    scheduledDays.Remove(day);
                day.Reorder();
            }
        }

        public void Clear()
        {
            CreateSchedule();
        }

        public void ValidateSettings()
        {
            if (defaultPawn != null && defaultPawn.Dead)
                defaultPawn = null;

            foreach (ScheduledDay day in scheduledDays)
                day.ValidateSettings();
        }

        private void CreateSchedule()
        {
            scheduledDays = new List<ScheduledDay>();
            {
                for (int i = 1; i < 16; ++i)
                    AddDay(i);
            }
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<ScheduledDay>(ref this.scheduledDays, false, "scheduledDays", LookMode.Deep, this, 0);
            Scribe_References.Look<Pawn>(ref this.defaultPawn, "defaultPawn");
        }
    }
}
