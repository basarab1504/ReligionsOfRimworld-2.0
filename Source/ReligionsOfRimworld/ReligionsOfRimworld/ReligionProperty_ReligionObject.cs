using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionProperty_ReligionGroupObject : ReligionProperty
    {
        private ReligionGroupTagDef religionGroupTagObject;

        public override Def GetObject()
        {
            return religionGroupTagObject;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ReligionGroupTagDef>(ref this.religionGroupTagObject, "religionGroupTagObject");
        }
    }
}