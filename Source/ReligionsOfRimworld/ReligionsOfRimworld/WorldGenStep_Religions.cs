using RimWorld.Planet;
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
            foreach (ReligionDef def in DefDatabase<ReligionDef>.AllDefs)
                if (!ReligionsBuffer.religions.Any(x => x.Def == def))
                    ReligionsBuffer.religions.Add(new Religion(def));
        }

        public override void GenerateFromScribe(string seed)
        {
            foreach (ReligionDef def in DefDatabase<ReligionDef>.AllDefs)
                if (!ReligionsBuffer.religions.Any(x => x.Def == def))
                    ReligionsBuffer.religions.Add(new Religion(def));
        }
    }
}
