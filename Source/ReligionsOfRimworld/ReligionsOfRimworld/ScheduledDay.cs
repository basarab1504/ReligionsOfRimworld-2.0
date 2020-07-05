using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class ScheduledDay : IExposable
    {
        private int dayNumber;
        private ActivityTaskSchedule schedule;
        private List<ActivityTask> tasks;

        public ScheduledDay(ActivityTaskSchedule schedule, int dayNumber)
        {
            this.schedule = schedule;
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                this.dayNumber = dayNumber;
                tasks = new List<ActivityTask>();
            }
        }

        public ScheduledDay(ScheduledDay day)
        {
            this.schedule = day.schedule;
            this.dayNumber = day.dayNumber;
            this.tasks = new List<ActivityTask>();
            foreach(var task in day.tasks)
            {
                this.tasks.Add(new ActivityTask(task));
            }
        }

        public int DayNumber => dayNumber;
        public IEnumerable<ActivityTask> Tasks => tasks;
        public ActivityTaskSchedule ParentSchedule => schedule;

        public bool AnyShouldDoNow()
        {
            if (GenLocalDate.DayOfSeason(Find.CurrentMap) == dayNumber)
                return tasks.Any(x => x.ShouldDoNow());
            return false;
        }

        public bool ShouldDoNow(ActivityTask task)
        {
            if (tasks.Contains(task))
                if (GenLocalDate.DayOfSeason(Find.CurrentMap) == dayNumber && task.ShouldDoNow())
                    return true;
            return false;
        }

        public void Add(ActivityTask task)
        {
            tasks.Add(task);
        }

        public void Remove(ActivityTask task)
        {
            tasks.Remove(task);
        }

        public void Reorder()
        {
            tasks.Sort((x, y) => x.StartHour.CompareTo(y.StartHour));
        }

        public void ValidateSettings()
        {
            foreach (ActivityTask task in tasks)
                task.ValidateSettings();
        }

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref dayNumber, "dayNumber");
            Scribe_Collections.Look<ActivityTask>(ref tasks, "tasks", LookMode.Deep, this, null);
        }
    }
}
