using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_PietyNeed : ReligionSettings
    {
        private NeedDef pietyNeed;
        private ThoughtDef needThought;

        public NeedDef PietyNeed => pietyNeed;
        public ThoughtDef NeedThought => needThought;

        //public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        //{
        //    if(pietyNeed != null)
        //    {
        //        yield return new ReligionInfoEntry("ReligionInfo_PietyNeed".Translate(), pietyNeed.LabelCap, pietyNeed.description);
        //        yield return new ReligionInfoEntry("ReligionInfo_SeekerFallPerHour".Translate(), pietyNeed.seekerFallPerHour.ToString());
        //    }
        //}

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<NeedDef>(ref this.pietyNeed, "pietyNeed");
            Scribe_Defs.Look<ThoughtDef>(ref this.needThought, "needThought");
        }
    }
}
