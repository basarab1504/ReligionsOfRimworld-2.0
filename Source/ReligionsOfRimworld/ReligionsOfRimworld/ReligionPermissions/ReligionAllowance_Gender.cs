using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionPermission_Gender : ReligionPermission
    {
        Gender permission;

        public override string Reason => permission.GetLabel().CapitalizeFirst();

        protected override bool IsFound(Pawn pawn)
        {
            if (pawn.gender == permission)
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<Gender>(ref permission, "gender");
        }
    }
}
