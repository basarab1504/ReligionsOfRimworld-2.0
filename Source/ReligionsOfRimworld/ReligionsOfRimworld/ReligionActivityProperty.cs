using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityProperty
    {
        private ThingDef relic;
        private ReligionPropertyData headPawnProperty;
        private ReligionPropertyData congregationMemberProperty;

        public ReligionPropertyData HeadPawnProperty => headPawnProperty;
        public ReligionPropertyData CongregationMemberProperty => congregationMemberProperty;

        public IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            //yield return new ReligionInfoEntry("");
            if (relic != null)
                yield return new ReligionInfoEntry("ReligionInfo_Object".Translate(), relic.LabelCap, relic.description);

            if (headPawnProperty != null)
                foreach (ReligionInfoEntry entry in headPawnProperty.GetInfoEntries())
                    yield return entry;

            if (congregationMemberProperty != null)
                foreach (ReligionInfoEntry entry in congregationMemberProperty.GetInfoEntries())
                    yield return entry;
        }

        public virtual void ExposeData()
        {
            Scribe_Defs.Look<ThingDef>(ref this.relic, "relic");
            Scribe_Deep.Look<ReligionPropertyData>(ref this.headPawnProperty, "headPawnProperty");
            Scribe_Deep.Look<ReligionPropertyData>(ref this.congregationMemberProperty, "congregationMemberProperty");
        }
    }
}
