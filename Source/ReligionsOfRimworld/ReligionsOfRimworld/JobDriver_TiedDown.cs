using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class JobDriver_TiedDown : JobDriver_Wait
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {

            yield return new Toil
            {
                initAction = delegate
                {
                    //this.pawn.Reserve(this.pawn.Position, this.job);
                    this.pawn.pather.StopDead();
                    JobDriver curDriver = this.pawn.jobs.curDriver;
                    pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
                    curDriver.asleep = false;
                },
                tickAction = delegate
                {
                    if (this.job.expiryInterval == -1 && this.job.def == JobDefOf.Wait_Combat && !this.pawn.Drafted)
                    {
                        Log.Error(this.pawn + " in eternal WaitCombat without being drafted.");
                        this.ReadyForNextToil();
                        return;
                    }

                },
                defaultCompleteMode = ToilCompleteMode.Never
            };
        }
    }
}
