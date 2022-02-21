using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Tools;
using System;

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

        [HarmonyPrefix]
        [HarmonyPatch(nameof(FishingRod.pullFishFromWater))]
        public static void pullFishFromWater_Prefix(FishingRod __instance, ref int fishQuality, bool fromFishPond)
        {
            if (fromFishPond)
            {
                var calculateBobberTileMethod = AccessTools.Method(typeof(FishingRod), "calculateBobberTile");
                Vector2 bobberTile = (Vector2)calculateBobberTileMethod.Invoke(__instance, new object[] { });

                Building building = Game1.getFarm().getBuildingAt(bobberTile);
                if ((building is FishPond || building.GetType().IsSubclassOf(typeof(FishPond))) && building.modData.ContainsKey(ModEntry.fishPondIdKey))
                {       
                    FishPond pond = (FishPond)building;                   
                    string pondData = pond.modData[ModEntry.fishPondIdKey];
                    int randomIndex = Game1.random.Next(pondData.Length);
                    fishQuality = int.Parse(pondData[randomIndex].ToString());
                    pond.modData[ModEntry.fishPondIdKey] = pondData.Remove(randomIndex, 1);
                    return;
                }
            }         
        }
    }
}