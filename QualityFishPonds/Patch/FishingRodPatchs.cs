using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley.Buildings;
using StardewValley.Tools;
using System;
using System.IO;
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

        [HarmonyPostfix]
        [HarmonyPatch("doPullFishFromWater")]
        public static void doPullFishFromWater_Postfix(FishingRod __instance, BinaryReader argReader)
        {
            argReader.BaseStream.Seek(0, SeekOrigin.Begin);
            _ = argReader.ReadString();
            _ = argReader.ReadInt32();
            _ = argReader.ReadInt32();
            _ = argReader.ReadInt32();
            _ = argReader.ReadBoolean();
            _ = argReader.ReadBoolean();
            bool fromFishPond = argReader.ReadBoolean();

            if (!fromFishPond)
            {
                return;
            }

            Vector2 bobberTile = (Vector2)calculateBobberTileMethod.Invoke(__instance, new object[] { });
            Building building = __instance.getLastFarmerToUse().currentLocation.getBuildingAt(bobberTile);

            if (ModEntry.IsBuildingFishPond(building) && building.modData.ContainsKey(ModEntry.FishPondIdKey))
            {
                FishPond pond = (FishPond)building;
                string pondData = pond.modData[ModEntry.FishPondIdKey];
                if (pondData?.Length > 0)
                {
                    __instance.fishQuality = Convert.ToInt32(pondData[pondData.Length - 1].ToString());
                    pond.modData[ModEntry.FishPondIdKey] = pondData.Remove(pondData.Length - 1);
                    return;
                }
            }
        }
    }
}