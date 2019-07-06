using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ActivityJobQueue : IExposable
    {
        private List<ActivityJobNode> activityJobs;

        public ActivityJobQueue(IEnumerable<ActivityJobNode> activityJobs)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                this.activityJobs = new List<ActivityJobNode>();
                this.activityJobs.AddRange(activityJobs);
            }              
        }

        public int Count => activityJobs.Count;

        public bool NullOrEmpty()
        {
            return activityJobs.NullOrEmpty();
        }

        public ActivityJobNode this[int key]
        {
            get => activityJobs[key];
        }

        private void InitQueue()
        {
            //////////////
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<ActivityJobNode>(ref this.activityJobs, "activityJobs", LookMode.Deep);
        }
    }
}
