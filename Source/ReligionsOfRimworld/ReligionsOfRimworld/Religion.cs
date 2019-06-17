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
        public ReligionDef def;
        public ReligionSettings_Need needSettings;

        public Religion()
        {
        }

        public Religion(ReligionDef def)
        {
            loadID = Find.UniqueIDsManager.GetNextThingID();
            this.def = def;
            InitializeReligion();
        }

        private void InitializeReligion()
        {
            needSettings = def.FindByTag<ReligionSettings_Need>(SettingsTagDefOf.NeedTag);
        }

        public string GetUniqueLoadID()
        {
            return "Religion_" + (object)this.loadID;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.loadID, "loadID");
            Scribe_Defs.Look<ReligionDef>(ref def, "religion");
            Scribe_Deep.Look<ReligionSettings_Need>(ref needSettings, "needSettings");
        }
    }
}
