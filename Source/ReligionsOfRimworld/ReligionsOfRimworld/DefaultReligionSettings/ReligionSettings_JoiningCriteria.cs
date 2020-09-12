using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionSettings_JoiningCriteria : ReligionSettings
    {
        private List<JoiningCriteria> criteria;

        public ReligionSettings_JoiningCriteria()
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            criteria = new List<JoiningCriteria>();
        }

        public IEnumerable<JoiningCriteria> Criteria => criteria;

        //public override IEnumerable<ReligionInfoEntry> GetInfoEntries()
        //{      
        //    foreach(JoiningCriteria joiningCriteria in criteria)
        //        yield return new ReligionInfoEntry("ReligionInfo_Criteria".Translate(), CriteriaTitle(joiningCriteria), "ReligionInfo_CriteriaImportance".Translate() + ": " + joiningCriteria.Importance);
        //}

        private string CriteriaTitle(JoiningCriteria criteria)
        {
            if (criteria.ShouldHave)
                return $"\"{criteria.Reason}\" required";
            else
                return $"lack of \"{criteria.Reason}\" required";
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look<JoiningCriteria>(ref this.criteria, "criteria", LookMode.Deep);
        }
    }
}
