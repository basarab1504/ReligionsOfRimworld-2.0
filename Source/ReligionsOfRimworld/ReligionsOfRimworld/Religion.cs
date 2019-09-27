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
        private ReligionDef religionDef;
        private ReligionConfiguration configuration;
        private ReligionSettings_PietyNeed needSettings;
        private ReligionSettings_JoiningCriteria joiningRestrictionsSettings;
        private ReligionSettings_ReligionTalks religionTalksSettings;
        private ReligionSettings_Incidents incidentsSettings;
        private ReligionSettings_MentalBreaks mentalBreaksSettings;
        private ReligionSettings_Social opinionSettings;
        private ReligionSettings_Social deathSettings;
        private ReligionSettings_Social killSettings;
        private ReligionSettings_Social weaponSettings;
        private ReligionSettings_Social foodSettings;
        private ReligionSettings_Social apparelSettings;
        private ReligionSettings_AllowedBuildings allowedBuildingsSettings;
        private ReligionSettings_Prayings prayingSettings;

        public Religion(ReligionConfiguration configuration)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                loadID = Find.UniqueIDsManager.GetNextThingID();
                this.configuration = configuration;
                InitializeReligion();
            }
        }

        public Religion(ReligionDef def)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                this.religionDef = def;
                loadID = Find.UniqueIDsManager.GetNextThingID();
                this.configuration = new ReligionConfiguration(def);
                InitializeReligion();
            }
        }

        public string Label => configuration.Label;
        public string Description => configuration.Description;
        public ReligionGroupTagDef GroupTag => configuration.GroupTag;
        public IEnumerable<ReligionSettings> AllSettings => configuration.Settings;
        public ReligionSettings_PietyNeed NeedSettings => needSettings;
        public ReligionSettings_JoiningCriteria JoiningRestrictionsSettings => joiningRestrictionsSettings;
        public ReligionSettings_ReligionTalks ReligionTalksSettings => religionTalksSettings;
        public ReligionSettings_Incidents IncidentsSettings => incidentsSettings;
        public ReligionSettings_MentalBreaks MentalBreaksSettings => mentalBreaksSettings;
        public ReligionSettings_Social OpinionSettings => opinionSettings;
        public ReligionSettings_Social DeathSettings => deathSettings;
        public ReligionSettings_Social KillSettings => killSettings;
        public ReligionSettings_Social WeaponSettings => weaponSettings;
        public ReligionSettings_Social FoodSettings => foodSettings;
        public ReligionSettings_Social ApparelSettings => apparelSettings;
        public ReligionSettings_AllowedBuildings AllowedBuildingsSettings => allowedBuildingsSettings;
        public ReligionSettings_Prayings PrayingSettings => prayingSettings;

        public void TryAddSettings(ReligionSettings settings)
        {
            configuration.TryAddSettings(settings);
        }

        public void TryRemoveSettings(SettingsTagDef settingsTag)
        {
            configuration.TryRemoveSettings(settingsTag);
        }

        public void TryChangeSettings(ReligionSettings settings)
        {
            configuration.TryChangeSettings(settings);
        }

        private void InitializeReligion()
        {
            needSettings = configuration.FindByTag<ReligionSettings_PietyNeed>(SettingsTagDefOf.NeedTag);
            joiningRestrictionsSettings = configuration.FindByTag<ReligionSettings_JoiningCriteria>(SettingsTagDefOf.JoiningCriteriaTag);
            religionTalksSettings = configuration.FindByTag<ReligionSettings_ReligionTalks>(SettingsTagDefOf.TalksTag);
            incidentsSettings = configuration.FindByTag<ReligionSettings_Incidents>(SettingsTagDefOf.IncidentsTag);
            mentalBreaksSettings = configuration.FindByTag<ReligionSettings_MentalBreaks>(SettingsTagDefOf.MentalBreaksTag);
            opinionSettings = configuration.FindByTag<ReligionSettings_Social>(SettingsTagDefOf.OpinionTag);
            deathSettings = configuration.FindByTag<ReligionSettings_Social>(SettingsTagDefOf.DeathTag);
            killSettings = configuration.FindByTag<ReligionSettings_Social>(SettingsTagDefOf.KillTag);
            weaponSettings = configuration.FindByTag<ReligionSettings_Social>(SettingsTagDefOf.WeaponTag);
            foodSettings = configuration.FindByTag<ReligionSettings_Social>(SettingsTagDefOf.FoodTag);
            apparelSettings = configuration.FindByTag<ReligionSettings_Social>(SettingsTagDefOf.ApparelTag);
            allowedBuildingsSettings = configuration.FindByTag<ReligionSettings_AllowedBuildings>(SettingsTagDefOf.AllowedBuildingsTag);
            prayingSettings = configuration.FindByTag<ReligionSettings_Prayings>(SettingsTagDefOf.PrayingsTag);
        }

        public T FindByTag<T>(SettingsTagDef tag) where T : ReligionSettings
        {
            return (T)configuration.FindByTag<T>(tag);
        }


        public IEnumerable<ReligionInfoCategory> GetInfo()
        {
            return configuration.GetInfo();
        }

        public string GetUniqueLoadID()
        {
            return "Religion_" + this.loadID;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.loadID, "loadID");
            Scribe_Deep.Look<ReligionConfiguration>(ref configuration, "configuration", null, null, null, null);
            if (Scribe.mode == LoadSaveMode.LoadingVars)
                Scribe_Defs.Look<ReligionDef>(ref this.religionDef, "religionDef");
            if (religionDef != null)
                Scribe_Deep.Look<ReligionConfiguration>(ref configuration, "configuration", new object[4]);
            else
                Scribe_Deep.Look<ReligionConfiguration>(ref configuration, "configuration", religionDef);
            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                InitializeReligion();
            }
        }
    }
}
