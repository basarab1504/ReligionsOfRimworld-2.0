using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class ReligionExtensions
    {
        public static CompReligion GetReligionComponent(this Pawn pawn)
        {
            return pawn.TryGetComp<CompReligion>();
        }

        //public static bool IsReligious(this Pawn pawn)
        //{
        //    return GetReligionComponent(pawn) != null;
        //}
    }
}
