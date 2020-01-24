using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class JoiningCriteria_Gender : JoiningCriteria
    {
        Gender criteria;

        public override string Reason => criteria.GetLabel().CapitalizeFirst();

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.gender == criteria)
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<Gender>(ref criteria, "gender");
        }
    }
}
