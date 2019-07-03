using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ReligionsOfRimworld
{
    public static class ReligiousBuildingAssignerUtility
    {
        public static void SelectParent()
        {
            DebugTools.curTool = new DebugTool("ReligiousBuilgingAssigner_SelectABuilding".Translate(), (Action)(() =>
            {
                foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
                    if (thing is Building_ReligionBuilding parent)
                    {
                        if (parent.AvaliableToAssign)
                            SelectChild(parent);
                        else
                            Messages.Message("ReligiousBuilgingAssigner_BuildingIsNotCompleteToAssign".Translate(), MessageTypeDefOf.NeutralEvent);
                    }
            }));
        }

        private static void SelectChild(Building_ReligionBuilding parent)
        {
            DebugTools.curTool = new DebugTool("ReligiousBuilgingAssigner_SelectABuilding".Translate(), (Action)(() =>
            {
                foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
                    if (thing is Building_ReligionBuilding child)
                    {
                        if (parent == child)
                        {
                            TryToUnassignAllBuildings(parent);
                            SelectParent();
                            return;
                        }

                        if (child.AvaliableToAssign)
                        {
                            TryToAssignBuilding(parent, child);
                            SelectParent();
                            return;
                        }
                        else
                            Messages.Message("ReligiousBuilgingAssigner_BuildingIsNotCompleteToAssign".Translate(), MessageTypeDefOf.NeutralEvent);

                        DebugTools.curTool = null;
                    }
            }), (Action)(() =>
            {
                foreach (Building_ReligionBuilding assigned in parent.AssignedBuildings)
                    Widgets.DrawLine(parent.Position.ToUIPosition(), assigned.Position.ToUIPosition(), Color.green, 2f);
                if (UI.MouseCell().InBounds(Find.CurrentMap))
                    Widgets.DrawLine(parent.Position.ToUIPosition(), UI.MouseCell().ToUIPosition(), Color.white, 2f);
            }));
        }

        private static void TryToUnassignAllBuildings(Building_ReligionBuilding parent)
        {
            if (parent.AssignedBuildings.Count() != 0)
            {
                parent.UnassignAllBuildings();
                Messages.Message("ReligiousBuilgingAssigner_BuildingWasUnassigned".Translate(), MessageTypeDefOf.NeutralEvent);
            }
        }

        private static void TryToAssignBuilding(Building_ReligionBuilding parent, Building_ReligionBuilding child)
        {
            if (parent.MayAssignBuilding(child) && child.MayAssignBuilding(parent))
            {
                parent.SendAssigningRequest(child, AssigningRequestType.Add);
                SoundDefOf.Click.PlayOneShotOnCamera((Map)null);
            }
            else
                Messages.Message("ReligiousBuilgingAssigner_AssigningIsNotAllowed".Translate(), MessageTypeDefOf.NeutralEvent);
        }
    }
}
