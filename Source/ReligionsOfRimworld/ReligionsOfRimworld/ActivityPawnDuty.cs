using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class ActivityPawnDuty : PawnDuty
    {
        public List<LocalTargetInfo> focusThings = new List<LocalTargetInfo>();

        public ActivityPawnDuty(DutyDef def) : base(def)
        { }

        public ActivityPawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : base(def)
        { }

        public ActivityPawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : base(def, focus, radius)
        { }

        public ActivityPawnDuty(DutyDef def, LocalTargetInfo focus, IEnumerable<LocalTargetInfo> focusThings, float radius = -1f) : base(def, focus, radius)
        {
            this.focusThings.AddRange(focusThings);
        }

        public ActivityPawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, IEnumerable<LocalTargetInfo> focusThings, float radius = -1f) : base(def, focus, radius)
        {
            this.focusThings.AddRange(focusThings);
        }

        public new void ExposeData()
        {
            Scribe_Defs.Look<DutyDef>(ref this.def, "def");
            Scribe_TargetInfo.Look(ref this.focus, "focus", LocalTargetInfo.Invalid);
            Scribe_TargetInfo.Look(ref this.focusSecond, "focusSecond", LocalTargetInfo.Invalid);
            Scribe_Collections.Look<LocalTargetInfo>(ref this.focusThings, "focusThings", LookMode.LocalTargetInfo);
            Scribe_Values.Look<float>(ref this.radius, "radius", -1f, false);
            Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
            Scribe_Values.Look<Danger>(ref this.maxDanger, "maxDanger", Danger.Unspecified, false);
            Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", new CellRect(), false);
            Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.All, false);
            Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
            Scribe_Values.Look<PawnsToGather>(ref this.pawnsToGather, "pawnsToGather", PawnsToGather.None, false);
            Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", -1, false);
            Scribe_Values.Look<bool>(ref this.attackDownedIfStarving, "attackDownedIfStarving", false, false);
        }
    }
}
