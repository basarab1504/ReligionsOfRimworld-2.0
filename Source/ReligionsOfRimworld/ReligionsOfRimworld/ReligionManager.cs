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
        { }

        public override void FinalizeInit()
        {
            if (allReligions == null)
                allReligions = ReligionsBuffer.religions;
            else
            {
                foreach (Religion rel in ReligionsBuffer.religions)
                    if (!allReligions.Any(x => x.Def == rel.Def))
                        allReligions.Add(rel);
            }

            //CreateReligions();
            RecacheReligions();
        }

        public IEnumerable<Religion> AllReligions => allReligions;

        //public void Add(Religion religion)
        //{
        //    allReligions.Add(religion);
        //    RecacheReligions();
        //}

        //public void Remove(Religion religion)
        //{
        //    if (allReligions.Contains(religion))
        //        allReligions.Remove(religion);
        //    RecacheReligions();
        //}

        //private void CreateReligions()
        //{
        //    foreach (ReligionDef def in DefDatabase<ReligionDef>.AllDefs)
        //        if (!allReligions.Any(x => x.Def == def))
        //            allReligions.Add(new Religion(def));
        //}

        private void RecacheList()
        {
            foreach (Religion religion in allReligions)
                if (religion.Def == null)
                    foreach (Pawn pawn in Find.World.worldPawns.AllPawnsAlive)
                        pawn.GetReligionComponent().TryChangeReligion(allReligions.FirstOrDefault(x => x.Def == MiscDefOf.NonBeliever));

            RecacheReligions();
        }

        public void RecacheReligions()
        {
            foreach (Pawn pawn in Find.World.worldPawns.AllPawnsAlive)
                pawn.GetReligionComponent().Refresh();
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look<Religion>(ref this.allReligions, "allReligions", LookMode.Deep);
        }
    }
}
