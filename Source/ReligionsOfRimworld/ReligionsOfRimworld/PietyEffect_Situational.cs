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

        public bool Active
        {
            get
            {
                return this.curStageIndex >= 0;
            }
        }

        public override int CurStageIndex
        {
            get
            {
                return this.curStageIndex;
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
