using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionPermission_Hediff : ReligionPermission
    {
        public HediffDef hediff;

        public override string Reason => hediff.LabelCap.CapitalizeFirst();

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.health.hediffSet.hediffs.Any(x => x.def == hediff))
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Defs.Look<HediffDef>(ref hediff, "hediff");
        }
    }
}
