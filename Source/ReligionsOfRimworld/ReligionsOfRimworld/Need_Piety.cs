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

        public IEnumerable<Piety> Piety => pietyEffectHandler.Piety;

        public void Add(Piety_Memory piety)
        {
            pietyEffectHandler.Add(piety);
        }

        public void Remove(Piety_Memory piety)
        {
            pietyEffectHandler.Remove(piety);
        }

        public int CurCategoryIntWithoutZero => CurCategoryInt + 1;

        public int CurCategoryInt
        {
            get
            {
                switch (CurCategory)
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

        public override float CurInstantLevel => Mathf.Clamp01(this.pietyEffectHandler.TotalOffset() / 100f);

        public override void NeedInterval()
        {
            if (this.IsFrozen)
                return;

            float curInstantLevel = this.CurInstantLevel;
            if ((double)curInstantLevel > (double)this.CurLevel)
            {
                this.CurLevel += this.def.seekerRisePerHour * 0.06f;
                this.CurLevel = Mathf.Min(this.CurLevel, curInstantLevel);
            }
            if ((double)curInstantLevel >= (double)this.CurLevel)
                return;
            this.CurLevel -= this.def.seekerFallPerHour * 0.06f;
            this.CurLevel = Mathf.Max(this.CurLevel, curInstantLevel);

            pietyEffectHandler.PietyInterval();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<PietyHandler>(ref this.pietyEffectHandler, "pietyEffectHandler", pawn);
        }
    }
}
