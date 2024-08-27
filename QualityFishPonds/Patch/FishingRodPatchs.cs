using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley.Buildings;
using StardewValley.Tools;
using System;
using System.Reflection;

namespace QualityFishPonds.Patch
{
    [HarmonyPatch(typeof(FishingRod))]
    public static class FishingRodPatchs
    {
        private static IMonitor Monitor;
        public static void Initialize(IMonitor monitor)
        {
            Monitor = monitor;
        }

        private static MethodInfo calculateBobberTileMethod = AccessTools.Method(typeof(FishingRod), "calculateBobberTile");

        [HarmonyPrefix]
        [HarmonyPatch(nameof(FishingRod.pullFishFromWater))]
        public static void pullFishFromWater_Prefix()
        {
            Monitor.Log("pullFishFromWater_Prefix", LogLevel.Info);
        }

        [HarmonyDebug]
        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishingRod.pullFishFromWater))]
        public static void pullFishFromWater_Postfix(FishingRod __instance, bool fromFishPond)
        {
            Monitor.Log("pullFishFromWater_Postfix", LogLevel.Info);
            if (!fromFishPond)
            {
                return;
            }

            Vector2 bobberTile = (Vector2)calculateBobberTileMethod.Invoke(__instance, new object[] { });
            Building building = __instance.getLastFarmerToUse().currentLocation.getBuildingAt(bobberTile);

            if (ModEntry.IsBuildingFishPond(building) && building.modData.ContainsKey(ModEntry.fishPondIdKey))
            {
                FishPond pond = (FishPond)building;
                string pondData = pond.modData[ModEntry.fishPondIdKey];
                if (pondData?.Length > 0)
                {
                    __instance.fishQuality = Convert.ToInt32(pondData[pondData.Length - 1].ToString());
                    pond.modData[ModEntry.fishPondIdKey] = pondData.Remove(pondData.Length - 1);
                    return;
                }
            }
        }
    }
}