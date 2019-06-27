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

        public void Add(Piety_Memory pietyMultiplier)
        {
            memoryPiety.Add(pietyMultiplier);
        }

        public void Remove(Piety_Memory pietyMultiplier)
        {
            memoryPiety.Remove(pietyMultiplier);
        }

        public float MultiplierValue
        {
            get
            {
                float v = 1f;
                v *= memoryPiety.TotalMemoryMultiplpierValue;
                v *= situationalPiety.TotalSitationalMultiplierValue;
                return v;
            }
        }

        public void GetPiety(List<Piety> outMultipliers)
        {
            foreach (Piety piety in memoryPiety.Piety)
                outMultipliers.Add(piety);
            foreach (Piety piety in situationalPiety.Piety)
                outMultipliers.Add(piety);
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
