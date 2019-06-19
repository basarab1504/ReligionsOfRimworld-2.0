using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    class CompProperties_ReligionComp : CompProperties
    {
        public CompProperties_ReligionComp()
        {
            this.compClass = typeof(CompReligion);
        }
    }
}
