using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace ReligionsOfRimworld
{
    public static class ActivityUtility
    {
        public static void StartActivity(Religion religion, Pawn organizer, ActivityTask task, IEnumerable<LocalTargetInfo> relics = null)
        {
            ReligionActivityData data = new ReligionActivityData(religion, organizer, task, relics);
            LordMaker.MakeNewLord(organizer.Faction, new LordJob_ReligionActivity(data), organizer.Map, new[] { organizer });
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

        public static IEnumerable<Building_ReligiousBuildingFacility> GlobalActivityGivers()
		{
			foreach (Map map in Find.Maps)
			{
				foreach (Thing thing in map.listerThings.AllThings.OfType<Building_ReligiousBuildingFacility>())
				{
                    Building_ReligiousBuildingFacility giver = thing as Building_ReligiousBuildingFacility;
					if (giver == null)
					{
						Log.ErrorOnce("Found non-bill-giver tagged as PotentialActivityGiver", 13389774, false);
					}
					else
					{
						yield return giver;
					}
				}
				foreach (Thing thing2 in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)))
				{
                    Building_ReligiousBuildingFacility giver2 = thing2.GetInnerIfMinified() as Building_ReligiousBuildingFacility;
					if (giver2 != null)
					{
						yield return giver2;
					}
				}
			}
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				foreach (Thing thing3 in caravan.AllThings)
				{
                    Building_ReligiousBuildingFacility giver3 = thing3.GetInnerIfMinified() as Building_ReligiousBuildingFacility;
					if (giver3 != null)
					{
						yield return giver3;
					}
				}
			}
		}

		//public static IEnumerable<ActivityTask> GlobalActivityTasks()
		//{
		//	foreach (Building_ReligiousBuildingFacility giver in GlobalActivityGivers())
  //          {
  //              foreach (ActivityTask task in giver.TaskSchedule.AllTasks())
  //                  yield return task;
		//	}
		//}

        public static void Notify_ColonistUnavailable(Pawn pawn)
        {
            try
            {
                foreach (Building_ReligiousBuildingFacility current in GlobalActivityGivers())
                {
                    current.ValidateSettings();
                }
            }
            catch (Exception arg)
            {
                Log.Error("Could not notify tasks: " + arg, false);
            }
        }
    }
}
