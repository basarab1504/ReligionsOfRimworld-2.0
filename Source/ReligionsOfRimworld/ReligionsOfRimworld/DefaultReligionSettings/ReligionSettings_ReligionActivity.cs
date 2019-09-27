using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_ReligionActivity : ReligionSettings
    {
        private List<ReligionProperty> properties;

        public ReligionSettings_ReligionActivity()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                properties = new List<ReligionProperty>();
        }

        public IEnumerable<ReligionProperty> Properties => properties;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            foreach (ReligionProperty property in properties)
                yield return property.GetReligionInfoEntry();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ReligionProperty>(ref this.properties, "properties", LookMode.Deep);
        }
    }
}
