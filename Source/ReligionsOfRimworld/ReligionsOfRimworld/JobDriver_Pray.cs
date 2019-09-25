using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class JobDriver_Pray : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn pawn = this.pawn;
            LocalTargetInfo target = this.job.targetA;
            Job job = this.job;
            int num = this.job.def.joyMaxParticipants;
            int num2 = 0;
            pawn = this.pawn;
            target = this.job.targetB;
            job = this.job;
            if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
            {
                return false;
            }
            if (base.TargetC.HasThing)
            {
                if (base.TargetC.Thing is Building_Bed)
                {
                    pawn = this.pawn;
                    LocalTargetInfo targetC = this.job.targetC;
                    job = this.job;
                    num2 = ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount;
                    num = 0;
                    if (!pawn.Reserve(targetC, job, num2, num, null, errorOnFailed))
                    {
                        return false;
                    }
                }
                else
                {
                    pawn = this.pawn;
                    LocalTargetInfo targetC = this.job.targetC;
                    job = this.job;
                    if (!pawn.Reserve(targetC, job, 1, -1, null, errorOnFailed))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            base.AddEndCondition(delegate
            {
                Thing thing = this.GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
                if (thing is Building && !thing.Spawned)
                {
                    return JobCondition.Incompletable;
                }
                return JobCondition.Ongoing;
            });
            this.FailOnBurningImmobile(TargetIndex.A);

            if (TargetC.HasThing)
                yield return Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.OnCell);
            else
                yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);

            yield return new Toil()
            {
                defaultCompleteMode = ToilCompleteMode.Never,
                tickAction = delegate
                {
                    PrayUtility.TickCheckEnd(pawn);
                    this.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
                    this.pawn.GainComfortFromCellIfPossible();
                }
            };

            this.AddFinishAction(() =>
            {
                PietyUtility.TryApplyOnPawn(pawn.GetReligionComponent().Religion.PrayingSettings.PrayProperty, pawn);
            });
        }
    }
}
