using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Pawn_ReligionRestrictions : IExposable
    {
        private bool mayConvertByTalking = true;
        private bool mayDoReligionActivities = true;
        private bool mayPray = true;

        public bool MayConvertByTalking
        {
            get => mayConvertByTalking;
            set => mayConvertByTalking = value;
        }

        public bool MayDoReligionActivities
        {
            get => mayDoReligionActivities;
            set => mayDoReligionActivities = value;
        }

        public bool MayPray
        {
            get => mayPray;
            set => mayPray = value;
        }

        public void RestoreToDefault()
        {
            mayConvertByTalking = true;
            mayDoReligionActivities = true;
            mayPray = true;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.mayConvertByTalking, "mayConvertByTalking");
            Scribe_Values.Look<bool>(ref this.mayDoReligionActivities, "mayDoReligionActiviries");
            Scribe_Values.Look<bool>(ref this.mayPray, "mayPray");
        }
    }
}
