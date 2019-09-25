using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ActivityTaskDef : Def, IDescribable
    {
        private List<ThingDefsCount> thingDefsCount = new List<ThingDefsCount>();
        private ActivityJobQueueDef activityJobQueue;
        private ReligionPropertyData organizerProperty;
        private ReligionPropertyData congregationProperty;
        [Unsaved]
        private IngredientValueGetter ingredientValueGetterInt;
        private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

        public IEnumerable<ThingDefsCount> ThingDefsCount => thingDefsCount;
        public ActivityJobQueueDef ActivityQueue => activityJobQueue;
        public ReligionPropertyData OrganizerProperty => organizerProperty;
        public ReligionPropertyData CongregationProperty => congregationProperty;

        public IngredientValueGetter IngredientValueGetter
        {
            get
            {
                if (this.ingredientValueGetterInt == null)
                    this.ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(this.ingredientValueGetterClass);
                return this.ingredientValueGetterInt;
            }
        }

        public IEnumerable<ReligionInfoEntry> GetInfoEntries()
        {
            yield return new ReligionInfoEntry("ReligionInfo_Activity".Translate(), label.CapitalizeFirst(), description);

            if (organizerProperty != null)
                foreach (ReligionInfoEntry entry in organizerProperty.GetInfoEntries())
                    yield return entry;

            if (congregationProperty != null)
                foreach (ReligionInfoEntry entry in congregationProperty.GetInfoEntries())
                    yield return entry;
        }
    }


}
