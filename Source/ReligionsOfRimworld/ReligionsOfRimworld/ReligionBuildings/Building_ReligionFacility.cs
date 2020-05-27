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
        private ActivityTaskSchedule taskSchedule;

        public ActivityTaskSchedule TaskSchedule => taskSchedule;

        public Building_ReligiousBuildingFacility()
        {
            this.taskSchedule = new ActivityTaskSchedule(this);
        }

        public IEnumerable<IntVec3> IngredientStackCells => GenAdj.CellsOccupiedBy((Thing)this);
        public bool CurrentlyUsableForBills() => this.InteractionCell.IsValid;
        public bool UsableForBillsAfterFueling() => this.CurrentlyUsableForBills();

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            LessonAutoActivator.TeachOpportunity(MiscDefOf.FacilityHolyBuilding, OpportunityType.GoodToKnow);
        }

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
            taskSchedule.Clear();
        }

        public override bool IsComplete
        {
            get
            {
                if (parentBuilding != null)
                    return parentBuilding.IsComplete;
                return false;
            }
        }

        public override bool AvaliableToAssign
        {
            get
            {
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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Building_ReligionBuilding>(ref this.parentBuilding, "parentBuilding");
            Scribe_Deep.Look<ActivityTaskSchedule>(ref this.taskSchedule, "taskSchedule", this);
        }

        public void ValidateSettings()
        {
            taskSchedule.ValidateSettings();
        }
    }
}
