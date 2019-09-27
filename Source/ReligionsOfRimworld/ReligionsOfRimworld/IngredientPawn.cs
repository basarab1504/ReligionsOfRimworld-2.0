using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class IngredientPawn : IExposable
    {
        private Pawn concretePawn;

        public Pawn ConcretePawn { get => concretePawn; set => concretePawn = value; }

        public void Reset()
        {
            concretePawn = null;
        }

        public void ValidateSettings()
        {
            if (concretePawn != null && concretePawn.Dead)
                concretePawn = null;
        }

        public void ExposeData()
        {
            Scribe_References.Look<Pawn>(ref this.concretePawn, "concretePawn");
        }
    }
}
