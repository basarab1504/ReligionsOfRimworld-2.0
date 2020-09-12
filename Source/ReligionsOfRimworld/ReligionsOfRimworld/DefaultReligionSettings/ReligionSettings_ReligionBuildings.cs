using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_AllowedBuildings : ReligionSettings
    {
        private List<ThingDef> allowedBuildings;

        public ReligionSettings_AllowedBuildings()
        {
            if (Scribe.mode == LoadSaveMode.Inactive)
                allowedBuildings = new List<ThingDef>();
        }

        public IEnumerable<ThingDef> AllowedBuildings => allowedBuildings;

        //public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        //{
        //    foreach (ThingDef building in allowedBuildings)
        //        yield return new ReligionInfoEntry("ReligionInfo_Building".Translate(), building.LabelCap, building.description);
        //}

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<ThingDef>(ref this.allowedBuildings, "allowedBuildings", LookMode.Def);
        }
    }
}
