using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class Religion_IngestingUtility
    {
        private static List<ReligionProperty> properties = new List<ReligionProperty>();

        public static void FoodIngested(Pawn ingester, Thing foodSource, ThingDef foodDef)
        {
            properties.Clear();
            ReligionSettings_Social settings = ingester.GetReligionComponent().Religion.FoodSettings;
            if(settings != null)
            {
                properties.Add(settings.GetPropertyByObject(foodDef));

                CompIngredients comp = foodSource.TryGetComp<CompIngredients>();
                if(comp != null)
                {
                    foreach (ThingDef ingridient in comp.ingredients)
                        properties.Add(settings.GetPropertyByObject(ingridient));
                }
                AppendThoughts(new ReligionProperty_ThingObject());
            }
        }

        private static void AppendThoughts(IEnumerable<ReligionProperty> __result)
        {
            foreach (ReligionProperty property in properties)
                if (property != null && property.IndividualThought != null)
                    __result.Add(property.IndividualThought);
        }
    }
}
