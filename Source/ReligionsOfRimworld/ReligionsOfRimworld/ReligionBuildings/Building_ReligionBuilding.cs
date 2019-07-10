using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class Building_ReligionBuilding : Building
    {
        public abstract Religion AssignedReligion { get; }

        public virtual void Notify_BuildingAssigningChanged() { }

        public abstract bool AvaliableToAssign { get; }

        public abstract IEnumerable<Building_ReligionBuilding> AssignedBuildings { get; }

        public abstract bool MayAssignBuilding(Building_ReligionBuilding building);

        protected abstract void AssignBuilding(Building_ReligionBuilding building);

        protected abstract void UnassignBuilding(Building_ReligionBuilding building);

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            UnassignAllBuildingsAndNotify();
            base.Destroy(mode);
        }

        public void UnassignAllBuildingsAndNotify()
        {
            foreach (Building_ReligionBuilding building in AssignedBuildings.ToList())
                SendAssigningRequest(building, AssigningRequestType.Delete);
        }

        public void SendAssigningRequest(Building_ReligionBuilding toBuilding, AssigningRequestType requestType)
        {
            switch (requestType)
            {
                case AssigningRequestType.Add:
                    {
                        if (toBuilding.AssigningRequestAccepted(this, requestType))
                        {
                            AssignBuilding(toBuilding);
                            Notify_BuildingAssigningChanged();
                        }
                        return;
                    }
                case AssigningRequestType.Delete:
                    {
                        if (toBuilding.AssigningRequestAccepted(this, requestType))
                        {
                            UnassignBuilding(toBuilding);
                            Notify_BuildingAssigningChanged();
                        }
                        return;
                    }
            }
        }

        private bool AssigningRequestAccepted(Building_ReligionBuilding fromBuilding, AssigningRequestType requestType)
        {
            switch (requestType)
            {
                case AssigningRequestType.Add:
                    {
                        if (MayAssignBuilding(fromBuilding) && fromBuilding.MayAssignBuilding(this))
                        {
                            AssignBuilding(fromBuilding);
                            Notify_BuildingAssigningChanged();
                            return true;
                        }
                        return false;
                    }
                case AssigningRequestType.Delete:
                    {
                        UnassignBuilding(fromBuilding);
                        Notify_BuildingAssigningChanged();
                        return true;
                    }
                default:
                    return false;
            }
        }
    }
}
