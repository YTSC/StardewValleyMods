using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using HarmonyLib;
using System.Reflection;
using QualityFishPonds.Patch;
using System;

namespace QualityFishPonds
{
    public class ModEntry : Mod
    {
        public static string fishPondIdKey;  
        private Harmony harmony;
        public override void Entry(IModHelper helper)
        {        
            harmony = new(Helper.ModRegistry.ModID);
            fishPondIdKey = $"{Helper.ModRegistry.ModID}(FishPondID)";
            FishPondPatchs.Initialize(this.Monitor);
            FishingRodPatchs.Initialize(this.Monitor);
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Farm farm = Game1.getFarm();
            foreach(Building bulding in farm.buildings)
            {
                if(bulding is FishPond pond)
                {
                    if (!pond.modData.ContainsKey(fishPondIdKey))
                    {
                        string fishQualities = "";
                        if (pond.FishCount > 0)
                        {
                            fishQualities = "0";
                            for (int x = 0; x < pond.FishCount; x++)                            
                                fishQualities += "0";                            
                        }
                        pond.modData.Add(fishPondIdKey, fishQualities);                       
                    }
                }
            }
        }
    }
}
