using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class JoiningCriteria_Trait : JoiningCriteria
    {
        public TraitDef criteria;

        public override string Reason => criteria.degreeDatas[0].label.CapitalizeFirst();

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.story != null && pawn.story.traits.allTraits.Any(x => x.def == criteria))
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Defs.Look<TraitDef>(ref criteria, "trait");
        }
    }
}
