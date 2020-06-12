using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ReligionsOfRimworld
{
    public class PietyWorker_TempleImpressiveness : PietyWorker
    {
        public override PietyState CurrentState(Pawn p)
        {
            Room room = p.GetRoom(RegionType.Set_Passable);
            if (room == null)
                return PietyState.Inactive;
            int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
            if (this.def.Stages.ElementAt(scoreStageIndex) == null)
                return PietyState.Inactive;
            if (p.GetRoom(RegionType.Set_Passable).Role == MiscDefOf.Temple)
                return PietyState.ActiveAtStage(scoreStageIndex);
            return PietyState.Inactive;
        }
    }
}
