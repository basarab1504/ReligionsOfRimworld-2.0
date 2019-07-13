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
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 1200,
                tickAction = delegate
                {
                    this.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
                    this.pawn.GainComfortFromCellIfPossible();
                    PietyUtility.PietyRateTick(Data, pawn);
                }
            }.WithProgressBarToilDelay(TargetIndex.A, false, .5f);

            this.AddFinishAction(() =>
            {
                pawn.GetReligionComponent().PrayTracker.LastPrayTick = Find.TickManager.TicksGame;
            });
        }

        private ReligionPropertyData Data => pawn.GetReligionComponent().Religion.PrayingSettings.PrayProperty;
    }
}
