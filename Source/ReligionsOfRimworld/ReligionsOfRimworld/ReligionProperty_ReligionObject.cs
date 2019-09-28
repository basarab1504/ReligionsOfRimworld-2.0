using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionProperty_ReligionGroupDef : ReligionProperty
    {
        private ReligionGroupTagDef propertyObject;

        public override Def GetObject()
        {
            return propertyObject;
        }

        protected override string ObjectLabel => propertyObject.LabelCap;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ReligionGroupTagDef>(ref this.propertyObject, "religionGroupTagObject");
        }
    }
}