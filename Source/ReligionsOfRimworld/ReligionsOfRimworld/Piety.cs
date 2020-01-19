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
        protected string reason;

        public Piety()
        { }

        public Piety(Pawn pawn, PietyDef def)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                this.pawn = pawn;
                this.def = def;
            }
        }

        public PietyDef Def
        {
            get => def;
        }

        public PietyStage CurStage
        {
            get
            {
                return this.def.Stages.ElementAt(this.CurStageIndex);
            }
        }

        public abstract int CurStageIndex { get; }

        public abstract string LabelCap
        {
            get;
        }

        public virtual void ExposeData()
        {
            Scribe_References.Look<Pawn>(ref this.pawn, "pawn");
            Scribe_Defs.Look<PietyDef>(ref this.def, "def");
        }
    }
}
