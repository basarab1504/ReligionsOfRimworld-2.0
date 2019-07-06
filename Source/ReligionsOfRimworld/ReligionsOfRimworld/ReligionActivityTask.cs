using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityTask : IExposable, ILoadReferenceable
    {
        private int loadID;
        private ThingFilter fixedFilter;
        private ThingFilter dynamicFilter;
        private bool suspended;

        public ReligionActivityTask(IEnumerable<ThingDef> allowedThings)
        {
            if(Scribe.mode == LoadSaveMode.Inactive)
            {
                this.loadID = Find.UniqueIDsManager.GetNextBillID();
                fixedFilter = new ThingFilter();
                dynamicFilter = new ThingFilter();

                foreach (ThingDef def in allowedThings)
                    fixedFilter.SetAllow(def, true);

                dynamicFilter.SetAllowAll(fixedFilter);
            }
        }

        public ThingFilter FixedFilter => fixedFilter;
        public ThingFilter DynamicFilter => dynamicFilter;
        public bool Suspended => suspended;

        public bool ShouldDoNow => !suspended;

        public string GetUniqueLoadID()
        {
            return "ReligionActivityTask_" + (object)this.loadID;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.loadID, "loadID");
            Scribe_Deep.Look<ThingFilter>(ref this.fixedFilter, "fixedFilter");
            Scribe_Deep.Look<ThingFilter>(ref this.dynamicFilter, "dynamicFilter");
            Scribe_Values.Look<bool>(ref this.suspended, "suspended");
        }
    }
}
