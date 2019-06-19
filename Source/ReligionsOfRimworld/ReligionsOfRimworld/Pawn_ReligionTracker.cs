using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Pawn_ReligionTracker : IExposable
    {
        private Pawn pawn;
        private Pawn_PietyTracker pietyTracker;

        public Pawn_ReligionTracker(Pawn pawn) //Do not use! Used on game loading
        {
            this.pawn = pawn;
        }

        public Pawn_ReligionTracker(Pawn pawn, Religion religion)
        {
            pietyTracker = new Pawn_PietyTracker(pawn, religion);
        }

        public Pawn_PietyTracker PietyTracker
        {
            get => pietyTracker;
        }

        public void TrackerTick()
        {
            pietyTracker.TrackerTick();
        }

        public void ExposeData()
        {
            Scribe_Deep.Look<Pawn_PietyTracker>(ref this.pietyTracker, "pietyTracker", pawn);
        }
    }
}
