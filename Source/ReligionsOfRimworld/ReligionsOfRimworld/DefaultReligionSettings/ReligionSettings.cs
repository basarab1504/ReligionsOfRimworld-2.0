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
        public string Label => tag.LabelCap;
        public string Description => tag.description;

        //public abstract IEnumerable<ReligionInfoEntry> GetInfoEntries();

        //public ReligionInfoCategory GetInfoCategory()
        //{
        //    return new ReligionInfoCategory(Label, Description, GetInfoEntries());
        //}

        public virtual void ExposeData()
        {
            Scribe_Defs.Look<SettingsTagDef>(ref this.tag, "tag");
        }
    }
}
