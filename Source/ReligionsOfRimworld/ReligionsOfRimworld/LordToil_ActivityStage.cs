using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace ReligionsOfRimworld
{
    public class LordToil_ActivityStage : LordToil
    {
        private ReligionActivityData activtityData;

        public LordToil_ActivityStage(ReligionActivityData data)
        {
            this.activtityData = data;
        }

        public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
        {
            return MiscDefOf.ReligionActivityStageDuty.hook;
        }

        public override void UpdateAllDuties()
        {
            foreach (Pawn pawn in lord.ownedPawns)
            {
                pawn.mindState.duty = new PawnDuty(MiscDefOf.ReligionActivityStageDuty, (LocalTargetInfo)activtityData.Facility)
                {
                    spectateRect = CellRect.SingleCell(activtityData.Facility.Position)
                };
            }
        }

        private LocalTargetInfo GetRandomRelic()
        {
            IEnumerable<LocalTargetInfo> relics = activtityData.Relics;
            if (relics != null && relics.Count() != 0)
            {
                LocalTargetInfo tar = relics.RandomElement();
                Log.Message(tar.Thing.Position + " " + tar.Thing + " " + tar.Thing.stackCount);
                return tar;
            }
            return null;
        }
    }
}
