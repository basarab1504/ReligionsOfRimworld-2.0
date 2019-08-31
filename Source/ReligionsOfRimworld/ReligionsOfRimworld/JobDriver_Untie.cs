using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class JobDriver_UntiePawn : JobDriver_Wait
    {
        public Pawn TargetPawn => (Pawn)TargetA;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.CanReserve(TargetPawn))
                return true;
            else
                return false;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
            yield return new Toil()
            {
                initAction = delegate
                {
                    TargetPawn.jobs.CheckForJobOverride();
                }
            };
        }
    }
}
