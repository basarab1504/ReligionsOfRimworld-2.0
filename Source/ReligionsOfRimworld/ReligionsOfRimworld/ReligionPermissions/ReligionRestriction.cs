using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class JoiningCriteria : IExposable
    {
        protected bool shouldHave = true;
        protected ReligionCriteriaImportance importance = ReligionCriteriaImportance.Low;

        public abstract string Reason
        {
            get;
        }

        public bool ShouldHave => shouldHave;
        public ReligionCriteriaImportance Importance => importance;

        public float PermissionRate(Pawn pawn)
        {
            if (IsPermitted(pawn))
            {
                switch (importance)
                {
                    case ReligionCriteriaImportance.Low:
                        return .25f;
                    case ReligionCriteriaImportance.Regular:
                        return .5f;
                    case ReligionCriteriaImportance.Important:
                        return .75f;
                    case ReligionCriteriaImportance.Critical:
                        return 1f;
                    default:
                        return 0f;
                }
            }
            else
                return 0f;
        }

        private bool IsPermitted(Pawn pawn)
        {
            return IsFound(pawn) == shouldHave;
        }

        protected abstract bool IsFound(Pawn pawn);

        public abstract void ExposeData();
    }
}
