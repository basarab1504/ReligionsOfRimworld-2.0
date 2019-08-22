using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ActivityTaskDef : Def
    {
        private Dictionary<ThingDef, int> thingDefsCount = new Dictionary<ThingDef, int>();
        private ActivityJobQueueDef activityJobQueue;
        private ReligionPropertyData organizerProperty;
        private ReligionPropertyData congregationProperty;
        [Unsaved]
        private IngredientValueGetter ingredientValueGetterInt;
        private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

        public IEnumerable<KeyValuePair<ThingDef, int>> ThingDefsCount => thingDefsCount;
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
            //yield return new ReligionInfoEntry("");
            //if (recipe != null)
            //    yield return new ReligionInfoEntry("ReligionInfo_Activity".Translate(), recipe.LabelCap, recipe.description);

            if (organizerProperty != null)
                foreach (ReligionInfoEntry entry in organizerProperty.GetInfoEntries())
                    yield return entry;

            if (congregationProperty != null)
                foreach (ReligionInfoEntry entry in congregationProperty.GetInfoEntries())
                    yield return entry;
        }
    }
}
