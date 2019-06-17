using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettingsDef : Def
    {
        public ReligionSettings settings;
        public SettingsTagDef tag;

        public ReligionInfo GetInfoCategory()
        {
            ReligionInfo infoCategory = new ReligionInfo(LabelCap);
            infoCategory.Add(new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", description));
            infoCategory.AddRange(settings.GetInfoEntries());
            return infoCategory;
        }
    }
}
