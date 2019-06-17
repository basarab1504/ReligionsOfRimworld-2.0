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
        public List<Religion> AllReligions { get; }

        public ReligionManager(World world) : base(world)
        {
            AllReligions = new List<Religion>();
        }

        private void CreateReligions()
        {
            foreach (ReligionDef def in DefDatabase<ReligionDef>.AllDefs)
                AllReligions.Add(new Religion(def));
        }

        public override void FinalizeInit()
        {
            CreateReligions();
        }

        public override void ExposeData()
        {

        }
    }
}
