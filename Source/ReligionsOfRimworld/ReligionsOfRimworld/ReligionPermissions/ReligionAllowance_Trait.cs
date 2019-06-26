using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionPermission_Trait : ReligionPermission
    {
        public TraitDef trait;

        public override string Reason => trait.degreeDatas[0].label.CapitalizeFirst();

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.story.traits.allTraits.Any(x => x.def == trait))
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Defs.Look<TraitDef>(ref trait, "trait");
        }
    }
}
