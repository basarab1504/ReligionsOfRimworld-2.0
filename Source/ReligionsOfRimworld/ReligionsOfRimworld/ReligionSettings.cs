using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class ReligionSettings : IExposable, ILoadReferenceable
    {
        private int loadID;    

        public ReligionSettings()
        {           
        }

        public ReligionSettings(int loadID)
        {
            loadID = Find.UniqueIDsManager.GetNextThingID();
        }

        public abstract IEnumerable<ReligionInfoEntry> GetInfoEntries();

        public string GetUniqueLoadID()
        {
            return "ReligionSettings_" + (object)this.loadID;
        }

        public virtual void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.loadID, "loadID");
        }
    }
}
