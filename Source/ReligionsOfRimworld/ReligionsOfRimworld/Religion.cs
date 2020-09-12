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
        private ReligionDef def;
        private Dictionary<SettingsTagDef, ReligionSettings> settings = new Dictionary<SettingsTagDef, ReligionSettings>();

        public Religion()
        {}

        public int ID => loadID;

        public Religion(ReligionDef def)
        {
            this.def = def;
            loadID = ReligionsBuffer.GetNextID();

            foreach (var settingsDef in def.Settings)
                settings.Add(settingsDef.Settings.Tag, settingsDef.Settings);
        }

        public string Label => def.LabelCap;
        public string Description => def.description;
        public ReligionDef Def => def;
        public ReligionGroupTagDef GroupTag => def.GroupTag;
        public IEnumerable<ReligionSettings> AllSettings => settings.Values;

        public T GetSettings<T>() where T : ReligionSettings
        {
            T settings = default(T);
            foreach(T s in AllSettings)
            {
                settings = s;
            }
            return settings;
        }

        public T GetSettings<T>(SettingsTagDef tag) where T : ReligionSettings
        {
            if (settings.ContainsKey(tag))
                return (T)settings[tag];
            else
                return null;
        }

        public ReligionSettings GetSettings(SettingsTagDef tag)
        {
            if (settings.ContainsKey(tag))
                return settings[tag];
            else
                return null;
        }

        //public IEnumerable<ReligionInfoCategory> GetInfo()
        //{
        //    ReligionInfoCategory category = new ReligionInfoCategory("ReligionInfo_Overall".Translate(), def.description);
        //    category.Add(new ReligionInfoEntry("ReligionInfo_Religion".Translate(), def.LabelCap, def.description));
        //    yield return category;

        //    foreach (var setting in def.Settings)
        //        yield return setting.Settings.GetInfoCategory();
        //}

        public string GetUniqueLoadID()
        {
            return "Religion_" + this.loadID;
        }

        public void ExposeData()
        {
            Scribe_Defs.Look<ReligionDef>(ref def, "religionDef");
            Scribe_Values.Look<int>(ref this.loadID, "loadID");

            if(Scribe.mode == LoadSaveMode.LoadingVars)
                foreach (var settingsDef in def.Settings)
                    settings.Add(settingsDef.Settings.Tag, settingsDef.Settings);
        }
    }
}
