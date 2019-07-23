using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityProperty : IExposable
    {
        private string label;
        private string description;
        private ActivityJobQueueDef activityJobQueue;
        private ReligionPropertyData organizerProperty;
        private ReligionPropertyData congregationProperty;
        private List<IngredientCount> ingredients = new List<IngredientCount>();
        private ThingFilter defaultIngredientFilter;

        public string Label => label;
        public string Description => description;
        public ActivityJobQueueDef ActivityJobQueue => activityJobQueue;
        public ReligionPropertyData Subject => organizerProperty;
        public ReligionPropertyData Witness => congregationProperty;

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

        public virtual void ExposeData()
        {
            Scribe_Values.Look<string>(ref this.label, "label");
            Scribe_Values.Look<string>(ref this.description, "description");
            Scribe_Defs.Look<ActivityJobQueueDef>(ref this.activityJobQueue, "activityJobQueue");
            Scribe_Deep.Look<ReligionPropertyData>(ref this.organizerProperty, "organizerProperty");
            Scribe_Deep.Look<ReligionPropertyData>(ref this.congregationProperty, "congregationProperty");
            //Scribe_Defs.Look<RecipeDef>(ref this.recipe, "recipe");
        }
    }
}
