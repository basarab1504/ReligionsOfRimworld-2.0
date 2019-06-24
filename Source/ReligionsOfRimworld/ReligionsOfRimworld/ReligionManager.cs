using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionManager : IExposable
    {
        private static ReligionManager instance;
        private List<Religion> allReligions;

        public ReligionManager()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                Log.Message("oh siu");
                allReligions = new List<Religion>();
            }
        }

        public static ReligionManager GetReligionManager()
        {
            if (instance == null)
                instance = new ReligionManager();
            return instance;
        }

        public IEnumerable<Religion> AllReligions => allReligions;

        public void Add(Religion religion)
        {
            allReligions.Add(religion);
            RecacheReligions();
        }

        public void Remove(Religion religion)
        {
            if (allReligions.Contains(religion))
                allReligions.Remove(religion);
            RecacheReligions();
        }

        public void CreateReligions()
        {
            foreach (ReligionDef def in DefDatabase<ReligionDef>.AllDefs)
                allReligions.Add(MakeReligionFromDefUtility.MakeReligionFromDef(def));
        }

        public void RecacheReligions()
        {

        }

        public void ExposeData()
        {
            Scribe_Collections.Look<Religion>(ref this.allReligions, "allReligions", LookMode.Deep, (ReligionConfiguration)null);
        }
    }
}
