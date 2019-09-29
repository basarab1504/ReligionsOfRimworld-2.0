using Harmony;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace ReligionsOfRimworld
{
    [StaticConstructorOnStartup]
    internal static class HarmonyPatches
    {
        private static HarmonyInstance harmony = HarmonyInstance.Create("ReligionMod");

        [HarmonyPatch(typeof(World), "ExposeComponents")]
        private static class Patch_ExposeComponents
        {
            private static void Prefix(World __instance)
            {
                ReligionManager.GetReligionManager().ExposeData();
            }
        }

        [HarmonyPatch(typeof(Thing), "Ingested")]
        private static class Patch_Ingested
        {
            private static void Postfix(Pawn ingester, Thing __instance)
            {
                Religion_IngestingUtility.FoodIngested(ingester, __instance, __instance.def);
            }
        }

        [HarmonyPatch(typeof(PawnGenerator), "TryGenerateNewPawnInternal")]
        private static class Patch_TryGenerateNewPawnInternal
        {
            private static void Postfix(Pawn __result)
            {
                if (__result != null && __result.RaceProps.Humanlike)
                {
                    CompReligion religionComp = __result.GetReligionComponent();
                    religionComp.TryChangeReligion(religionComp.ReligionCompability.MostSuitableReligion());
                }
            }
        }

        [HarmonyPatch(typeof(Pawn), "Kill")]
        private static class Patch_PawnKill
        {
            private static void Postfix(Pawn __instance)
            {
                ActivityUtility.Notify_ColonistUnavailable(__instance);
            }
        }

        [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility), "GetThoughts")]
        private static class Patch_GetThoughts
        {
            private static void Postfix(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
            {
                Religion_PawnDiedOrDownedUtility.AppendThoughts_Religious(victim, dinfo, thoughtsKind);
            }
        }
    }
}
