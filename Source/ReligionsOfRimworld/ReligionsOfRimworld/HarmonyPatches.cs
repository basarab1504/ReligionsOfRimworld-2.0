using Harmony;
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
        private static class Patch_GetThoughts
        {
            private static void Prefix(World __instance)
            {
                ReligionManager.GetReligionManager().ExposeData();
            }
        }
    }
}
