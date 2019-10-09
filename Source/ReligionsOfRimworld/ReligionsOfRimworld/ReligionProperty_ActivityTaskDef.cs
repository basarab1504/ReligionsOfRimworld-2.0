using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionProperty_ActivityTaskDef : ReligionProperty
    {
        private ActivityTaskDef propertyObject;

        public override Def GetObject()
        {
            return propertyObject;
        }

        protected override string ObjectLabel => propertyObject.LabelCap;
        protected override string Description => propertyObject.description;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ActivityTaskDef>(ref this.propertyObject, "activityTaskDef");
        }
    }
}