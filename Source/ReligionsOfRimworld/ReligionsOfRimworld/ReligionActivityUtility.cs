using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace ReligionsOfRimworld
{
    public static class ReligionActivityUtility
    {
        public static void StartActivity(Religion religion, Pawn organizer, ActivityTask task, IEnumerable<LocalTargetInfo> relics = null)
        {
            ReligionActivityData data = new ReligionActivityData(religion, organizer, task, relics);
            LordMaker.MakeNewLord(organizer.Faction, new LordJob_ReligionActivity(data), organizer.Map, new[] {organizer});
        }

        public static void TrySendStageEndedSignal(Pawn pawn)
        {
            ((LordJob_ReligionActivity)pawn.GetLord().LordJob).RecieveStageEndedSignal(pawn);
        }

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
