using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class PietyHandler : IExposable
    {
        private PietyMemoryHandler memoryPiety;
        private PietySituationalHandler situationalPiety;

        public PietyHandler(Pawn pawn)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            memoryPiety = new PietyMemoryHandler();

            situationalPiety = new PietySituationalHandler(pawn);
        }

        public void PietyInterval()
        {
            memoryPiety.Interval();
            situationalPiety.Interval();
        }

        public void ExposeData()
        {
            Scribe_Deep.Look<PietyMemoryHandler>(ref this.memoryPiety, "memoryPiety");
        }
    }
}
