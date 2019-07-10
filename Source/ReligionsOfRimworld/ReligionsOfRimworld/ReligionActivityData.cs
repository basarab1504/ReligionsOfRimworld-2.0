using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityData : IExposable
    {
        private Religion religion;
        private Pawn organizer;
        private List<LocalTargetInfo> relics;
        private Bill_ReligionActivity bill; 

        public ReligionActivityData(Religion religion, Pawn organizer, Bill_ReligionActivity bill, IEnumerable<LocalTargetInfo> relics = null)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                this.relics = new List<LocalTargetInfo>();

            this.religion = religion;
            this.organizer = organizer;
            this.bill = bill;
            this.relics.AddRange(relics);
        }

        public Religion Religion => religion;
        public Pawn Organizer => organizer;
        public Building_ReligiousBuildingFacility Facility => (Building_ReligiousBuildingFacility)bill.billStack.billGiver;
        public IEnumerable<LocalTargetInfo> Relics => relics;
        public IEnumerable<ActivityJobNode> ActivityJobs => bill.Property.ActivityJobQueue.ActivityNodes;
        public ReligionPropertyData OrganizerProperty => bill.Property.Witness;
        public ReligionPropertyData СongregationProperty => bill.Property.Subject;
        public Bill_ReligionActivity Bill => bill;

        public void ExposeData()
        {
            Scribe_References.Look<Religion>(ref this.religion, "religion");
            Scribe_References.Look<Pawn>(ref this.organizer, "organizer");
            Scribe_Collections.Look<LocalTargetInfo>(ref this.relics, "relicsTargets", LookMode.LocalTargetInfo);
            Scribe_References.Look<Bill_ReligionActivity>(ref this.bill, "bill");
        }

    }
}
