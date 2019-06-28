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

        public static void TryApplyReligionProperty(Pawn pawn, ReligionProperty property, Pawn otherPawn = null)
        {           
            int curPietyStage = pawn.GetReligionComponent().PietyTracker.Piety.CurCategoryInt;
            if (property != null)
            {             
                if(property.IndividualThought != null)
                    AddThought(pawn, ThoughtMaker.MakeThought(property.IndividualThought, curPietyStage), otherPawn);

                if(property.SocialThought != null)
                    AddThought(pawn, ThoughtMaker.MakeThought(property.SocialThought, curPietyStage), otherPawn);

                if (property.IndividualPiety != null)
                    AddPiety(pawn, property.IndividualPiety);

                if (property.SocialThought != null)
                    AddPiety(pawn, property.SocialPiety);
            }
        }

        public static void AddPiety(Pawn pawn, PietyDef pietyDef)
        {
            pawn.GetReligionComponent().PietyTracker.Piety.Add(new Piety_Memory(pawn, pietyDef));
        }

        private static void AddThought(Pawn pawn, Thought_Memory memory, Pawn otherPawn = null)
        {
            pawn.needs.mood.thoughts.memories.TryGainMemory(memory);
        }
    }
}
