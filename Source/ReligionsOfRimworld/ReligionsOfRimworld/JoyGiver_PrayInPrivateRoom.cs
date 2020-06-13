using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReligionsOfRimworld
{
    public class JoyGiver_PrayInPrivateRoom : JoyGiver_InPrivateRoom
    {

        public override bool CanBeGivenTo(Pawn pawn)
        {
            CompReligion rel = pawn.GetReligionComponent();
            if (rel == null || rel.Religion.GetSettings<ReligionSettings_Prayings>(SettingsTagDefOf.PrayingsTag) == null || !rel.ReligionRestrictions.MayPray)
                return false;
            return base.CanBeGivenTo(pawn);
        }
    }
}
