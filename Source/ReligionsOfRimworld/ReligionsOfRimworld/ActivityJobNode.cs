using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ActivityJobNode : IExposable
    {
        private JobDef headPawnJob;
        private JobDef congregationMemberJob;

        public ActivityJobNode(JobDef headJob, JobDef memberJob)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                this.headPawnJob = headJob;
                this.congregationMemberJob = memberJob;
            }
        }

        public void ExposeData()
        {
            Scribe_Defs.Look<JobDef>(ref this.headPawnJob, "headPawnJob");
            Scribe_Defs.Look<JobDef>(ref this.congregationMemberJob, "congregationMemberJob");
        }
    }
}
