using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public abstract class ReligionPermission : IExposable
    {
        protected bool shouldHave = true;
        protected ReligionPermissionImportance importance = ReligionPermissionImportance.Regular;

        public abstract string Reason
        {
            get;
        }

        public bool ShouldHave => shouldHave;
        public ReligionPermissionImportance Importance => importance;

        public float PermissionRate(Pawn pawn)
        {
            if (!IsPermitted(pawn))
            {
                switch (importance)
                {
                    case ReligionPermissionImportance.Low:
                        return .25f;
                    case ReligionPermissionImportance.Regular:
                        return .5f;
                    case ReligionPermissionImportance.Important:
                        return .75f;
                    case ReligionPermissionImportance.Critical:
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
