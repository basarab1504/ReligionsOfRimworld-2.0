using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Pawn_PietyTracker : IExposable
    {
        private Pawn pawn;
        private Need_Piety piety;

        public Pawn_PietyTracker(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public Need_Piety Piety
        {
            get => piety;
        }

        public void TrackerTick()
        {
            if (!this.pawn.IsHashIntervalTick(150))
                return;
            if (piety == null)
                return;
            piety.NeedInterval();
        }

        public void ExposeData()
        {
            Scribe_Deep.Look<Need_Piety>(ref this.piety, "piety", (object)(this.pawn));
        }
    }
}
