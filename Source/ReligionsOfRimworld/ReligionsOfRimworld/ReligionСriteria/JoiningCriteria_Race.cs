using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    class JoiningCriteria_Race : JoiningCriteria
    {
        public List<ThingDef> criteria = new List<ThingDef>();

        public override string Reason => GetReason();

        private string GetReason()
        {
            string reason = "";
            foreach (var race in criteria)
                reason += race.LabelCap + ", ";
            reason.Remove(reason.Length - 2);
            return reason;
        }

        protected override bool IsFound(Pawn pawn)
        {
            if (criteria.Any(x => x == pawn.def))
                return true;
            else
                return false;
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look<ThingDef>(ref criteria, "races", LookMode.Def);
        }
    }
}
