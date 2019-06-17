using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class Religion : IExposable
    {
        public ReligionDef Def { get; }
        public ReligionSettings_Need NeedSettings { get; set; }

        public Religion(ReligionDef def)
        {
            Def = null;
            InitializeReligion();
        }

        private void InitializeReligion()
        {
            NeedSettings = Def.FindByTag<ReligionSettings_Need>(SettingsTagDefOf.NeedTag);
        }

        public void ExposeData()
        {

        }
    }
}
