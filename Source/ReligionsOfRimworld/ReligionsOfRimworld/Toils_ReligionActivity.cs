using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ReligionsOfRimworld
{
    public static class Toils_ReligionActivity
    {
        public static Toil StageEnded(Pawn pawn)
        {
            return new Toil()
            {
                initAction = (Action)(() => ActivityUtility.TrySendStageEndedSignal(pawn))
            };                
        }
    }
}
