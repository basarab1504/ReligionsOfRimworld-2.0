using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class IngredientPawn : IExposable
    {
        private ThingFilter fixedFilter;
        private ThingFilter filter;
        private bool partOfColony;
        private Pawn concretePawn;

        public IngredientPawn(ThingFilter fixedFilter)
        {
            this.fixedFilter = fixedFilter;
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                filter = new ThingFilter();
                filter.CopyAllowancesFrom(fixedFilter);
                partOfColony = true;
            }
        }

        public ThingFilter Filter { get => filter; set => filter = value; }
        public bool PartOfColony { get => partOfColony; set => partOfColony = value; }
        public Pawn ConcretePawn { get => concretePawn; set => concretePawn = value; }

        public void Reset()
        {
            filter.CopyAllowancesFrom(fixedFilter);
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

        }
    }
}
