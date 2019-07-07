using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_ReligionActivity : ReligionSettings
    {
        private ActivityJobQueue activityJobQueue;
        private List<ReligionActivityProperty> activityRelics;
        private List<SkillRequirement> skillRequirements;

        public IEnumerable<ReligionActivityProperty> ActivityRelics => activityRelics;
        public IEnumerable<SkillRequirement> SkillRequirements => skillRequirements;

        public ReligionSettings_ReligionActivity()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                activityRelics = new List<ReligionActivityProperty>();
                skillRequirements = new List<SkillRequirement>();
            }
        }

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            foreach (ReligionActivityProperty property in activityRelics)
                foreach (ReligionInfoEntry entry in property.GetInfoEntries())
                    yield return entry;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ActivityJobQueue>(ref this.activityJobQueue, "activityJobQueue");
            Scribe_Collections.Look<ReligionActivityProperty>(ref this.activityRelics, "activityRelics", LookMode.Deep);
            Scribe_Collections.Look<SkillRequirement>(ref this.skillRequirements, "skillRequirements", LookMode.Deep);
        }
    }
}
