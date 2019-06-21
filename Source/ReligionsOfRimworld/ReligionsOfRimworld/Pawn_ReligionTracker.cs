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

        public Pawn_ReligionTracker(Pawn pawn, Religion religion)
        {
            this.pawn = pawn;
            if (Scribe.mode == LoadSaveMode.Inactive)
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
            Scribe_Deep.Look<Pawn_PietyTracker>(ref this.pietyTracker, "pietyTracker", pawn, (Religion)null);
        }
    }
}
