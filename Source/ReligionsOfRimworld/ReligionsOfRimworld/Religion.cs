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
        private ReligionSettings_Incidents incidentsSettings;
        private ReligionSettings_MentalBreaks mentalBreaksSettings;

        public Religion(ReligionConfiguration configuration)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                loadID = Find.UniqueIDsManager.GetNextThingID();
                this.configuration = configuration;
                InitializeReligion();
            }
        }

        public string Label => configuration.Label;
        public string Description => configuration.Description;
        public ReligionConfiguration Configuration => configuration;
        public ReligionSettings_Need NeedSettings => needSettings;
        public ReligionSettings_JoiningRestriction JoiningRestrictionsSettings => joiningRestrictionsSettings;
        public ReligionSettings_ReligionTalks ReligionTalksSettings => religionTalksSettings;
        public ReligionSettings_Incidents IncidentsSettings => incidentsSettings;
        public ReligionSettings_MentalBreaks MentalBreaksSettings => mentalBreaksSettings;

        public T FindByTag<T>(SettingsTagDef tag) where T : ReligionSettings
        {
            return (T)configuration.FindByTag<T>(tag);
        }

        private void InitializeReligion()
        {
            needSettings = configuration.FindByTag<ReligionSettings_Need>(SettingsTagDefOf.NeedTag);
            joiningRestrictionsSettings = configuration.FindByTag<ReligionSettings_JoiningRestriction>(SettingsTagDefOf.JoiningRestrictionsTag);
            religionTalksSettings = configuration.FindByTag<ReligionSettings_ReligionTalks>(SettingsTagDefOf.TalksTag);
            incidentsSettings = configuration.FindByTag<ReligionSettings_Incidents>(SettingsTagDefOf.IncidentsTag);
            mentalBreaksSettings = configuration.FindByTag<ReligionSettings_MentalBreaks>(SettingsTagDefOf.MentalBreaksTag);
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
        }
    }
}
