using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_Incidents : ReligionSettings
    {
        private List<IncidentDef> incidents;

        public ReligionSettings_Incidents()
        {
            incidents = new List<IncidentDef>();
        }

        public IEnumerable<IncidentDef> Incidents => incidents;

        public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            foreach (IncidentDef incident in incidents)
                yield return new ReligionInfoEntry("ReligionInfo_Incident", incident.LabelCap);
        }
    }
}
