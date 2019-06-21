using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class CompReligion : ThingComp
    {
        private Religion religion;
        private Pawn_ReligionTracker religionTracker;

        public Religion Religion
        {
            get => religion;
        }

        public Pawn_ReligionTracker ReligionTracker
        {
            get => religionTracker;
        }

        public void ChangeReligion(Religion religion)
        {
            this.religion = religion;
            religionTracker = new Pawn_ReligionTracker((Pawn)parent, religion);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (religion == null && parent is Pawn)
                PawnReligionGenerator.GenerateReligionToPawn((Pawn)parent);
            religionTracker.TrackerTick();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look<Religion>(ref this.religion, "religionOfPawn");
            Scribe_Deep.Look<Pawn_ReligionTracker>(ref this.religionTracker, "religionTracker", (Pawn)parent, religion);
        }
    }
}
