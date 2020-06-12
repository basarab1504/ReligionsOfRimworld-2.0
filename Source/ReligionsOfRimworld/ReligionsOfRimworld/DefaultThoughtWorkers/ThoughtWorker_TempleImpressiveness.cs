using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace ReligionsOfRimworld
{
    public class ThoughtWorker_TempleImpressiveness : ThoughtWorker_RoomImpressiveness
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            ThoughtState thoughtState = base.CurrentStateInternal(p);
            if (thoughtState.Active && p.GetRoom(RegionType.Set_Passable).Role == MiscDefOf.Temple)
                return thoughtState;
            return ThoughtState.Inactive;
        }
    }
}
