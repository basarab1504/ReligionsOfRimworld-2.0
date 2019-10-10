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
        private class Info
        {
            public Pawn victim;
            public Pawn instigator;
            public ThingDef weapon;
            public IEnumerable<Def> criteria;
            public IEnumerable<Pawn> witnesses;

            public Info(Pawn victim, DamageInfo? dinfo)
            {
                this.victim = victim;
                criteria = GetVictimThoughtsCriteria();
                witnesses = GetWitnesses();

                if (dinfo.HasValue)
                {
                    if (dinfo.Value.Instigator != null && dinfo.Value.Instigator is Pawn)
                        instigator = (Pawn)dinfo.Value.Instigator;
                    if (dinfo.Value.Weapon != null)
                        weapon = dinfo.Value.Weapon;
                }
            }

            private IEnumerable<Pawn> GetWitnesses()
            {
                foreach (Pawn pawn in ReligionExtensions.AllMapsCaravansAndTravelingTransportPods_Alive_Religious)
                {
                    if (pawn != instigator && (Witnessed(pawn, victim) || pawn.Faction == victim.Faction))
                        yield return pawn;
                }
            }

            private bool Witnessed(Pawn pawn, Pawn victim)
            {
                if (!pawn.Awake() || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
                    return false;
                if (victim.IsCaravanMember())
                    return victim.GetCaravan() == pawn.GetCaravan();
                return victim.Spawned && pawn.Spawned && (pawn.Position.InHorDistOf(victim.Position, 12f) && GenSight.LineOfSight(victim.Position, pawn.Position, victim.Map, false, (Func<IntVec3, bool>)null, 0, 0));
            }

            private IEnumerable<Def> GetVictimThoughtsCriteria()
            {
                yield return victim.def;

                CompReligion comp = victim.GetReligionComponent();
                if (comp != null)
                {
                    yield return comp.Religion.Def;
                    yield return comp.Religion.GroupTag;
                }
            }
        }

        public static void AppendReligious(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
        {
            if (thoughtsKind != PawnDiedOrDownedThoughtsKind.Died)
                return;

            AppendByInfo(new Info(victim, dinfo));
        }

        private static void AppendByInfo(Info info)
        {
                ReligionProperty weaponProperty = GetProperty(info.instigator, info.victim, SettingsTagDefOf.WeaponTag, info.weapon);
            AppendForPawn(info.instigator, info.victim, weaponProperty, true);

            foreach (ReligionProperty prop in GetProperties(info.instigator, info.victim, SettingsTagDefOf.KillTag, info.criteria))
                AppendForPawn(info.instigator, info.victim, prop, true);

            foreach(Pawn pawn in info.witnesses)
            {
                if(info.instigator != null)
                {
                    foreach (ReligionProperty prop in GetProperties(pawn, info.instigator, SettingsTagDefOf.KillTag, info.criteria))
                        AppendForPawn(pawn, info.instigator, prop, false);
                }
                else
                {
                    foreach (ReligionProperty prop in GetProperties(pawn, info.victim, SettingsTagDefOf.DeathTag, info.criteria))
                        AppendForPawn(pawn, info.victim, prop, false);
                }
            }
        }

        private static void AppendForPawn(Pawn pawn, Pawn otherPawm, ReligionProperty property, bool isSubject)
        {
            if(property != null)
            {
                if(isSubject)
                    PietyUtility.TryApplyOnPawn(property.Subject, pawn, otherPawm);
                else
                    PietyUtility.TryApplyOnPawn(property.Witness, pawn, otherPawm);
            }
        }

        private static IEnumerable<ReligionProperty> GetProperties(Pawn pawn, Pawn otherPawn, SettingsTagDef tag, IEnumerable<Def> defs)
        {
            foreach (Def def in defs)
                yield return GetProperty(pawn, otherPawn, tag, def);
        }

        private static ReligionProperty GetProperty(Pawn pawn, Pawn otherPawn, SettingsTagDef tag, Def def)
        {
            if(pawn != null && pawn.GetReligionComponent() != null)
            {
                ReligionSettings_Social settings = pawn.GetReligionComponent().Religion.FindByTag<ReligionSettings_Social>(tag);
                if (settings != null)
                    return settings.GetPropertyByObject(pawn, def, otherPawn);
            }
            return null;
        }
    }
}
