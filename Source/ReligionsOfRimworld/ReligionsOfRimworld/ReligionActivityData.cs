using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityData
    {
        private Religion religion;
        private Pawn organizer;
        private Building_ReligiousBuildingFacility facility;
        private IEnumerable<LocalTargetInfo> relics;
        private IEnumerable<ActivityJobNode> activityJobs;
        private ReligionPropertyData organizerProperty;
        private ReligionPropertyData congregationProperty;
        private Bill_ReligionActivity bill; 

        public ReligionActivityData(Religion religion, Pawn organizer, Bill_ReligionActivity bill, IEnumerable<LocalTargetInfo> relics = null)
        {
            this.religion = religion;
            this.organizer = organizer;
            this.facility = (Building_ReligiousBuildingFacility)bill.billStack.billGiver;
            this.bill = bill;
            this.activityJobs = bill.Property.ActivityJobQueue.ActivityNodes;
            this.organizerProperty = bill.Property.Subject;
            this.congregationProperty = bill.Property.Witness;
            this.relics = relics;
        }

        public Religion Religion => religion;
        public Pawn Organizer => organizer;
        public Building_ReligiousBuildingFacility Facility => facility;
        public IEnumerable<LocalTargetInfo> Relics => relics;
        public IEnumerable<ActivityJobNode> ActivityJobs => activityJobs;
        public ReligionPropertyData OrganizerProperty => organizerProperty;
        public ReligionPropertyData СongregationProperty => congregationProperty;
        public Bill_ReligionActivity Bill => bill;

        //public IEnumerable<JobDef> OrganizerJobs
        //{
        //    get
        //    {
        //        foreach (ActivityJobNode node in activityJobs)
        //            yield return node.OrganizerJob;
        //    }
        //}

        //public IEnumerable<JobDef> CongregationJobs
        //{
        //    get
        //    {
        //        foreach (ActivityJobNode node in activityJobs)
        //            yield return node.CongregationJob;
        //    }
        //}
    }
}
