using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityProperty
    {
        private ReligionProperty_ThingObject property;
        private int count;

        public ThingDef Relic => (ThingDef)property.GetObject();
        public int Count => count;

        public IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            //yield return new ReligionInfoEntry("");
            if (property != null)
                foreach (ReligionInfoEntry entry in property.GetInfoEntries())
                    yield return entry;
        }

        public virtual void ExposeData()
        {
            Scribe_Deep.Look<ReligionProperty_ThingObject>(ref this.property, "property");
            Scribe_Deep.Look<int>(ref this.count, "count");
        }
    }
}
