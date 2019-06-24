using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionDef : Def
    {
        private List<ReligionSettingsDef> settingsDefs;

        public ReligionDef()
        {
            settingsDefs = new List<ReligionSettingsDef>();
        }

        public IEnumerable<ReligionSettingsDef> SettingsDefs => settingsDefs;

        //public IEnumerable<ReligionInfo> GetInfo()
        //{
        //    ReligionInfo category = new ReligionInfo("ReligionInfo_Overall".Translate());
        //    category.Add(new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", description));
        //    yield return category;
        //    foreach (ReligionSettingsDef def in settingsDefs)
        //        yield return def.GetInfoCategory();
        //}

        //public ReligionSettings FindByTag(SettingsTagDef tag)
        //{
        //    return settingsDefs.FirstOrDefault(x => x.Tag == tag).Settings;
        //}

        //public T FindByTag<T>(SettingsTagDef tag) where T : ReligionSettings
        //{
        //    return (T)settingsDefs.FirstOrDefault(x => x.Tag == tag).Settings;
        //}
    }
}
