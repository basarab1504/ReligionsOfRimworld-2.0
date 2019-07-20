using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace ReligionsOfRimworld
{
    public static class PrayUtility
    {
        public static void TickCheckEnd(Pawn pawn)
        {
            Need_Pray prayNeed = pawn.GetReligionComponent().PrayTracker.PrayNeed;
            if (prayNeed != null)
            {
                prayNeed.Gain(0.0008f);
                if (prayNeed.CurLevel <= 0.999899983406067)
                    return;
                else
                    pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
            }
        }
    }
}
