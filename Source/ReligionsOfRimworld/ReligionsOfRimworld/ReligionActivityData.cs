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
        private ActivityTask bill;
        private List<ActivityJobNode> activityJobNodes;

        public ReligionActivityData(Religion religion, Pawn organizer, ActivityTask bill, IEnumerable<LocalTargetInfo> relics = null)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                this.relics = new List<LocalTargetInfo>();
                this.relics.AddRange(relics);
                this.activityJobNodes = new List<ActivityJobNode>();
                this.activityJobNodes.Add(new ActivityJobNode(MiscDefOf.ReligionActivityPreparations, null));
                this.activityJobNodes.AddRange(bill.Property.ActivityJobQueue.ActivityNodes);
            }

            this.religion = religion;
            this.organizer = organizer;
            this.bill = bill;
        }

        public Religion Religion => religion;
        public Pawn Organizer => organizer;
        public Building_ReligiousBuildingFacility Facility => (Building_ReligiousBuildingFacility)bill.billStack.billGiver;
        public IEnumerable<LocalTargetInfo> Relics => relics;
        public IEnumerable<ActivityJobNode> ActivityJobs => activityJobNodes;
        public ReligionPropertyData OrganizerProperty => bill.Property.Witness;
        public ReligionPropertyData СongregationProperty => bill.Property.Subject;
        public ActivityTask Bill => bill;

        public void ExposeData()
        {
            Scribe_References.Look<Religion>(ref this.religion, "religion");
            Scribe_References.Look<Pawn>(ref this.organizer, "organizer");
            Scribe_Collections.Look<LocalTargetInfo>(ref this.relics, "relicsTargets", LookMode.LocalTargetInfo);
            Scribe_References.Look<ActivityTask>(ref this.bill, "bill");
            Scribe_Collections.Look<ActivityJobNode>(ref activityJobNodes, "activityJobNodes", LookMode.Deep);
        }
    }
}
