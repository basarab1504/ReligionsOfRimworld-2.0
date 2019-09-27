using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionProperty_JobDef : ReligionProperty
    {
        private JobDef propertyObject;

        public override Def GetObject()
        {
            return propertyObject;
        }

        protected override string ObjectLabel => "ReligionInfo_Job".Translate();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<JobDef>(ref this.propertyObject, "jobDef");
        }
    }
}