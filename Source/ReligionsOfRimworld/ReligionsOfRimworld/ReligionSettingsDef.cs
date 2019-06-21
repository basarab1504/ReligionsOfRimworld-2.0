using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettingsDef : Def
    {
        private ReligionSettings settings;
        private SettingsTagDef tag;

        public ReligionSettings Settings
        {
            get => settings;
        }

        public SettingsTagDef Tag
        {
            get => tag;
        }

        public ReligionInfo GetInfoCategory()
        {
            ReligionInfo infoCategory = new ReligionInfo(LabelCap);
            infoCategory.Add(new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", description));
            infoCategory.AddRange(settings.GetInfoEntries());
            return infoCategory;
        }
    }
}
