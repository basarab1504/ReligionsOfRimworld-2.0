using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace ReligionsOfRimworld
{
    public class Building_ReligiousBuildingMain : Building_ReligionBuilding
    {
        private Religion assignedReligion;
        private List<Building_ReligionBuilding> assignedBuildings;

        public Building_ReligiousBuildingMain()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                assignedBuildings = new List<Building_ReligionBuilding>();
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            LessonAutoActivator.TeachOpportunity(MiscDefOf.MainHolyBuilding, OpportunityType.GoodToKnow);         
            LessonAutoActivator.TeachOpportunity(MiscDefOf.RoRPray, OpportunityType.GoodToKnow);         
        }

        public bool TryAssignReligion(Religion religion)
        {
            assignedReligion = religion;
            UnassignAllBuildingsAndNotify();
            return true;
        }

        public bool TryUnassignReligion()
        {
            assignedReligion = null;
            UnassignAllBuildingsAndNotify();
            return true;
        }

        public override bool IsComplete
        {
            get
            {
                if (assignedReligion != null)
                    return true;
                return false;
            }
        }


        public override bool AvaliableToAssign
        {
            get
            {
                if (assignedReligion != null)
                    return true;
                return false;
            }
        }

        public override Religion AssignedReligion => assignedReligion;

        public override IEnumerable<Building_ReligionBuilding> AssignedBuildings => assignedBuildings;

        public override bool MayAssignBuilding(Building_ReligionBuilding religionBuilding)
        {
            if (religionBuilding is Building_ReligiousBuildingFacility 
                && DoReligionAllowsBuildings(religionBuilding)
                && !assignedBuildings.Contains(religionBuilding))
                return true;
            else
                return false;
        }

        private bool DoReligionAllowsBuildings(Building_ReligionBuilding building)
        {
            if (assignedReligion != null && assignedReligion.GetSettings<ReligionSettings_AllowedBuildings>(SettingsTagDefOf.AllowedBuildingsTag) != null)
                if (assignedReligion.GetSettings<ReligionSettings_AllowedBuildings>(SettingsTagDefOf.AllowedBuildingsTag).AllowedBuildings.Any(x => x == building.def))
                    return true;
            return false;
        }

        protected override void AssignBuilding(Building_ReligionBuilding religionBuilding)
        {
            assignedBuildings.Add(religionBuilding);
        }

        protected override void UnassignBuilding(Building_ReligionBuilding building)
        {
            assignedBuildings.Remove(building);
        }

        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (Faction == Faction.OfPlayer)
            {
                yield return new Command_Action
                {
                    defaultLabel = "ReligionInfo_AssignReligion".Translate(),
                    icon = ContentFinder<Texture2D>.Get("Things/Symbols/AssignReligion", true),
                    defaultDesc = "ReligionInfo_AssignReligionDesc".Translate(),
                    action = delegate
                    {
                        Find.WindowStack.Add(new Dialog_AssignReligion(this));
                    },
                    hotKey = KeyBindingDefOf.Misc4
                };

                foreach (Gizmo gizmo in base.GetGizmos())
                    yield return gizmo;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Religion>(ref this.assignedReligion, "assignedReligion");
            Scribe_Collections.Look<Building_ReligionBuilding>(ref this.assignedBuildings, "assignedBuildings", LookMode.Reference);
        }
    }
}
