using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class ReligionActivityUtility
    {
        public static bool PawnSatisfiesSkillRequirements(Pawn pawn, IEnumerable<SkillRequirement> skillRequirements)
        {
            return FirstSkillRequirementPawnDoesntSatisfy(pawn, skillRequirements) == null;
        }

        public static SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn, IEnumerable<SkillRequirement> skillRequirements)
        {
            if (skillRequirements == null)
                return (SkillRequirement)null;
            for (int index = 0; index < skillRequirements.Count(); ++index)
            {
                if (!skillRequirements.ElementAt(index).PawnSatisfies(pawn))
                    return skillRequirements.ElementAt(index);
            }
            return (SkillRequirement)null;
        }

        public static string MinSkillString(IEnumerable<SkillRequirement> skillRequirements)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            if (skillRequirements != null)
            {
                for (int index = 0; index < skillRequirements.Count(); ++index)
                {
                    SkillRequirement skillRequirement = skillRequirements.ElementAt(index);
                    stringBuilder.AppendLine("   " + skillRequirement.skill.skillLabel.CapitalizeFirst() + ": " + (object)skillRequirement.minLevel);
                    flag = true;
                }
            }
            if (!flag)
                stringBuilder.AppendLine("   (" + "NoneLower".Translate() + ")");
            return stringBuilder.ToString();
        }
    }
}
