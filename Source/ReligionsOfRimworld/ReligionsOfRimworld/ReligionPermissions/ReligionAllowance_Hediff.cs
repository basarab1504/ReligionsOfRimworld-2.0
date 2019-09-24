using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionPermission_Hediff : JoiningCriteria
    {
        public HediffDef permission;

        public override string Reason => permission.LabelCap;

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.health.hediffSet.hediffs.Any(x => x.def == permission))
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Defs.Look<HediffDef>(ref permission, "hediff");
        }
    }
}
