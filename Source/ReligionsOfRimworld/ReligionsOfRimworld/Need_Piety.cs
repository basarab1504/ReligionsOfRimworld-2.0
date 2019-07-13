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
                0.16f,
                0.32f,
                0.64f,
                0.80f
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

        public int CurCategoryIntWithoutZero => CurCategoryInt + 1;

        public int CurCategoryInt
        {
            get
            {
                switch(CurCategory)
                {
                    case PietyCategory.VeryLow:
                        return 0;
                    case PietyCategory.Low:
                        return 1;
                    case PietyCategory.Satisfied:
                        return 2;
                    case PietyCategory.High:
                        return 3;
                    case PietyCategory.Extreme:
                        return 4;
                    default:
                        return 0;
                }
            }
        }

        public PietyCategory CurCategory
        {
            get
            {
                if ((double)this.CurLevel < 0.160000005960464)
                    return PietyCategory.VeryLow;
                if ((double)this.CurLevel < 0.320000005960464)
                    return PietyCategory.Low;
                if ((double)this.CurLevel < 0.640000011920929)
                    return PietyCategory.Satisfied;
                if ((double)this.CurLevel < 0.809999988079071)
                    return PietyCategory.High;
                return PietyCategory.Extreme;
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
