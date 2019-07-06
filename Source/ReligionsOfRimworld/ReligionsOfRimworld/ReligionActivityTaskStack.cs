using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityTaskStack : IExposable
    {
        private List<ReligionActivityTask> tasks = new List<ReligionActivityTask>();

        public IEnumerable<ReligionActivityTask> Tasks => tasks;
        
        public void AddTask(ReligionActivityTask task)
        {
            tasks.Add(task);
        }

        public void Remove(ReligionActivityTask task)
        {
            tasks.Remove(task);
        }

        public ReligionActivityTask FirstShouldDoNow
        {
            get
            {
                foreach (ReligionActivityTask task in tasks)
                    if (task.ShouldDoNow)
                        return task;
                return null;
            }
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<ReligionActivityTask>(ref this.tasks, "tasks", LookMode.Deep, null);
        }
    }
}
