using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Pawn_PrayTracker : IExposable
    {
        private int lastPrayTick;

        public int LastPrayTick { get => lastPrayTick; set => lastPrayTick = value; }
        //public int LastPrayHour => lastPrayTick * 2500;

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref lastPrayTick, "lastPrayTick");
        }
    }
}
