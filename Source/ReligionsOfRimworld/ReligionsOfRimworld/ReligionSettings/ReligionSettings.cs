using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class ReligionSettings : IExposable
    { 
        protected SettingsTagDef tag;

        public SettingsTagDef Tag => tag;

        public abstract IEnumerable<ReligionInfoEntry> GetInfoEntries();

        public ReligionInfo GetInfoCategory()
        {
            ReligionInfo infoCategory = new ReligionInfo(tag.LabelCap);
            infoCategory.Add(new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", tag.description));
            infoCategory.AddRange(GetInfoEntries());
            return infoCategory;
        }

        public virtual void ExposeData()
        {
            Scribe_Defs.Look<SettingsTagDef>(ref this.tag, "tag");
        }
    }
}
