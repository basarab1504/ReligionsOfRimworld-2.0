using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_Prayings : ReligionSettings
    {
        private ReligionPropertyData prayProperty;
        private JobDef prayJob;
        private int prayIntervalHours;

        public ReligionPropertyData PrayProperty => prayProperty;
        public JobDef PrayJob => prayJob;
        public int PrayIntervalHours => prayIntervalHours;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            yield return new ReligionInfoEntry("ReligionInfo_PrayJob".Translate(), prayJob.LabelCap, prayJob.description);

            foreach (ReligionInfoEntry entry in prayProperty.GetInfoEntries())
                yield return entry;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ReligionPropertyData>(ref prayProperty, "prayProperty");
            Scribe_Defs.Look<JobDef>(ref prayJob, "prayJob");
            Scribe_Values.Look<int>(ref prayIntervalHours, "prayIntervalHours");
        }
    }
}
