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
        public static void AppendThoughts_Religious(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
        {
            if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died)
            {                             
                Def objectDef = null;

                if (victim.RaceProps.Humanlike)
                    objectDef = victim.GetReligionComponent().Religion.GroupTag;
                else
                    objectDef = victim.def;

                AppendThoughts_PotentialRelatedPawns(victim, SettingsTagDefOf.DeathTag, objectDef);
               
                if (dinfo.HasValue)
                {
                    Pawn instigator = (Pawn)dinfo.Value.Instigator;
                    if (instigator != null && instigator.RaceProps.Humanlike)
                    {
                        ApppendThought_ForInstigator(instigator, victim, SettingsTagDefOf.KillTag, objectDef);
                        AppendThoughts_PotentialRelatedPawns(instigator, SettingsTagDefOf.KillTag, objectDef);

                        if (dinfo.Value.Weapon != null)
                        {
                            ApppendThought_ForInstigator(instigator, victim, SettingsTagDefOf.WeaponTag, dinfo.Value.Weapon);
                        }                          
                    }
                }
            }
        }

        private static void ApppendThought_ForInstigator(Pawn subject, Pawn victim, SettingsTagDef tagDef, Def objectDef)
        {
            ReligionProperty property = GetProperty(victim, objectDef, tagDef);
            if (property != null)
            {
                PietyUtility.TryApplyOnPawn(property.Subject, subject, victim);
            }
        }

        private static void AppendThoughts_PotentialRelatedPawns(Pawn subject, SettingsTagDef tagDef, Def objectDef)
        {
            foreach (Pawn pawn in ReligionExtensions.AllMapsCaravansAndTravelingTransportPods_Alive_Religious)
            {
                if(Witnessed(pawn, subject) || pawn.Faction == subject.Faction)
                {
                    AppendThought_TryApplyOnRelatedPawn(pawn, subject, tagDef, objectDef);
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

        private static void AppendThought_TryApplyOnRelatedPawn(Pawn pawn, Pawn subject, SettingsTagDef tagDef, Def objectDef)
        {
            ReligionProperty property = GetProperty(pawn, objectDef, tagDef);
            if (property != null)
            {
                PietyUtility.TryApplyOnPawn(property.Witness, pawn, subject);
            }
        }

        private static ReligionProperty GetProperty(Pawn pawn, Def objectDef, SettingsTagDef tagDef)
        {
            ReligionSettings_Social settings = pawn.GetReligionComponent().Religion.FindByTag<ReligionSettings_Social>(tagDef);
            if (settings != null)
                return settings.GetPropertyByObject(objectDef);
            return null;
        }
    }
}
