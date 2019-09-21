using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionPermission_Trait : JoiningCriteria
    {
        public TraitDef permission;

        public override string Reason => permission.degreeDatas[0].label.CapitalizeFirst();

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.story.traits.allTraits.Any(x => x.def == permission))
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Defs.Look<TraitDef>(ref permission, "trait");
        }
    }
}
