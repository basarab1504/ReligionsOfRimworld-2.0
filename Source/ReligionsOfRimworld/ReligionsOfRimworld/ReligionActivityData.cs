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
        private ActivityTask task;
        private List<ActivityJobNode> activityJobNodes;

        public ReligionActivityData(Religion religion, Pawn organizer, ActivityTask task, IEnumerable<LocalTargetInfo> relics = null)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                this.relics = new List<LocalTargetInfo>();
                if (relics != null)
                    this.relics.AddRange(relics);
                this.activityJobNodes = new List<ActivityJobNode>();
                this.activityJobNodes.Add(new ActivityJobNode(MiscDefOf.ReligionActivityPreparations, null));
                this.activityJobNodes.AddRange(task.ActivityTaskDef.ActivityQueue.ActivityNodes);
            }
        
            this.religion = religion;
            this.organizer = organizer;
            this.task = task;
        }

        public Religion Religion => religion;
        public Pawn Organizer => organizer;
        public Building_ReligiousBuildingFacility Facility => (Building_ReligiousBuildingFacility)task.ParentFacility;
        public IEnumerable<LocalTargetInfo> Relics => relics;
        public IEnumerable<ActivityJobNode> ActivityJobs => activityJobNodes;
        public ReligionPropertyData OrganizerProperty => task.Property.Subject;
        public ReligionPropertyData СongregationProperty => task.Property.Witness;
        public ActivityTask Task => task;

        public void ExposeData()
        {
            Scribe_References.Look<Religion>(ref this.religion, "religion");
            Scribe_References.Look<Pawn>(ref this.organizer, "organizer");
            Scribe_Collections.Look<LocalTargetInfo>(ref this.relics, "relics", LookMode.LocalTargetInfo);
            Scribe_References.Look<ActivityTask>(ref this.task, "task");
            Scribe_Collections.Look<ActivityJobNode>(ref activityJobNodes, "activityJobNodes", LookMode.Deep);
        }
    }
}
