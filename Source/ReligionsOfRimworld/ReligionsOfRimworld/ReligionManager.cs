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
            allReligions = new List<Religion>();
        }

        public static ReligionManager GetReligionManager()
        {
            if (instance == null)
            {
                instance = new ReligionManager();
            }
            return instance;
        }

        public void Initialize()
        {
            if (allReligions == null)
                allReligions = new List<Religion>();

            if (allReligions.Count == 0)
                CreateReligions();
            else
                RecacheList();
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

        private void CreateReligions()
        {
            foreach (ReligionDef def in DefDatabase<ReligionDef>.AllDefs)
                allReligions.Add(new Religion(new ReligionConfiguration(def)));
        }

        private void RecacheList()
        {
            foreach (Religion religion in allReligions)
                if (!religion.TryToRecache())
                    foreach (Pawn pawn in Find.World.worldPawns.AllPawnsAlive)
                        pawn.GetReligionComponent().TryChangeReligion(allReligions.FirstOrDefault(x => x.Def == MiscDefOf.NonBeliever));

            RecacheReligions();
        }

        public void RecacheReligions()
        {
            foreach (Pawn pawn in Find.World.worldPawns.AllPawnsAlive)
                pawn.GetReligionComponent().Refresh();
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<Religion>(ref this.allReligions, "allReligions", LookMode.Deep);
            Initialize();
        }
    }
}
