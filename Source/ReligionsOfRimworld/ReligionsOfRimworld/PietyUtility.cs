using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class PietyUtility
    {
        public static List<PietyDef> situationalPietyList = DefDatabase<PietyDef>.AllDefs.Where(x => x.IsSituational).ToList<PietyDef>();

        public static void TryApplyReligionPropertiesIndividual(Pawn pawn, IEnumerable<ReligionProperty> properties)
        {
            foreach (ReligionProperty property in properties)
                TryApplyReligionPropertyIndividual(pawn, property);
        }

        public static void TryApplyReligionPropertyIndividual(Pawn pawn, ReligionProperty property)
        {
            int curPietyStage = pawn.GetReligionComponent().PietyTracker.Piety.CurCategoryInt;
            if(property != null)
            {
                if (property.IndividualThought != null)
                    AddThought(pawn, curPietyStage, property.IndividualThought, null);

                if (property.IndividualPiety != null)
                    AddPiety(pawn, curPietyStage, property.IndividualPiety);
            }
        }

        public static void TryApplyReligionPropertiesSocial(Pawn pawn, IEnumerable<ReligionProperty> properties, Pawn otherPawn = null)
        {
            foreach (ReligionProperty property in properties)
                TryApplyReligionPropertySocial(pawn, property, otherPawn);
        }

        public static void TryApplyReligionPropertySocial(Pawn pawn, ReligionProperty property, Pawn otherPawn = null)
        {
            int curPietyStage = pawn.GetReligionComponent().PietyTracker.Piety.CurCategoryInt;
            if (property != null)
            {
                if (property.SocialThought != null)
                    AddThought(pawn, curPietyStage, property.SocialThought, otherPawn);

                if (property.SocialPiety != null)
                    AddPiety(pawn, curPietyStage, property.SocialPiety);
            }
        }

        private static void AddPiety(Pawn pawn, int curPietyStage, PietyDef pietyDef)
        {
            if (pietyDef != null)
                pawn.GetReligionComponent().PietyTracker.Piety.Add(new Piety_Memory(pawn, pietyDef, curPietyStage));
        }

        private static void AddThought(Pawn pawn, int curPietyStage, ThoughtDef thoughtDef, Pawn otherPawn = null)
        {
            if(thoughtDef != null)
                pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, curPietyStage), otherPawn);
        }
    }
}
