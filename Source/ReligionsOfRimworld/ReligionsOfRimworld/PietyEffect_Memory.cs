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
        //private int forcedStage;

        public Piety_Memory(Pawn pawn, PietyDef def) : base(pawn, def)
        { }

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
                return string.Format(this.CurStage.Label, (object)CurStage.Description).CapitalizeFirst();
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

        //public void SetForcedStage(int stageIndex)
        //{
        //    if (stageIndex >= def.Stages.Count())
        //        this.forcedStage = def.Stages.Count() - 1;
        //    else
        //        this.forcedStage = stageIndex;
        //}

        public override void ExposeData()
        {
            base.ExposeData();
            //Scribe_Values.Look<int>(ref this.forcedStage, "forcedStage");
            Scribe_Values.Look<int>(ref this.age, "age");
        }
    }
}
