using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class Building_ReligiousBuildingFacility : Building_ReligionBuilding
    {
        private Building_ReligionBuilding parentBuilding;
        private ReligionActivityTaskList taskStack;

        public Building_ReligiousBuildingFacility()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                taskStack = new ReligionActivityTaskList();
        }

        public ReligionActivityTaskList TaskStack => taskStack;

        public override Religion AssignedReligion
        {
            get
            {
                if (parentBuilding != null)
                    return parentBuilding.AssignedReligion;
                return null;
            }
        }

        public override void Notify_BuildingAssigningChanged()
        {
            taskStack.Clear();
        }

        public override bool AvaliableToAssign
        {
            get
            {
                if (parentBuilding != null)
                    return parentBuilding.AvaliableToAssign;
                return true;
            }
        }

        public override IEnumerable<Building_ReligionBuilding> AssignedBuildings
        {
            get
            {
                if (parentBuilding != null)
                    yield return parentBuilding;
            }
        }

        public override bool MayAssignBuilding(Building_ReligionBuilding religionBuilding)
        {
            if (religionBuilding is Building_ReligiousBuildingMain && parentBuilding == null)
                return true;
            else
                return false;
        }

        protected override void AssignBuilding(Building_ReligionBuilding religionBuilding)
        {
            parentBuilding = religionBuilding;
        }

        protected override void UnassignBuilding(Building_ReligionBuilding building)
        {
            parentBuilding = null;
        }

        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
                yield return gizmo;

            if (Faction == Faction.OfPlayer)
            {
                yield return new Command_Action
                {
                    defaultLabel = "ReligiousBuilgingAssigner_AssignBuildings".Translate(),
                    icon = ContentFinder<Texture2D>.Get("Things/Symbols/AssignReligion", true),
                    defaultDesc = "ReligiousBuilgingAssigner_AssignBuildingsDesc".Translate(),
                    action = delegate
                    {
                        ReligiousBuildingAssignerUtility.SelectParent();
                    },
                    hotKey = KeyBindingDefOf.Misc4
                };
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Building_ReligionBuilding>(ref this.parentBuilding, "parentBuilding");
            Scribe_Deep.Look<ReligionActivityTaskList>(ref this.taskStack, "taskStack");
        }
    }
}
