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
        private List<ActivityTask> tasks;

        public ScheduledDay(int dayNumber)
        {
            this.dayNumber = dayNumber;
            tasks = new List<ActivityTask>();
        }

        public int DayNumber => dayNumber;
        public IEnumerable<ActivityTask> Tasks => tasks;

        public bool AnyShouldDoNow => tasks.Any(x => x.ShouldDoNow());

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

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref dayNumber, "dayNumber");
            Scribe_Collections.Look<ActivityTask>(ref tasks, "tasks", LookMode.Deep);
        }
    }
}
