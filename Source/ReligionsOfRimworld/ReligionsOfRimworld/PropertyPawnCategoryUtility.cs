using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace ReligionsOfRimworld
{
    public static class PropertyPawnCategoryUtility
    {
        public static bool IsSubjectFromRightCategory(Pawn pawn, Pawn subject, PropertyPawnCategory pawnCategory)
        {
            switch(pawnCategory)
            {
                case PropertyPawnCategory.Everyone:
                    return true;
                case PropertyPawnCategory.Hostile:
                    return pawn.HostileTo(subject);
                case PropertyPawnCategory.Peaceful:
                    return !pawn.HostileTo(subject);
                case PropertyPawnCategory.SameFaction:
                    return (pawn.Faction == subject.Faction);
                case PropertyPawnCategory.SameReligionGroup:
                    return pawn.GetReligionComponent().Religion.GroupTag == subject.GetReligionComponent().Religion.GroupTag;
                default:
                    return false;
            }
        }
    }
}
