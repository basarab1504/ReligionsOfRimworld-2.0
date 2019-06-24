using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionConfiguration : IExposable
    {
        private string label;
        private string description;
        private List<ReligionSettings> settings;

        public ReligionConfiguration(string label, string description, IEnumerable<ReligionSettings> settings)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                this.label = label;
                this.description = description;
                this.settings = new List<ReligionSettings>();
                this.settings.AddRange(settings);
            }
        }

        public string Label => label;
        public string Description => description;
        public IEnumerable<ReligionSettings> Settings => settings;

        public IEnumerable<ReligionInfo> GetInfo()
        {
            ReligionInfo category = new ReligionInfo("ReligionInfo_Overall".Translate());
            category.Add(new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", description));
            yield return category;
            foreach (ReligionSettings setting in settings)
                yield return setting.GetInfoCategory();
        }

        public ReligionSettings FindByTag(SettingsTagDef tag)
        {
            return settings.FirstOrDefault(x => x.Tag == tag);
        }

        public T FindByTag<T>(SettingsTagDef tag) where T : ReligionSettings
        {
            return (T)settings.FirstOrDefault(x => x.Tag == tag);
        }

        public void ExposeData()
        {
            Scribe_Values.Look<string>(ref this.label, "label");
            Scribe_Values.Look<string>(ref this.description, "descrtiption");
            Scribe_Collections.Look<ReligionSettings>(ref this.settings, "settings", LookMode.Deep);
        }
    }
}
