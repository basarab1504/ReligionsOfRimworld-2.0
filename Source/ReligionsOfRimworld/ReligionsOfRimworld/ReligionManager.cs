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
        public List<Religion> allReligions;

        public ReligionManager(World world) : base(world)
        {
            allReligions = new List<Religion>();
        }

        private void CreateReligions()
        {
            if(allReligions.NullOrEmpty())
            foreach (ReligionDef def in DefDatabase<ReligionDef>.AllDefs)
                allReligions.Add(new Religion(def));
        }

        public override void FinalizeInit()
        {
            CreateReligions();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<Religion>(ref allReligions, "allReligions", LookMode.Deep);
            //if (Scribe.mode != LoadSaveMode.LoadingVars)
            //    return;
            //for (int index = 0; index < this.activeConditions.Count; ++index)
            //    this.activeConditions[index].gameConditionManager = this;
        }
    }
}
