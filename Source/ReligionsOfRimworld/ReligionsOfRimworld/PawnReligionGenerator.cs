using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class PawnReligionGenerator
    {
        public static void GenerateReligionToPawn(Pawn pawn)
        {
            CompFaith faith = pawn.GetFaithComponent();
            Religion religion = FindExtensions.GetReligionManager().allReligions.RandomElement();
            faith.Religion = religion;
            //faith.compability.CalculateCompabilitites();
            //ReligionDef religion = faith.compability.MostSuitable();
            //faith.religionDef = religion;
            //if (religion.needPiety != null)
            //    faith.Piety = TryGetNeed<Need_Piety>(religion.needPiety, p);
        }
    }
}
