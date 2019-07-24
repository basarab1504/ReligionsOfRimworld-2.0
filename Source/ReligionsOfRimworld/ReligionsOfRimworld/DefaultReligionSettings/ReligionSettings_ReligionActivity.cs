using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_ReligionActivity : ReligionSettings
    {
        private List<ActivityTaskDef> properties;

        public ReligionSettings_ReligionActivity()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                properties = new List<ActivityTaskDef>();
        }

        public IEnumerable<ActivityTaskDef> Properties => properties;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            foreach (ActivityTaskDef property in properties)
                foreach (ReligionInfoEntry entry in property.GetInfoEntries())
                    yield return entry;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ActivityTaskDef>(ref this.properties, "properties", LookMode.Def);
        }
    }
}
