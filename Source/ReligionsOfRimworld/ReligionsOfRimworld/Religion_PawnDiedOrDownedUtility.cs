using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    public static class Religion_PawnDiedOrDownedUtility
    {
        private static List<SettingsTagDef> tagsToLook = new List<SettingsTagDef>();

        public static void AppendThoughts_Religious(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
        {
            tagsToLook.Clear();
            if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died)
            {                             
                tagsToLook.Add(SettingsTagDefOf.DeathTag);
                Def objectDef = null;

                if (victim.RaceProps.Humanlike)
                    objectDef = victim.GetReligionComponent().Religion.GroupTag;
                else
                    objectDef = victim.def;

                Pawn instigator = (Pawn)dinfo.Value.Instigator;
                if (dinfo.HasValue)
                {                  
                    if (instigator != null && instigator.RaceProps.Humanlike)
                    {
                        tagsToLook.Add(SettingsTagDefOf.KillTag);
                        AppendThought_ForInstigator(objectDef, SettingsTagDefOf.KillTag, instigator, victim);

                        if (dinfo.Value.Weapon != null)
                        {
                            AppendThought_ForInstigator(dinfo.Value.Weapon, SettingsTagDefOf.WeaponTag, instigator, victim);
                        }                          
                    }
                }
                AppendThoughts_PotentialRelatedPawns(objectDef, victim, instigator);
            }
        }

        private static void AppendThoughts_PotentialRelatedPawns(Def objectDef, Pawn victim, Pawn instigator = null)
        {
            foreach (Pawn pawn in ReligionExtensions.AllMapsCaravansAndTravelingTransportPods_Alive_Religious)
            {
                if(Witnessed(pawn, victim) || pawn.Faction == victim.Faction || (instigator != null && pawn.Faction == instigator.Faction))
                {
                    foreach (SettingsTagDef tag in tagsToLook)
                        AppendThought_PotentialRelatedPawn(objectDef, tag, pawn, victim, instigator);
                }
            }
        }


        private static bool Witnessed(Pawn pawn, Pawn victim)
        {
            if (!pawn.Awake() || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
                return false;
            if (victim.IsCaravanMember())
                return victim.GetCaravan() == pawn.GetCaravan();
            return victim.Spawned && pawn.Spawned && (pawn.Position.InHorDistOf(victim.Position, 12f) && GenSight.LineOfSight(victim.Position, pawn.Position, victim.Map, false, (Func<IntVec3, bool>)null, 0, 0));
        }

        private static void AppendThought_PotentialRelatedPawn(Def objectDef, SettingsTagDef tagDef, Pawn pawn, Pawn victim, Pawn instigator = null)
        {
            ReligionSettings_Social settings = pawn.GetReligionComponent().Religion.FindByTag<ReligionSettings_Social>(tagDef);
            if (settings != null)
            {
                ReligionProperty property = settings.GetPropertyByObject(objectDef);
                if (instigator != null)
                {
                    if(instigator != pawn)
                        PietyUtility.TryApplyOnPawn(property.Witness, pawn, instigator);
                }
                else
                {
                    PietyUtility.TryApplyOnPawn(property.Witness, pawn, victim);
                }
                    
            }
        }

        private static void AppendThought_ForInstigator(Def objectDef, SettingsTagDef tagDef, Pawn instigator, Pawn victim)
        {
            if (objectDef == null)
                Log.Message("no obj");
            else
                Log.Message(objectDef.ToString());

            if (tagDef == null)
                Log.Message("no tagDef");
            else
                Log.Message(tagDef.ToString());

            if (instigator == null)
                Log.Message("no instigator");
            else
                Log.Message(instigator.ToString());

            if (victim == null)
                Log.Message("no victim");
            else
                Log.Message(victim.ToString());

            if(instigator.GetReligionComponent() == null)
                Log.Message("no GetReligionComponent");
            else
                Log.Message("comp is ok");

            if (instigator.GetReligionComponent().Religion == null)
                Log.Message("no GetReligionComponent");
            else
                Log.Message("Religion is ok");

            ReligionSettings_Social settings = instigator.GetReligionComponent().Religion.FindByTag<ReligionSettings_Social>(tagDef);
            if (settings != null)
            {
                Log.Message("settings is ok");
                ReligionProperty property = settings.GetPropertyByObject(objectDef);
                PietyUtility.TryApplyOnPawn(property.Subject, instigator, victim);
            }
            else
                Log.Message("settings is not ok");
        }
    }
}
