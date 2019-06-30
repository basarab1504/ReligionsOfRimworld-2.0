using RimWorld;
using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ReligionsOfRimworld
{
    public static class ReligionCardUtility
    {
        private static Pawn selPawn;
        private static List<Piety> cachedMultipliers = new List<Piety>();
        private static Vector2 scrollPosition = Vector2.zero;
        private static CompReligion CompReligion => selPawn.GetReligionComponent();
        private static Religion Religion => CompReligion.Religion;
        private static Need_Piety Piety => CompReligion.PietyTracker.Piety;

        public static void DrawReligionCard(Rect rect, Pawn pawn)
        {
            selPawn = pawn;
            //GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            //GUI.BeginGroup(leftSide);
            GUI.color = new Color(1f, 1f, 1f, 0.5f);
            GUI.color = Color.white;
            LeftSide(rect.LeftHalf().ContractedBy(10f), pawn);
            RightSide(rect.RightHalf().ContractedBy(10f), pawn);
            //GUI.EndGroup();
            //GUI.EndGroup();
        }

        public static void LeftSide(Rect rect, Pawn pawn)
        {
            GUI.BeginGroup(rect);
            float y = 0.0f;
            DrawOverall(new Rect(0.0f, y, rect.width, rect.height), pawn, ref y);
            //y += 30f;
            //DrawRestrictions(new Rect(0.0f, y, rect.width, rect.height), pawn, ref y);
            y += 30f;
            DrawCompabilities(new Rect(0.0f, y, rect.width, rect.height), pawn, ref y);
            GUI.EndGroup();
        }

        public static void DrawOverall(Rect rect, Pawn pawn, ref float curY)
        {
            GUI.BeginGroup(rect);
            float y = 0.0f;
            Widgets.ListSeparator(ref y, rect.width, "ReligionInfo_Overall".Translate());
            Rect rect2 = new Rect(0.0f, y, rect.width, 24f);
            y += 24f;
            Widgets.Label(rect2, Religion.Label);
            if (Mouse.IsOver(rect2))
                Widgets.DrawHighlight(rect2);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Religion.Label);
            stringBuilder.AppendLine();
            stringBuilder.Append(Religion.Description);
            stringBuilder.AppendLine();
            TooltipHandler.TipRegion(rect2, new TipSignal(stringBuilder.ToString(), 7773));
            curY += y;
            GUI.EndGroup();
        }

        public static void RightSide(Rect rect, Pawn pawn)
        {
            if (Piety != null)
            {
                Rect rect2 = new Rect(rect.x, rect.y, 225f, 70);
                if (Piety != null)
                    Piety.DrawOnGUI(rect2, int.MaxValue, -1f, true, true);
                Rect rect3 = new Rect(rect.x, rect2.y + 80, rect.width, 20f);
                if (Mouse.IsOver(rect3))
                    Widgets.DrawHighlight(rect3);
                Widgets.Label(rect3, "PietyMultiplier".Translate() + " " + Piety.MultiplierValue.ToString());
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(Religion.Label);
                TooltipHandler.TipRegion(rect3, new TipSignal(stringBuilder.ToString(), 7773));
                DrawMultiplierListing(new Rect(rect.x, rect3.y + 30, rect.width, rect.height), pawn);
            }
        }

        public static void DrawMultiplierListing(Rect listingRect, Pawn pawn)
        {
            if (Event.current.type == EventType.Layout)
                return;
            Verse.Text.Font = GameFont.Small;
            cachedMultipliers.Clear();
            Piety.GetPiety(cachedMultipliers);
            float height = cachedMultipliers.Count * 24f;
            Widgets.BeginScrollView(listingRect, ref scrollPosition, new Rect(0.0f, 0.0f, listingRect.width, height), false);
            Verse.Text.Anchor = TextAnchor.MiddleLeft;
            float y = 0.0f;
            foreach (Piety piety in cachedMultipliers)
                DrawMultiplier(piety, listingRect, ref y);
            Widgets.EndScrollView();
            Verse.Text.Anchor = TextAnchor.UpperLeft;
        }

        public static void DrawMultiplier(Piety piety, Rect rect, ref float curY)
        {
            Rect rectRow = new Rect(0.0f, curY, rect.width, 24f);
            StringBuilder stringBuilder = new StringBuilder();
            if (piety.CurStage.Description != null)
            {
                stringBuilder.Append(piety.CurStage.Description.CapitalizeFirst());
                stringBuilder.AppendLine();
            }
            //stringBuilder.Append("PietyAdded".Translate((NamedArgument)multipliers[index].CurStage.addPiety));
            if (!piety.Def.IsSituational)
            {
                stringBuilder.Append("ThoughtExpiresIn".Translate((NamedArgument)(piety.Def.DurationTicks - (piety as Piety_Memory).Age).ToStringTicksToPeriod()));
            }
            TooltipHandler.TipRegion(rectRow, new TipSignal(stringBuilder.ToString(), 7771));
            if (Mouse.IsOver(rectRow))
                Widgets.DrawHighlight(rectRow);
            Widgets.Label(rectRow, piety.LabelCap.ToString());
            Widgets.Label(new Rect(rectRow.x + 190f, rectRow.y, rectRow.width, rectRow.height), piety.MultiplierValue.ToString());
            curY += 24f;
        }

        //public static void DrawRestrictions(Rect rect, Pawn pawn, ref float curY)
        //{
        //    GUI.BeginGroup(rect);
        //    float y = 0.0f;
        //    Widgets.ListSeparator(ref y, rect.width, "Restrictions".Translate());
        //    Rect rect2 = new Rect(rect.x, y, rect.width, 24f);
        //    y += 24f;
        //    Widgets.CheckboxLabeled(rect2, "restrict_attend", ref pawn.GetReligionComponent().religionActivity);
        //    Rect rect3 = new Rect(rect.x, y, rect2.width, 24f);
        //    y += 24f;
        //    Widgets.CheckboxLabeled(rect3, "restrict_convert", ref pawn.GetReligionComponent().ableToConvert);
        //    curY += y;
        //    GUI.EndClip();
        //}

        public static void DrawCompabilities(Rect rect, Pawn pawn, ref float curY)
        {
            GUI.BeginGroup(rect);
            float y = 0.0f;
            Widgets.ListSeparator(ref y, rect.width, "Compabilities".Translate());
            float height = CompReligion.ReligionCompability.Compabilities.Count() * 24f;
            Widgets.BeginScrollView(new Rect(0.0f, y, rect.width, rect.height), ref scrollPosition, new Rect(0.0f, y, rect.width, height), true);
            foreach (KeyValuePair<Religion, float> kvp in CompReligion.ReligionCompability.Compabilities)
            {
                Rect rectRow = new Rect(0.0f, y, rect.width, 24f);
                if (Mouse.IsOver(rectRow))
                    Widgets.DrawHighlight(rectRow);
                //StringBuilder stringBuilder = CompabilityReason(kvp.Key, pawn);
                //if (stringBuilder != null)
                //    TooltipHandler.TipRegion(rectRow, new TipSignal(stringBuilder.ToString(), 7770));
                Widgets.Label(rectRow, kvp.Key.Label);
                Widgets.Label(new Rect(rectRow.x + 210f, rectRow.y, rectRow.width, rectRow.height), kvp.Value.ToString());
                y += 24f;
            }
            curY += y;
            Widgets.EndScrollView();
            GUI.EndGroup();
        }

        //static StringBuilder CompabilityReason(Religion religion, Pawn pawn)
        //{
        //    Religion_JoiningRestrictionSettings settings = religion.joiningRestrictionSettings;
        //    StringBuilder stringBuilder = new StringBuilder();
        //    if (settings == null)
        //        return null;
        //    //stringBuilder.Append(religion.ToString());
        //    //stringBuilder.AppendLine();
        //    //stringBuilder.Append(religion.description.ToString());
        //    //stringBuilder.AppendLine();
        //    foreach (Religion_Restriction r in settings.restrictions)
        //    {
        //        stringBuilder.AppendFormat("{0}: {1} (should have: {2}) RESTRICTED: {3}", r.Importance.ToString(), r.Reason, r.ShouldHave.ToString(), r.IsRestricted(pawn));
        //        stringBuilder.AppendLine();
        //    }
        //    return stringBuilder;
        //}
    }
}
