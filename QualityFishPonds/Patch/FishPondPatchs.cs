using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using System.Linq;
using SObject = StardewValley.Object;

namespace QualityFishPonds.Patch
{
    [HarmonyPatch(typeof(FishPond))]
    public static class FishPondPatchs
    {
        private static IMonitor Monitor;
        public static void Initialize(IMonitor monitor)
        {
            Monitor = monitor;
        }

        [HarmonyPostfix]
        [HarmonyPatch("addFishToPond")]
        public static void addFishToPond_Postfix(FishPond __instance, SObject fish)
        {
            if (__instance.modData.ContainsKey(ModEntry.fishPondIdKey))
                __instance.modData[ModEntry.fishPondIdKey] += fish.Quality;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.SpawnFish))]
        public static void SpawnFish_Postfix(FishPond __instance)
        {
            if (!__instance.hasSpawnedFish.Value || !__instance.modData.ContainsKey(ModEntry.fishPondIdKey))
            {
                return;
            }

            double dailyLuck = Game1.player.DailyLuck;
            string pondData = __instance.modData[ModEntry.fishPondIdKey];

            if (ModEntry.Instance.config.EnableGaranteedIridum && dailyLuck < -0.02)
            {
                pondData += "2";
                __instance.modData[ModEntry.fishPondIdKey] = pondData;
                return;
            }
            else if (ModEntry.Instance.config.EnableGaranteedIridum && pondData.Count(x => x == '4') == pondData.Count() && dailyLuck >= -0.02)
            {
                pondData += "4";
                __instance.modData[ModEntry.fishPondIdKey] = pondData;
                return;
            }
            else
            {
                double random = Game1.random.NextDouble();

                if (Game1.player.professions.Contains(8) && random < (pondData.Count(x => x == '4') * (Game1.player.LuckLevel / 10)) / (2 * pondData.Count(x => int.TryParse(x.ToString(), out int result) == true)))
                    pondData += "4";
                else if (random < 0.33)
                    pondData += "2";
                else if (random < 0.66)
                    pondData += "1";
                else
                    pondData += "0";

                __instance.modData[ModEntry.fishPondIdKey] = pondData;
                return;
            }


        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.ClearPond))]
        public static void ClearPond_Postfix(FishPond __instance)
        {
            __instance.modData[ModEntry.fishPondIdKey] = string.Empty;
        }


        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.dayUpdate))]
        public static void dayUpdate_Postfix(FishPond __instance)
        {
            if (!__instance.modData.ContainsKey(ModEntry.fishPondIdKey))
            {
                string fishQualities = new string('0', __instance.FishCount);
                __instance.modData.Add(ModEntry.fishPondIdKey, fishQualities);
                return;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.performActionOnConstruction))]
        public static void performActionOnConstruction_Postfix(FishPond __instance)
        {
            if (!__instance.modData.ContainsKey(ModEntry.fishPondIdKey))
                __instance.modData.Add(ModEntry.fishPondIdKey, string.Empty);

        }
    }
}
