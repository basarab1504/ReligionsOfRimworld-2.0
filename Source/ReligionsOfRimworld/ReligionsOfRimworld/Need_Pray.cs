using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class Need_Pray : Need_Seeker
    {
        private int lastGainTick = -999;

        public Need_Pray(Pawn pawn)
        : base(pawn)
        {
            this.threshPercents = new List<float>
            {
                0.05f
            };
        }

        public void Gain(float amount)
        {
            if ((double)amount <= 0.0)
                return;
            amount = Mathf.Min(amount, 1f - this.CurLevel);
            this.curLevelInt += amount;
            this.lastGainTick = Find.TickManager.TicksGame;
        }

        private bool Gaining
        {
            get
            {
                return Find.TickManager.TicksGame < this.lastGainTick + 10;
            }
        }

        public override void NeedInterval()
        {
            if (this.Gaining)
                return;
            base.NeedInterval();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.lastGainTick, "lastGainPrayTick");
        }
    }
}
