using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public class JobDriver_ListenPrayers : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Waiting();
            yield return Toils_ReligionActivity.StageEnded(pawn);
        }

        private Toil Waiting()
        {
            return new Toil()
            {
                defaultCompleteMode = ToilCompleteMode.Delay,
                defaultDuration = 700,
            }.WithProgressBarToilDelay(TargetIndex.A, false, .5f);
        }
    }
}
