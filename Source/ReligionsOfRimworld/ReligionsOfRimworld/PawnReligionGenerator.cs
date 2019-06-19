using RimWorld;
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
            CompReligion compReligion = pawn.GetReligionComponent();
            Religion religion = FindExtensions.GetReligionManager().AllReligions.RandomElement();
            SetReligionToPawn(compReligion, religion);
        }

        public static void SetReligionToPawn(CompReligion compReligion, Religion religion)
        {
            compReligion.ChangeReligion(religion);
        }
    }
}
