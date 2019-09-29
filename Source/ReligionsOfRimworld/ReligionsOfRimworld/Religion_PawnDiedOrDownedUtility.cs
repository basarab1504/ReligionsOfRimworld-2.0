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
            if (thoughtsKind != PawnDiedOrDownedThoughtsKind.Died)
                return;

            if (dinfo.HasValue && dinfo.Value.Instigator is Pawn)
                AppendThoughtsForPawns(victim, (Pawn)dinfo.Value.Instigator, dinfo.Value.Weapon);
            else
                AppendThoughtsForPawns(victim);
        }

        private static void AppendThoughtsForPawns(Pawn victim, Pawn instigator = null, ThingDef weapon = null)
        {
            IEnumerable<Def> criteria = GetVictimThoughtsCriteria(victim);

            if (instigator != null && instigator.RaceProps.Humanlike)
            {
                //AppendThoughtsByCriteria(instigator, SettingsTagDefOf.KillTag, criteria);
                AppendThoughtsForPawn(instigator, SettingsTagDefOf.WeaponTag, weapon);

                foreach (Pawn pawn in GetWitnesses(victim))
                    AppendThoughtsByCriteria(pawn, SettingsTagDefOf.KillTag, criteria, instigator);
            }
            foreach (Pawn pawn in GetWitnesses(victim))
                AppendThoughtsByCriteria(pawn, SettingsTagDefOf.DeathTag, criteria, victim);
        }

        private static IEnumerable<Def> GetVictimThoughtsCriteria(Pawn victim)
        {
            yield return victim.def;

            CompReligion comp = victim.GetReligionComponent();
            if (comp != null)
            {
                yield return comp.Religion.Def;
                yield return comp.Religion.GroupTag;
            }
        }

        private static void AppendThoughtsByCriteria(Pawn pawn, SettingsTagDef tag, IEnumerable<Def> criteria, Pawn otherPawn = null)
        {
            foreach (Def def in criteria)
                AppendThoughtsForPawn(pawn, tag, def, otherPawn);
        }

        private static IEnumerable<Pawn> GetWitnesses(Pawn victim)
        {
            foreach (Pawn pawn in ReligionExtensions.AllMapsCaravansAndTravelingTransportPods_Alive_Religious)
            {
                if (Witnessed(pawn, victim) || pawn.Faction == victim.Faction)
                    yield return pawn;
            }
        }

        private static void AppendThoughtsForPawn(Pawn pawn, SettingsTagDef tag, Def def, Pawn otherPawn = null)
        {
            ReligionProperty property = GetProperty(pawn, def, tag);
            if (property != null)
            {
                if(otherPawn == null || pawn == otherPawn)
                    PietyUtility.TryApplyOnPawn(property.Subject, pawn, otherPawn);
                else
                    PietyUtility.TryApplyOnPawn(property.Witness, pawn, otherPawn);
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

        private static ReligionProperty GetProperty(Pawn pawn, Def objectDef, SettingsTagDef tagDef)
        {
            ReligionSettings_Social settings = pawn.GetReligionComponent().Religion.FindByTag<ReligionSettings_Social>(tagDef);
            if (settings != null)
                return settings.GetPropertyByObject(objectDef);
            return null;
        }
    }
}
