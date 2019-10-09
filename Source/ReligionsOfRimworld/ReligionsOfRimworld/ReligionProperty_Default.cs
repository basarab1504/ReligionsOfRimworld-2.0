using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionProperty_Default : ReligionProperty
    {
        public override Def GetObject()
        {
            return MiscDefOf.TiedDown;
        }

        protected override string ObjectLabel => "ReligionInfo_DefaultProperty".Translate();
        protected override string Description => "ReligionInfo_DefaultPropertyDesc".Translate();
    }
}