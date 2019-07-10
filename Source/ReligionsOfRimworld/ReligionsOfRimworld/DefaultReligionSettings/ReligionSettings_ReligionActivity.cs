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
            yield return new ReligionInfoEntry("");
            //yield return new ReligionInfoEntry("");
            //if (GetObject() != null)
            //    yield return new ReligionInfoEntry("ReligionInfo_Object".Translate(), GetObject().LabelCap, GetObject().description);

            //if (subject != null)
            //    foreach (ReligionInfoEntry entry in subject.GetInfoEntries())
            //        yield return entry;

            //if (witness != null)
            //    foreach (ReligionInfoEntry entry in witness.GetInfoEntries())
            //        yield return entry;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ReligionActivityProperty>(ref this.properties, "properties");
        }
    }
}
