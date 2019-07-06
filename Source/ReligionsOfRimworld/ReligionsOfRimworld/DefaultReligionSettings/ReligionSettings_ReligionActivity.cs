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
        private List<ReligionProperty> activityRelics;
        private List<SkillRequirement> skillRequirements;

        public IEnumerable<ReligionProperty> ActivityRelics => activityRelics;
        public IEnumerable<SkillRequirement> SkillRequirements => skillRequirements;

        public ReligionSettings_ReligionActivity()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                activityRelics = new List<ReligionProperty>();
                skillRequirements = new List<SkillRequirement>();
            }
        }

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            foreach (ReligionProperty property in activityRelics)
                foreach (ReligionInfoEntry entry in property.GetInfoEntries())
                    yield return entry;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ActivityJobQueue>(ref this.activityJobQueue, "activityJobQueue");
            Scribe_Collections.Look<ReligionProperty>(ref this.activityRelics, "activityRelics", LookMode.Deep);
            Scribe_Collections.Look<SkillRequirement>(ref this.skillRequirements, "skillRequirements", LookMode.Deep);
        }
    }
}
