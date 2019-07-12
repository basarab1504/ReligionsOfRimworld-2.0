using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_ReligionActivity : ReligionSettings
    {
        private List<ReligionActivityProperty> properties;

        public ReligionSettings_ReligionActivity()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                properties = new List<ReligionActivityProperty>();
        }

        public IEnumerable<ReligionActivityProperty> Properties => properties;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            foreach (ReligionActivityProperty property in properties)
                foreach (ReligionInfoEntry entry in property.GetInfoEntries())
                    yield return entry;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ReligionActivityProperty>(ref this.properties, "properties");
        }
    }
}
