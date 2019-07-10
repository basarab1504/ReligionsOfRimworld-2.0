using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ActivityJobNode : IExposable
    {
        private JobDef organizerJob;
        private JobDef congregationJob;

        public ActivityJobNode()
        { }

        public ActivityJobNode(JobDef headJob, JobDef memberJob)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                this.organizerJob = headJob;
                this.congregationJob = memberJob;
            }
        }

        public JobDef OrganizerJob => organizerJob;
        public JobDef CongregationJob => congregationJob;

        public void ExposeData()
        {
            Scribe_Defs.Look<JobDef>(ref this.organizerJob, "organizerJob");
            Scribe_Defs.Look<JobDef>(ref this.congregationJob, "congregationJob");
        }
    }
}
