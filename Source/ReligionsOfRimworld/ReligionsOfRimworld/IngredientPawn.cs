using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class IngredientPawn : IExposable
    {
        private bool partOfColony;
        private Pawn concretePawn;

        public IngredientPawn()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                partOfColony = true;
            }
        }

        public bool PartOfColony { get => partOfColony; set => partOfColony = value; }
        public Pawn ConcretePawn { get => concretePawn; set => concretePawn = value; }

        public void Reset()
        {
            partOfColony = true;
            concretePawn = null;
        }

        public void Notify_PawnUnavaliable(Pawn pawn)
        {
            if (pawn == concretePawn)
                concretePawn = null;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.partOfColony, "partOfColony");
            Scribe_References.Look<Pawn>(ref this.concretePawn, "concretePawn");
        }
    }
}
