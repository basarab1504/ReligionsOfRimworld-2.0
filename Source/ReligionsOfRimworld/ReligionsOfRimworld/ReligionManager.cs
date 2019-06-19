using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionManager : WorldComponent
    {
        private List<Religion> allReligions;

        public ReligionManager(World world) : base(world)
        {
            allReligions = new List<Religion>();
        }

        public List<Religion> AllReligions
        {
            get => allReligions;
        }

        public override void FinalizeInit()
        {
            if (allReligions.NullOrEmpty())
                CreateReligions();
            else
                RecacheReligions();
        }

        private void CreateReligions()
        {
            foreach (ReligionDef def in DefDatabase<ReligionDef>.AllDefs)
                allReligions.Add(new Religion(def));
        }

        private void RecacheReligions()
        {

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<Religion>(ref allReligions, "allReligions", LookMode.Deep);
        }
    }
}
