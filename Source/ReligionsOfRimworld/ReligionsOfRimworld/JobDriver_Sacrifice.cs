using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class JobDriver_Sacrifice : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn pawn = this.pawn;
            LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
            Job job = this.job;
            if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
            {
                return false;
            }
            this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Waiting();
            Toil stageEnded = Toils_ReligionActivity.StageEnded(pawn);
            yield return Toils_Jump.JumpIf(stageEnded, () => this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
            Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
            yield return extract;
            Toil getToTarget = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
            yield return getToTarget;
            yield return Sacrifce(TargetIndex.B);
            yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, extract);
            yield return Waiting();
            yield return stageEnded;
        }

        private Toil Sacrifce(TargetIndex sacrificable)
        {          
            return new Toil()
            {
                initAction = delegate
                {
                    Thing sacrificableThing = pawn.CurJob.GetTarget(sacrificable).Thing;
                    if (sacrificableThing != null)
                    {
                        Pawn sacrificablePawn = sacrificableThing as Pawn;
                        if (sacrificablePawn != null)
                        {
                            ExecutionUtility.DoExecutionByCut(pawn, sacrificablePawn);
                            pawn.Reserve(sacrificablePawn.Corpse, this.job);
                        }
                        else
                            sacrificableThing.Destroy(DestroyMode.Vanish);
                    }
                }
            };
        }

        private Toil Waiting()
        {
            return new Toil()
            {
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 400,
            }.WithProgressBarToilDelay(TargetIndex.A, false, .5f);
        }
    }
}
