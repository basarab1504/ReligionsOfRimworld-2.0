using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    class JoiningCriteria_RaceGroupName : JoiningCriteria
    {
        public string criteria;

        public override string Reason => criteria;

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.def.defName.Contains(criteria))
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<string>(ref criteria, "races");
        }
    }
}