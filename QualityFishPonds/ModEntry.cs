using HarmonyLib;
using QualityFishPonds.Patch;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using System.Reflection;

namespace QualityFishPonds
{
    public class ModEntry : Mod
    {
        public static ModEntry Instance;
        public static string fishPondIdKey;
        internal Config config;
        private Harmony harmony;
        public override void Entry(IModHelper helper)
        {
            Instance = this;

            harmony = new(Helper.ModRegistry.ModID);
            config = helper.ReadConfig<Config>();
            fishPondIdKey = $"{Helper.ModRegistry.ModID}(FishPondID)";
            FishPondPatchs.Initialize(Monitor);
            FishingRodPatchs.Initialize(Monitor);
            PondQueryMenuPatchs.Initialize(Monitor);
            Helper.Events.GameLoop.DayStarted += OnDayStarted;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            Farm farm = Game1.getFarm();
            foreach (Building building in farm.buildings)
            {
                if (!IsBuildingFishPond(building)) continue;

                FishPond pond = (FishPond)building;
                if (!pond.modData.ContainsKey(fishPondIdKey))
                {
                    string fishQualities = new string('0', pond.FishCount);
                    pond.modData.Add(fishPondIdKey, fishQualities);
                }

            }
        }

        public static bool IsBuildingFishPond(Building building)
        {
            return building is FishPond || building.GetType().IsSubclassOf(typeof(FishPond));
        }
    }
}
