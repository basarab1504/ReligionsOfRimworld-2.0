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

        public ReligionSettings Settings => settings;

        //public ReligionInfo GetInfoCategory()
        //{
        //    ReligionInfo infoCategory = new ReligionInfo(LabelCap);
        //    infoCategory.Add(new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", description));
        //    infoCategory.AddRange(settings.GetInfoEntries());
        //    return infoCategory;
        //}
    }
}
