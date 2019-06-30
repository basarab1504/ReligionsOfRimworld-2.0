using RimWorld;
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

        public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_Religious
        {
            get
            {
                foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
                {
                    if (p.GetReligionComponent() != null)
                    {
                        yield return p;
                    }
                }
            }
        }
    }
}
