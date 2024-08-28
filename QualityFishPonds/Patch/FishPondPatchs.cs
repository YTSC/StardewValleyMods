using HarmonyLib;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using System;
using System.Linq;
using SObject = StardewValley.Object;

namespace QualityFishPonds.Patch
{
    [HarmonyPatch(typeof(FishPond))]
    public static class FishPondPatchs
    {
        private static bool IsClearingPond = false;
        private static IMonitor Monitor;
        public static void Initialize(IMonitor monitor)
        {
            Monitor = monitor;
        }

        [HarmonyPostfix]
        [HarmonyPatch("addFishToPond")]
        public static void addFishToPond_Postfix(FishPond __instance, SObject fish)
        {
            if (__instance.modData.ContainsKey(ModEntry.FishPondIdKey))
            {
                __instance.modData[ModEntry.FishPondIdKey] += fish.Quality;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.SpawnFish))]
        public static void SpawnFish_Postfix(FishPond __instance)
        {
            if (!__instance.hasSpawnedFish.Value || !__instance.modData.ContainsKey(ModEntry.FishPondIdKey))
            {
                return;
            }

            double dailyLuck = Game1.getOnlineFarmers().Max(farmer => farmer.DailyLuck);
            string pondData = __instance.modData[ModEntry.FishPondIdKey];

            if (ModEntry.Instance.config.EnableGaranteedIridum && dailyLuck < -0.02)
            {
                pondData += "2";
                __instance.modData[ModEntry.FishPondIdKey] = pondData;
                return;
            }
            else if (ModEntry.Instance.config.EnableGaranteedIridum && pondData.Count(x => x == '4') == pondData.Count() && dailyLuck >= -0.02)
            {
                pondData += "4";
                __instance.modData[ModEntry.FishPondIdKey] = pondData;
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

                __instance.modData[ModEntry.FishPondIdKey] = pondData;
                return;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.GetFishProduce))]
        public static void GetFishProduce_Postfix(FishPond __instance, ref Item __result)
        {
            if (!ModEntry.Instance.config.EnableQualityFishProduce
                || !__instance.modData.ContainsKey(ModEntry.FishPondIdKey)
                || __result is null)
            {
                return;
            }

            string pondData = __instance.modData[ModEntry.FishPondIdKey];
            if (pondData.Length > 0)
            {
                int index = new Random().Next(pondData.Length - 1);
                __result.Quality = Convert.ToInt32(pondData[index].ToString());
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("CreateFishInstance")]
        public static void CreateFishInstance_Postfix(FishPond __instance, NetString ___fishType, ref Item __result)
        {
            if (!__instance.modData.ContainsKey(ModEntry.FishPondIdKey) || !IsClearingPond)
            {
                return;
            }

            string pondData = __instance.modData[ModEntry.FishPondIdKey];
            if (pondData?.Length > 0)
            {
                int fishQuality = Convert.ToInt32(pondData[pondData.Length - 1].ToString());
                __instance.modData[ModEntry.FishPondIdKey] = pondData.Remove(pondData.Length - 1);
                __result = new SObject(___fishType.Value, 1, quality: fishQuality);
                return;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(FishPond.ClearPond))]
        public static void ClearPond_Prefix()
        {
            IsClearingPond = true;
        }


        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.ClearPond))]
        public static void ClearPond_Postfix(FishPond __instance)
        {
            __instance.modData[ModEntry.FishPondIdKey] = string.Empty;
            IsClearingPond = false;
        }


        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.dayUpdate))]
        public static void dayUpdate_Postfix(FishPond __instance)
        {
            if (!__instance.modData.ContainsKey(ModEntry.FishPondIdKey))
            {
                string fishQualities = new string('0', __instance.FishCount);
                __instance.modData.Add(ModEntry.FishPondIdKey, fishQualities);
                return;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(FishPond.performActionOnConstruction))]
        public static void performActionOnConstruction_Postfix(FishPond __instance)
        {
            if (!__instance.modData.ContainsKey(ModEntry.FishPondIdKey))
            {
                __instance.modData.Add(ModEntry.FishPondIdKey, string.Empty);
            }

        }
    }
}
