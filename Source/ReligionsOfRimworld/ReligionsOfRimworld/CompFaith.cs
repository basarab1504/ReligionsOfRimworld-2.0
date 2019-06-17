using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class CompFaith : ThingComp
    {
        private Religion religion;
        private Pawn_PietyTracker pietyTracker;

        public Pawn_PietyTracker PietyTracker
        {
            get
            {
                if (this.pietyTracker == null)
                {
                    if (this.parent is Pawn parent)
                    {
                        this.pietyTracker = new Pawn_PietyTracker(parent);
                        //this.compability = new ReligionCompability(parent);
                        PawnReligionGenerator.GenerateReligionToPawn(parent);
                    }
                    else
                        Log.Error("CompFaith was added to " + this.parent.Label + " which cannot be cast to a Pawn.", false);
                }
                return this.pietyTracker;
            }
            set
            {
                this.pietyTracker = value;
            }
        }

        public Religion Religion
        {
            get => religion;
            set => religion = value;
        }

        public override void CompTick()
        {
            base.CompTick();
            PietyTracker.TrackerTick();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look<Religion>(ref this.religion, "religion");
            Scribe_Deep.Look<Pawn_PietyTracker>(ref this.pietyTracker, "faith", (object)(this.parent as Pawn));
            //Scribe_References.Look<Building_WorshipPlace>(ref this.ownership, "ownerOf");
            //Scribe_References.Look<Building_ReligionFacility>(ref this.templeOfPawn, "temple");
            //Scribe_Deep.Look<ReligionCompability>(ref this.compability, "compability", (object)(this.parent as Pawn));
            //Scribe_Values.Look<bool>(ref this.ableToConvert, "ableToConvert");
            //Scribe_Values.Look<bool>(ref this.religionActivity, "religionActivity");
        }
    }
}
