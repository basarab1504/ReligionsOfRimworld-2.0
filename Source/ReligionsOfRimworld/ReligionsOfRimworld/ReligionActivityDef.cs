using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public class ReligionActivityDef : RecipeDef /*IDescribable*/
    {
        private ActivityJobQueueDef activityJobQueue;
        private ReligionPropertyData organizerProperty;
        private ReligionPropertyData congregationProperty;
        private ThingFilter ingredientHumanlike;
        private ThingFilter ingredientAnimal;

        public ActivityJobQueueDef ActivityJobQueue => activityJobQueue;
        public ReligionPropertyData Subject => organizerProperty;
        public ReligionPropertyData Witness => congregationProperty;
        public ThingFilter IngredientHumanlike => ingredientHumanlike;
        public ThingFilter IngredientAnimal => ingredientAnimal;

        //public IEnumerable<ReligionInfoEntry> GetInfoEntries()
        //{
        //    //yield return new ReligionInfoEntry("");
        //    //if (recipe != null)
        //    //    yield return new ReligionInfoEntry("ReligionInfo_Activity".Translate(), recipe.LabelCap, recipe.description);

        //    //if (organizerProperty != null)
        //    //    foreach (ReligionInfoEntry entry in organizerProperty.GetInfoEntries())
        //    //        yield return entry;

        //    //if (congregationProperty != null)
        //    //    foreach (ReligionInfoEntry entry in congregationProperty.GetInfoEntries())
        //    //        yield return entry;
        //}
    }
}
