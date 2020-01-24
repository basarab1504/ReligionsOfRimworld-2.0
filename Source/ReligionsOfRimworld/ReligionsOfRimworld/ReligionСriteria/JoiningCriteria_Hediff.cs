using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class JoiningCriteria_Hediff : JoiningCriteria
    {
        public HediffDef criteria;

        public override string Reason => criteria.LabelCap;

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.health.hediffSet.hediffs.Any(x => x.def == criteria))
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Defs.Look<HediffDef>(ref criteria, "hediff");
        }
    }
}
