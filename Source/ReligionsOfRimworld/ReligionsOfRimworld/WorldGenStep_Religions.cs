using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class WorldGenStep_Religions : WorldGenStep
    {
        public override int SeedPart
        {
            get
            {
                return 777998381;
            }
        }

        public override void GenerateFresh(string seed)
        {
            ReligionManager.GetReligionManager().CreateReligions();
        }
    }
}
