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
        private Pawn_ReligionCompability religionCompability;
        private Pawn_PietyTracker pietyTracker;

        public Religion Religion => religion;
        public Pawn_ReligionCompability ReligionCompability => religionCompability;
        public Pawn_PietyTracker PietyTracker => pietyTracker;

        public void ChangeReligion(Religion religion)
        {
            this.religion = religion;
            pietyTracker = new Pawn_PietyTracker((Pawn)parent, religion);
            religionCompability.RecalculateCompabilities();
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            religionCompability = new Pawn_ReligionCompability((Pawn)parent);
        }

        public void Refresh()
        {

        }

        public override void CompTick()
        {
            base.CompTick();
            if (religion == null && parent is Pawn)
                PawnReligionHandler.GenerateReligionToPawn((Pawn)parent);
            pietyTracker.TrackerTick();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look<Religion>(ref this.religion, "religionOfPawn");
            Scribe_Deep.Look<Pawn_PietyTracker>(ref this.pietyTracker, "pietyTracker", (Pawn)parent, religion);
        }
    }
}
