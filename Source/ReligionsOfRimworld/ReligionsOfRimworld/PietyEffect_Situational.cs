using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Piety_Situational : Piety
    {
        protected int curStageIndex = -1;
        protected string reason;

        public Piety_Situational(Pawn pawn, PietyDef def) : base(pawn, def)
        { }

        public bool Active => this.curStageIndex >= 0;
        public override int CurStageIndex => this.curStageIndex;

        public override string LabelCap
        {
            get
            {
                if (!this.reason.NullOrEmpty())
                    return string.Format(this.CurStage.Label, (object)this.reason).CapitalizeFirst();
                return string.Format(this.CurStage.Label, (object)CurStage.Description).CapitalizeFirst();
            }
        }

        public void RecalculateState()
        {
            PietyState state = this.CurrentStateInternal();
            if (state.ActiveFor(this.def))
            {
                this.curStageIndex = state.StageIndexFor(this.def);
                this.reason = state.Reason;
            }
            else
                this.curStageIndex = -1;
        }

        protected virtual PietyState CurrentStateInternal()
        {
            return this.def.Worker.CurrentState(this.pawn);
        }
    }
}
