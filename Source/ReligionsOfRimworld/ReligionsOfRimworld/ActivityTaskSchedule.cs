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

        public ActivityTaskSchedule(Building_ReligiousBuildingFacility facility)
        {
            this.facility = facility;
            scheduledDays = new List<ScheduledDay>(15);
        }

        public Building_ReligiousBuildingFacility Facility => facility;
        public IEnumerable<ScheduledDay> ScheduledDays => scheduledDays;

        public IEnumerable<ActivityTask> AllTasks()
        {
            foreach (ScheduledDay day in scheduledDays)
                foreach (ActivityTask task in day.Tasks)
                    yield return task;
        }

        public bool AnyShouldDoNow()
        {
            return scheduledDays.Any(x => x.AnyShouldDoNow());
        }

        public bool ShouldDoNow(ActivityTask task)
        {
            if (scheduledDays.Any(x => x.ShouldDoNow(task)))
                return true;
            return false;
        }

        public bool PawnAllowedToStartAnew(Pawn p, ActivityTask task)
        {
            return task.PawnAllowedToStartAnew(p);
        }

        public void Create(int dayNumber)
        {
            if (!scheduledDays.Contains(scheduledDays.FirstOrDefault(x => x.DayNumber == dayNumber)))
                scheduledDays.Add(new ScheduledDay(dayNumber));
        }

        public void Delete(int dayNumber)
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

        public void ExposeData()
        {
            Scribe_Collections.Look<ScheduledDay>(ref this.scheduledDays, "scheduledDays", LookMode.Deep, null, null);
        }
    }
}
