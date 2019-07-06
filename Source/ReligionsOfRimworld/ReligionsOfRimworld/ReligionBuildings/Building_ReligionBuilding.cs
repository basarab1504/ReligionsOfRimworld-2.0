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

        public abstract bool AvaliableToAssign { get; }

        public abstract IEnumerable<Building_ReligionBuilding> AssignedBuildings { get; }

        public abstract bool MayAssignBuilding(Building_ReligionBuilding building);

        protected abstract void AssignBuilding(Building_ReligionBuilding building);

        protected abstract void UnassignBuilding(Building_ReligionBuilding building);

        public void UnassignAllBuildings()
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
                        if (toBuilding.GetAssigningRequest(this, requestType))
                            AssignBuilding(toBuilding);
                        return;
                    }
                case AssigningRequestType.Delete:
                    {
                        if (toBuilding.GetAssigningRequest(this, requestType))
                            UnassignBuilding(toBuilding);
                        return;
                    }
            }
        }

        private bool GetAssigningRequest(Building_ReligionBuilding fromBuilding, AssigningRequestType requestType)
        {
            switch (requestType)
            {
                case AssigningRequestType.Add:
                    {
                        if (MayAssignBuilding(fromBuilding) && fromBuilding.MayAssignBuilding(this))
                        {
                            AssignBuilding(fromBuilding);
                            return true;
                        }
                        return false;
                    }
                case AssigningRequestType.Delete:
                    {
                        UnassignBuilding(fromBuilding);
                        return true;
                    }
                default:
                    return false;
            }
        }
    }
}
