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
            Def objectDef = null;
            Pawn instigator = null;
            ThingDef weapon = null;

            if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died)
            {
                tagsToLook.Add(SettingsTagDefOf.DeathTag);

                if (victim.RaceProps.Humanlike)
                    objectDef = victim.GetReligionComponent().Religion.GroupTag;
                else
                    objectDef = victim.def;

                if (dinfo.HasValue)
                {
                    instigator = (Pawn)dinfo.Value.Instigator;
                    if (instigator != null)
                    {
                        tagsToLook.Add(SettingsTagDefOf.KillTag);
                        AppendThought_ForInstigator(instigator, objectDef, SettingsTagDefOf.KillTag);

                        weapon = dinfo.Value.Weapon;
                        if (weapon != null)
                            AppendThought_ForInstigator(instigator, weapon, SettingsTagDefOf.WeaponTag);
                    }
                }
                AppendThoughts_ForWitnesses(objectDef, victim, instigator);
            }
        }

        private static void AppendThoughts_ForWitnesses(Def objectDef, Pawn victim, Pawn instigator = null)
        {
            foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
            {
                if(Witnessed(pawn, victim) || RelationsUtility.PawnsKnowEachOther(pawn, victim))
                {
                    foreach (SettingsTagDef tag in tagsToLook)
                        AppendThought_ForWitness(pawn, objectDef, tag, instigator);
                }
            }
        }

        private static bool Witnessed(Pawn p, Pawn victim)
        {
            if (!p.Awake() || !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
                return false;
            if (victim.IsCaravanMember())
                return victim.GetCaravan() == p.GetCaravan();
            return victim.Spawned && p.Spawned && (p.Position.InHorDistOf(victim.Position, 12f) && GenSight.LineOfSight(victim.Position, p.Position, victim.Map, false, (Func<IntVec3, bool>)null, 0, 0));
        }

        private static void AppendThought_ForWitness(Pawn pawn, Def objectDef, SettingsTagDef tagDef, Pawn instigator = null)
        {
            ReligionSettings_Social settings = pawn.GetReligionComponent().Religion.FindByTag<ReligionSettings_Social>(tagDef);
            if (settings != null)
            {
                ReligionProperty property = settings.GetPropertyByObject(objectDef);
                PietyUtility.TryApplyReligionPropertySocial(pawn, property, instigator);
            }
        }

        private static void AppendThought_ForInstigator(Pawn pawn, Def objectDef, SettingsTagDef tagDef)
        {
            ReligionSettings_Social settings = pawn.GetReligionComponent().Religion.FindByTag<ReligionSettings_Social>(tagDef);
            if (settings != null)
            {
                ReligionProperty property = settings.GetPropertyByObject(objectDef);
                PietyUtility.TryApplyReligionPropertyIndividual(pawn, property);
            }
        }
    }
}
