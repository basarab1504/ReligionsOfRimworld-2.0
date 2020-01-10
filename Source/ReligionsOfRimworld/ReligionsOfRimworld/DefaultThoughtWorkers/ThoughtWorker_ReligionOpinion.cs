using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ReligionsOfRimworld
{
    public class ThoughtWorker_ReligionOpinion : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
        {
            if (!p.RaceProps.Humanlike)
                return (ThoughtState)false;

            if (!RelationsUtility.PawnsKnowEachOther(p, other))
                return (ThoughtState)false;

            Religion thisPawnReligion = p.GetReligionComponent().Religion;
            Religion otherPawnReligion = other.GetReligionComponent().Religion;

            ReligionSettings_Social opinionSettings = thisPawnReligion.GetSettings<ReligionSettings_Social>(SettingsTagDefOf.OpinionTag);

            if(opinionSettings == null)
                return (ThoughtState)false;

            ReligionProperty property = opinionSettings.GetPropertyByObject(p, otherPawnReligion.Def, other);

            if (property == null || property.Witness == null || property.Witness.OpinionThought == null || property.Witness.OpinionThought != this.def)
                return (ThoughtState)false;

            return ThoughtState.ActiveAtStage(p.GetReligionComponent().PietyTracker.PietyNeed.CurCategoryInt, otherPawnReligion.Def.LabelCap);
        }
    }
}
