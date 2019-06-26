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
            if(opinionSettings.GetPropertyBySubject(otherPawnReligion.) == null)

            switch (p.GetReligionComponent().PietyTracker.Piety.CurCategory)
            {
                case PietyCategory.Empty:
                    return ThoughtState.ActiveAtStage(0);
                case PietyCategory.VeryLow:
                    return ThoughtState.ActiveAtStage(1);
                case PietyCategory.Low:
                    return ThoughtState.ActiveAtStage(2);
                case PietyCategory.Satisfied:
                    return ThoughtState.ActiveAtStage(3);
                case PietyCategory.High:
                    return ThoughtState.ActiveAtStage(4);
                case PietyCategory.Extreme:
                    return ThoughtState.ActiveAtStage(5);
                default:
                    return ThoughtState.Inactive;
            }
        }
    }
}
