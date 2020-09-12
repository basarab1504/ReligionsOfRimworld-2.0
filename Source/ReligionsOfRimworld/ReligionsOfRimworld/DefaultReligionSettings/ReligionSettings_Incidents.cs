using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_Incidents : ReligionSettings
    {
        private List<IncidentDef> incidents;

        public ReligionSettings_Incidents()
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            incidents = new List<IncidentDef>();
        }

        public IEnumerable<IncidentDef> Incidents => incidents;

        //public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        //{
        //    foreach (IncidentDef incident in incidents)
        //        yield return new ReligionInfoEntry("ReligionInfo_Incident".Translate(), incident.LabelCap, incident.description);
        //}

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<IncidentDef>(ref this.incidents, "incidents", LookMode.Def);
        }
    }
}
