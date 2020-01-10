using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Pawn_PrayTracker : IExposable
    {
        private Pawn pawn;
        private Need_Pray pray;

        public Pawn_PrayTracker(Pawn pawn, Religion religion)
        {
            this.pawn = pawn;
            if (Scribe.mode == LoadSaveMode.Inactive)
            {
                if (religion.GetSettings<ReligionSettings_Prayings>(SettingsTagDefOf.PrayingsTag) != null)
                    this.pray = new Need_Pray(pawn)
                {
                    def = religion.GetSettings<ReligionSettings_Prayings>(SettingsTagDefOf.PrayingsTag).Need
                };
            }
        }

        public Need_Pray PrayNeed => pray;

        public void TrackerTick()
        {
            if (!this.pawn.IsHashIntervalTick(150))
                return;
            if (pray == null)
                return;
            pray.NeedInterval();
        }

        public void ExposeData()
        {
            Scribe_Deep.Look<Need_Pray>(ref this.pray, "pray", pawn);
        }
    }
}
