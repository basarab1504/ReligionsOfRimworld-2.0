using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ReligionsOfRimworld
{
    public class JobGiver_ActivityStage : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            return ((LordJob_ReligionActivity)pawn.GetLord().LordJob).GetCurrentJob(pawn);
        }
    }
}
