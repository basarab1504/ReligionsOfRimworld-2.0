using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class WorkGiver_UntiePawn : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.InteractionCell;
            }
        }

        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Some;
        }

        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn target = t as Pawn;
            if (target == null)
                return false;

            if (!pawn.CanReserveAndReach(target, PathEndMode.ClosestTouch, Danger.Some))
                return false;

            if (target.CurJobDef == MiscDefOf.TiedDown)
            {
                if (pawn.Faction != target.Faction || pawn == target)
                    return forced;
                return true;
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn target = t as Pawn;

            if (target == null)
                return (Job)null;

            return new Job(MiscDefOf.UntiePawn, t);
        }
    }
}
