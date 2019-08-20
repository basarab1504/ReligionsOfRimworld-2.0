using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class ActivityUtility
    {
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

		public static IEnumerable<ActivityTask> GlobalActivityTasks()
		{
			foreach (Building_ReligiousBuildingFacility giver in GlobalActivityGivers())
            {
                foreach (ActivityTask task in giver.TaskSchedule.AllTasks())
                    yield return task;
			}
		}

        public static void Notify_ColonistUnavailable(Pawn pawn)
        {
            try
            {
                foreach (ActivityTask current in GlobalActivityTasks())
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
