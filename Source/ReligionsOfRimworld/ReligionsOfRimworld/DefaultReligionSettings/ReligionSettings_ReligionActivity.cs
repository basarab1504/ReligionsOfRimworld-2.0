using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_ReligionActivity : ReligionSettings
    {
        private List<ReligionActivityDef> properties;

        public ReligionSettings_ReligionActivity()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                properties = new List<ReligionActivityDef>();
        }

        public IEnumerable<ReligionActivityDef> Properties => properties;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            foreach (ReligionActivityDef property in properties)
                foreach (ReligionInfoEntry entry in property.GetInfoEntries())
                    yield return entry;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ReligionActivityDef>(ref this.properties, "properties", LookMode.Def);
        }
    }
}
