using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class PietyMemoryHandler : IExposable
    {
        private List<Piety_Memory> pietyEffects;

        public PietyMemoryHandler()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                pietyEffects = new List<Piety_Memory>();
        }

        public IEnumerable<Piety_Memory> Piety => pietyEffects;

        public void Add(Piety_Memory piety)
        {
            pietyEffects.Add(piety);
        }

        public void Remove(Piety_Memory piety)
        {
            pietyEffects.Remove(piety);
        }

        public void Interval()
        {
            foreach(Piety_Memory pietyEffect in pietyEffects)
                pietyEffect.Interval();
            RemoveExpired();
        }

        public void RemoveExpired()
        {
            for (int index = this.pietyEffects.Count - 1; index >= 0; --index)
            {
                if (pietyEffects[index].ShouldDiscard())
                    pietyEffects.Remove(pietyEffects[index]);
            }
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<Piety_Memory>(ref this.pietyEffects, "pietyMemories", LookMode.Deep, (Pawn)null, (PietyDef)null);
        }
    }
}
