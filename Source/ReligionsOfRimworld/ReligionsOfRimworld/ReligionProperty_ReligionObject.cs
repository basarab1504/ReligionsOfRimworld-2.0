using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionProperty_ReligionGroupObject : ReligionProperty
    {
        private ReligionGroupTagDef propertyObject;

        public override Def GetObject()
        {
            return propertyObject;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ReligionGroupTagDef>(ref this.propertyObject, "religionGroupTagObject");
        }
    }
}