//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Verse;

//namespace ReligionsOfRimworld
//{
//    public class ReligionActivityTask : IExposable, ILoadReferenceable
//    {
//        private int loadID = -1;
//        ReligionActivityStack stack; 
//        public bool suspended;
//        public ThingFilter ingredientFilter;
//        public Pawn pawnRestriction;

//        public ReligionActivityTask(ReligionSettings_ReligionActivity religionSettings)
//        {
//            if (Scribe.mode == LoadSaveMode.Inactive)
//            {
//                this.loadID = Find.UniqueIDsManager.GetNextBillID();
//                ingredientFilter = new ThingFilter();

//                if (religionSettings.ActivityRelics != null)
//                    foreach (ThingDef def in GetAllThingDefsFromProperties(religionSettings.ActivityRelics))
//                        ingredientFilter.SetAllow(def, true);
//            }
//        }

//        public bool ShouldDoNow()
//        {
//            return !suspended;
//        }

//        private IEnumerable<ThingDef> GetAllThingDefsFromProperties(IEnumerable<ReligionProperty> properties)
//        {
//            foreach (ReligionProperty property in properties)
//                if (property != null && property.GetObject() != null)
//                    yield return (ThingDef)property.GetObject();
//        }

//        public string GetUniqueLoadID()
//        {
//            return "ReligionActivityTask_" + (object)this.loadID;
//        }

//        public void ExposeData()
//        {
//            Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
//            Scribe_Values.Look<bool>(ref this.suspended, "suspended", false, false);
//            Scribe_References.Look<Pawn>(ref this.pawnRestriction, "pawnRestriction", false);
//            Scribe_Deep.Look<ThingFilter>(ref this.ingredientFilter, "ingredientFilter");
//        }
//    }
//}
