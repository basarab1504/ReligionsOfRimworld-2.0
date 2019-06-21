using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class Piety : IExposable
    {
        protected Pawn pawn;
        protected PietyDef def;

        public Piety(Pawn pawn, PietyDef def)
        {
            this.pawn = pawn;
            this.def = def;
        }

        public PietyDef Def
        {
            get => def;
        }

        public PietyStage CurStage
        {
            get
            {
                return this.def.Stages[this.CurStageIndex];
            }
        }

        public abstract int CurStageIndex { get; }

        //public virtual string LabelCap
        //{
        //    get
        //    {
        //        if (!this.reason.NullOrEmpty())
        //            return string.Format(this.CurStage.label, (object)this.reason).CapitalizeFirst();
        //        return string.Format(this.CurStage.label, (object)CurStage.description).CapitalizeFirst();
        //    }
        //}

        //public virtual float Value
        //{
        //    get
        //    {
        //        return CurStage.multiplierValue;
        //    }
        //}

        //public virtual float InstantAdd
        //{
        //    get
        //    {
        //        return CurStage.addPiety;
        //    }
        //}

        public virtual void ExposeData()
        {
            Scribe_Defs.Look<PietyDef>(ref this.def, "def");
            //Scribe_Values.Look<string>(ref this.reason, "reason");
        }
    }
}
