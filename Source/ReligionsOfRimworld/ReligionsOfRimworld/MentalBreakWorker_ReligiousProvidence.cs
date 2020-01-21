using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class MentalBreakWorker_ReligiousBreakpoint : MentalBreakWorker
    {
        public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
        {
            Religion religion = ReligionManager.GetReligionManager().AllReligions.RandomElement();
            if(religion.GetSettings<ReligionSettings_MentalBreaks>(SettingsTagDefOf.MentalBreaksTag) != null)
            {
                ReligionSettings_MentalBreaks settings = religion.GetSettings<ReligionSettings_MentalBreaks>(SettingsTagDefOf.MentalBreaksTag);
                if (settings.MentalBreaks.Any(x => x == this.def))
                {
                    if (!pawn.GetReligionComponent().TryChangeReligion(religion))
                        return false;
                    return base.TryStart(pawn, reason, causedByMood);
                }
            }
            return false;
        }
    }
}
