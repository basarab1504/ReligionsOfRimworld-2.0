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
        private ReligionSettings_Need needSettings;

        private Religion() //Do not use! Used on game loading
        {        
        }

        public Religion(ReligionDef def) : this()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                loadID = Find.UniqueIDsManager.GetNextThingID();
                this.def = def;
                InitializeReligion();
            }
        }

        public ReligionSettings_Need NeedSettings
        {
            get => needSettings;
            set => needSettings = value;
        }

        public ReligionDef ReligionDef
        {
            get => def;
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
