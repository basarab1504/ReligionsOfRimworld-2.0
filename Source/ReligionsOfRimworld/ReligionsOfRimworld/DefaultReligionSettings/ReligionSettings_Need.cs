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
        private NeedDef need;
        private ThoughtDef needThought;

        public NeedDef Need => need;
        public ThoughtDef NeedThought => needThought;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            if(need != null)
                yield return new ReligionInfoEntry("ReligionInfo_SeekerFallPerHour".Translate(), need.seekerFallPerHour.ToString());
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<NeedDef>(ref this.need, "need");
            Scribe_Defs.Look<ThoughtDef>(ref this.needThought, "needThought");
        }
    }
}
