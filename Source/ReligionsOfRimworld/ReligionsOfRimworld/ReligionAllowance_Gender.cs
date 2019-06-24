using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionPermission_Gender : ReligionPermission
    {
        Gender gender;

        public override string Reason => gender.GetLabel().CapitalizeFirst();

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.gender == gender)
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<Gender>(ref gender, "gender");
        }
    }
}
