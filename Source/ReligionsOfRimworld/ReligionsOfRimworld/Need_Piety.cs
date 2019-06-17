using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace ReligionsOfRimworld
{
    public class Need_Piety : Need_Seeker
    {
        //public PietyMultiplierHandler multiplier;

        //public float MultiplierValue
        //{
        //    get
        //    {
        //        return multiplier.Value;
        //    }
        //}

        //public void GetMultipliers(List<PietyMultiplier> outList)
        //{
        //    foreach (PietyMultiplier m in multiplier.instant.Multipliers)
        //        outList.Add(m);
        //    foreach (PietyMultiplier m in multiplier.situational.Multipliers)
        //        outList.Add(m);
        //}

        public Need_Piety(Pawn pawn)
        : base(pawn)
        {
            //multiplier = new PietyMultiplierHandler(pawn);
            this.threshPercents = new List<float>
            {
                0.15f,
                0.3f,
                0.7f,
                0.85f
            };
        }

        private int lastGainTick = -999;

        private bool GainingPiety
        {
            get
            {
                return Find.TickManager.TicksGame < this.lastGainTick + 10;
            }
        }

        //public PietyCategory CurCategory
        //{
        //    get
        //    {
        //        if ((double)this.CurLevel < 0.00999999977648258)
        //            return PietyCategory.Empty;
        //        if ((double)this.CurLevel < 0.150000005960464)
        //            return PietyCategory.VeryLow;
        //        if ((double)this.CurLevel < 0.300000011920929)
        //            return PietyCategory.Low;
        //        if ((double)this.CurLevel < 0.699999988079071)
        //            return PietyCategory.Satisfied;
        //        return (double)this.CurLevel < 0.850000023841858 ? PietyCategory.High : PietyCategory.Extreme;
        //    }
        //}

        public void Gain(float amount)
        {
            amount = Mathf.Min(amount, 1f - this.CurLevel);
            this.curLevelInt += amount;
            this.lastGainTick = Find.TickManager.TicksGame;
        }

        public override void NeedInterval()
        {
            if (this.IsFrozen || this.GainingPiety)
                return;
            //multiplier.MultiplierInterval();
            this.CurLevel -= this.def.seekerFallPerHour * 0.06f/* * MultiplierValue*/;
        }

        //public override void ExposeData()
        //{
        //    base.ExposeData();
        //    Scribe_Deep.Look<PietyMultiplierHandler>(ref this.multiplier, "multiplier", pawn);
        //}
    }
}
