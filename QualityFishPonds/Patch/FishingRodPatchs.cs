using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Tools;
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
        public static void pullFishFromWater_Prefix(FishingRod __instance, ref int fishQuality, bool fromFishPond)
        {
            if (!fromFishPond)
            {
                return;
            }

            Vector2 bobberTile = (Vector2)calculateBobberTileMethod.Invoke(__instance, new object[] { });

            Building building = Game1.getFarm().getBuildingAt(bobberTile);
            if (ModEntry.IsBuildingFishPond(building) && building.modData.ContainsKey(ModEntry.fishPondIdKey))
            {
                FishPond pond = (FishPond)building;
                string pondData = pond.modData[ModEntry.fishPondIdKey];
                fishQuality = int.Parse(pondData[0].ToString());
                pond.modData[ModEntry.fishPondIdKey] = pondData.Remove(0, 1);
                return;
            }

        }
    }
}