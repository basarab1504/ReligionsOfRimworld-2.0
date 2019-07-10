using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    class SpectateReligionActivity : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            PawnDuty duty = pawn.mindState.duty;
            if (duty == null)
                return (Job)null;

            IntVec3 cell;
            if (!SpectatorCellFinder.TryFindSpectatorCellFor(pawn, duty.spectateRect, pawn.Map, out cell, duty.spectateRectAllowedSides, 1, (List<IntVec3>)null))
                return (Job)null;
            IntVec3 centerCell = duty.spectateRect.CenterCell;
            Building edifice = cell.GetEdifice(pawn.Map);

            ReligionActivityUtility.TrySendStageEndedSignal(pawn);
            if (edifice != null && edifice.def.category == ThingCategory.Building && (edifice.def.building.isSittable && pawn.CanReserve((LocalTargetInfo)((Thing)edifice), 1, -1, (ReservationLayerDef)null, false)))
                return new Job(JobDefOf.SpectateCeremony, (LocalTargetInfo)((Thing)edifice), (LocalTargetInfo)centerCell);
            return new Job(JobDefOf.SpectateCeremony, (LocalTargetInfo)cell, (LocalTargetInfo)centerCell);
        }
    }
}
