using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class PietyUtility
    {
        public static List<PietyDef> situationalPietyList = DefDatabase<PietyDef>.AllDefs.Where(x => x.IsSituational).ToList<PietyDef>();
    }
}
