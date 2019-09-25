using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class ReligionSettings : IExposable, IDescribable
    { 
        protected SettingsTagDef tag;

        public SettingsTagDef Tag => tag;

        public string Label => tag.LabelCap;
        public string Description => tag.description;

        public abstract IEnumerable<ReligionInfoEntry> GetInfoEntries();

        public ReligionInfo GetInfoCategory()
        {
            ReligionInfo infoCategory = new ReligionInfo(Label);
            infoCategory.Add(new ReligionInfoEntry("ReligionInfo_Description".Translate(), "", Description));
            infoCategory.AddRange(GetInfoEntries());
            return infoCategory;
        }

        public virtual void ExposeData()
        {
            Scribe_Defs.Look<SettingsTagDef>(ref this.tag, "tag");
        }
    }
}
