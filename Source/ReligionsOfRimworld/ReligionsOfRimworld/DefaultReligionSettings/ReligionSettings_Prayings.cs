using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_Prayings : ReligionSettings
    {
        private NeedDef prayNeed;
        private ReligionPropertyData prayProperty;
        private JobDef prayJob;

        public NeedDef Need => prayNeed;
        public ReligionPropertyData PrayProperty => prayProperty;
        public JobDef PrayJob => prayJob;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            yield return new ReligionInfoEntry("ReligionInfo_PrayNeed".Translate(), prayNeed.LabelCap, prayNeed.description);
            yield return new ReligionInfoEntry("ReligionInfo_SeekerFallPerHour".Translate(), prayNeed.seekerFallPerHour.ToString());
            yield return new ReligionInfoEntry("ReligionInfo_PrayJob".Translate(), prayJob.LabelCap, GetDescription());
        }

        private string GetDescription()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(prayJob.description);
            stringBuilder.AppendLine();
            if (prayProperty != null)
            {
                stringBuilder.AppendLine("ReligionInfo_OrganizerProperty".Translate());
                stringBuilder.Append(prayProperty.GetInfo());
            }
            return stringBuilder.ToString();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<NeedDef>(ref this.prayNeed, "prayNeed");
            Scribe_Deep.Look<ReligionPropertyData>(ref prayProperty, "prayProperty");
            Scribe_Defs.Look<JobDef>(ref prayJob, "prayJob");
        }
    }
}
