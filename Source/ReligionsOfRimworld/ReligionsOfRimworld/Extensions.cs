using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class FindExtensions
    {
        public static ReligionManager GetReligionManager()
        {
            return (ReligionManager)Find.World.components.FirstOrDefault(x => x is ReligionManager);
        }

        public static CompFaith GetFaithComponent(this Pawn pawn)
        {
            return pawn.TryGetComp<CompFaith>();
        }
    }
}
