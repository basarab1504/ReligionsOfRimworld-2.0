using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ReligionsOfRimworld.DefaultThoughtWorkers
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

            ReligionSettings_Social opinionSettings = thisPawnReligion.OpinionSettings;
            if(opinionSettings == null)
                return (ThoughtState)false;

            ReligionProperty property = opinionSettings.GetPropertyBySubject(otherPawnReligion.GroupTag);
            if (property == null || property.SocialThought == null || property.SocialThought != this.def)
                return (ThoughtState)false;

            return ThoughtState.ActiveAtStage(p.GetReligionComponent().PietyTracker.Piety.CurCategoryInt);
        }
    }
}
