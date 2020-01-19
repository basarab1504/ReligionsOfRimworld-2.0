using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Piety_Memory : Piety
    {
        private int age = 0;

        public Piety_Memory()
        {

        }

        public Piety_Memory(Pawn pawn, PietyDef def, string reason = "") : base(pawn, def)
        {
            this.reason = reason;
        }

        public override int CurStageIndex
        {
            get
            {
                return pawn.GetReligionComponent().PietyTracker.PietyNeed.CurCategoryInt;
            }
        }

        public override string LabelCap
        {
            get
            {
                return string.Format(this.CurStage.Label, reason, (object)CurStage.Description).CapitalizeFirst();
            }
        }

        public void Interval()
        {
            this.age += 150;
        }

        public int Age
        {
            get
            {
                return age;
            }
        }

        public bool ShouldDiscard()
        {
            return this.age > def.DurationTicks;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.age, "age");
            Scribe_Values.Look<string>(ref this.reason, "reason");
        }
    }
}
