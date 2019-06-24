using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Religion : IExposable, ILoadReferenceable
    {
        private int loadID;
        private ReligionConfiguration configuration;
        private ReligionSettings_Need needSettings;
        private ReligionSettings_JoiningRestriction joiningRestrictionsSettings;
        private ReligionSettings_ReligionTalks religionTalksSettings;

        public Religion(ReligionConfiguration configuration)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                loadID = Find.UniqueIDsManager.GetNextThingID();
                this.configuration = configuration;
                InitializeReligion();
            }
        }

        public ReligionConfiguration Configuration => configuration;
        public ReligionSettings_Need NeedSettings { get => needSettings; set => needSettings = value; }
        public ReligionSettings_JoiningRestriction JoiningRestrictionsSettings { get => joiningRestrictionsSettings; set => joiningRestrictionsSettings = value; }
        public ReligionSettings_ReligionTalks ReligionTalksSettings { get => religionTalksSettings; set => religionTalksSettings = value; }
        public string Label => configuration.Label;
        public string Description => configuration.Description;

        private void InitializeReligion()
        {
            needSettings = configuration.FindByTag<ReligionSettings_Need>(SettingsTagDefOf.NeedTag);
            joiningRestrictionsSettings = configuration.FindByTag<ReligionSettings_JoiningRestriction>(SettingsTagDefOf.JoiningRestrictionTag);
            religionTalksSettings = configuration.FindByTag<ReligionSettings_ReligionTalks>(SettingsTagDefOf.ReligionTalksTag);
        }

        public string GetUniqueLoadID()
        {
            return "Religion_" + this.loadID;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.loadID, "loadID");
            Scribe_Deep.Look<ReligionConfiguration>(ref configuration, "configuration", null, null, null);
            if(Scribe.mode == LoadSaveMode.LoadingVars)
                InitializeReligion();
            //Scribe_Deep.Look<ReligionSettings_Need>(ref needSettings, "needSettings");
            //Scribe_Deep.Look<ReligionSettings_JoiningRestriction>(ref joiningRestrictionsSettings, "joiningRestrictionsSettings");
            //Scribe_Deep.Look<ReligionSettings_ReligionTalks>(ref religionTalksSettings, "religionTalksSettings");
        }
    }
}
