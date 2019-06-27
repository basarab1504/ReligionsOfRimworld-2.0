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
        private PietyHandler pietyEffectHandler;

        public float MultiplierValue
        {
            get
            {
                return pietyEffectHandler.MultiplierValue;
            }
        }

        public void GetPiety(List<Piety> outList)
        {
            pietyEffectHandler.GetPiety(outList);
        }

        public Need_Piety(Pawn pawn)
        : base(pawn)
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                pietyEffectHandler = new PietyHandler(pawn);
            this.threshPercents = new List<float>
            {
                0.15f,
                0.3f,
                0.7f,
                0.85f
            };
        }

        public void Add(Piety_Memory pietyMultiplier)
        {
            pietyEffectHandler.Add(pietyMultiplier);
        }

        public void Remove(Piety_Memory pietyMultiplier)
        {
            pietyEffectHandler.Remove(pietyMultiplier);
        }

        private int lastGainTick = -999;

        private bool GainingPiety
        {
            get
            {
                return Find.TickManager.TicksGame < this.lastGainTick + 10;
            }
        }

        public int CurCategoryInt
        {
            get
            {
                if ((double)this.CurLevel < 0.00999999977648258)
                    return 0;
                if ((double)this.CurLevel < 0.150000005960464)
                    return 1;
                if ((double)this.CurLevel < 0.300000011920929)
                    return 2;
                if ((double)this.CurLevel < 0.699999988079071)
                    return 3;
                return (double)this.CurLevel < 0.850000023841858 ? 4 : 5;
            }
        }

        public PietyCategory CurCategory
        {
            get
            {
                if ((double)this.CurLevel < 0.00999999977648258)
                    return PietyCategory.Empty;
                if ((double)this.CurLevel < 0.150000005960464)
                    return PietyCategory.VeryLow;
                if ((double)this.CurLevel < 0.300000011920929)
                    return PietyCategory.Low;
                if ((double)this.CurLevel < 0.699999988079071)
                    return PietyCategory.Satisfied;
                return (double)this.CurLevel < 0.850000023841858 ? PietyCategory.High : PietyCategory.Extreme;
            }
        }

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
            pietyEffectHandler.PietyInterval();
            this.CurLevel -= this.def.seekerFallPerHour * 0.06f * pietyEffectHandler.MultiplierValue;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<PietyHandler>(ref this.pietyEffectHandler, "pietyEffectHandler", pawn);
        }
    }
}
