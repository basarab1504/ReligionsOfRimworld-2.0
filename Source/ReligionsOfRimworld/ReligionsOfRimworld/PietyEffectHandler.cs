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
            memoryPiety = new PietyMemoryHandler();
            situationalPiety = new PietySituationalHandler(pawn);
        }

        public float TotalOffset()
        {
            float total = 0f;
            foreach (Piety piety in Piety)
                total += piety.CurStage.PietyOffset;
            return total;
        }

        public IEnumerable<Piety> Piety
        {
            get
            {
                foreach (Piety piety in memoryPiety.Piety)
                    yield return piety;
                foreach (Piety piety in situationalPiety.Piety)
                    yield return piety;
            }
        }

        public void Add(Piety_Memory piety)
        {
            memoryPiety.Add(piety);
        }

        public void Remove(Piety_Memory piety)
        {
            memoryPiety.Remove(piety);
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
