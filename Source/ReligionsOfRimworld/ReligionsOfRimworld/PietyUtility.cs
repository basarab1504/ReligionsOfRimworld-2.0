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

        public static void TryApplyOnPawns(ReligionPropertyData propertyData, IEnumerable<Pawn> pawns, Pawn subjectPawn = null)
        {
            foreach (Pawn pawn in pawns)
                TryApplyOnPawn(propertyData, pawn, subjectPawn);
        }

        public static void TryApplyOnPawn(IEnumerable<ReligionPropertyData> propertyData, Pawn pawn, Pawn subjectPawn = null)
        {
            foreach (ReligionPropertyData property in propertyData)
                TryApplyOnPawn(property, pawn, subjectPawn);
        }

        public static void TryApplyOnPawn(ReligionPropertyData propertyData, Pawn pawn, Pawn subjectPawn = null)
        {
            if (propertyData != null)
            {
                int curPietyStage = pawn.GetReligionComponent().PietyTracker.PietyNeed.CurCategoryInt;

                if (propertyData.Thought != null)
                    AddThought(pawn, curPietyStage, propertyData.Thought);

                if (propertyData.OpinionThought != null)
                    AddThought(pawn, curPietyStage, propertyData.OpinionThought, subjectPawn);

                if (propertyData.Piety != null)
                    AddPiety(pawn, propertyData.Piety);
            }
        }

        private static void AddPiety(Pawn pawn, PietyDef pietyDef)
        {
            if (pietyDef != null)
                pawn.GetReligionComponent().PietyTracker.PietyNeed.Add(new Piety_Memory(pawn, pietyDef));
        }

        private static void AddThought(Pawn pawn, int curPietyStage, ThoughtDef thoughtDef, Pawn otherPawn = null)
        {
            if(thoughtDef != null)
                pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, curPietyStage), otherPawn);
        }
    }
}
