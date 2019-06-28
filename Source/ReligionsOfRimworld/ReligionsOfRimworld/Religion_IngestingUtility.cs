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
                PietyUtility.TryApplyReligionPropertiesIndividual(ingester, properties);
            }
        }
    }
}
