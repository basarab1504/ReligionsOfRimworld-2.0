using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_Need : ReligionSettings
    {
        public NeedDef needDef;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            if(needDef != null)
                yield return new ReligionInfoEntry("ReligionInfo_SeekerFallPerHour".Translate(), needDef.seekerFallPerHour.ToString());
        }
    }
}
