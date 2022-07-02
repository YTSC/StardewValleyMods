using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;
using StardewValley.Menus;
using System.Reflection;
using StardewValley.Locations;
using Microsoft.Xna.Framework.Graphics;
using EnhancedSlingshots.Framework.Patch;
using EnhancedSlingshots.Framework.Enchantments;
using Microsoft.Xna.Framework;
using System;
using EnhancedSlingshots.Framework;
using System.Collections.Generic;

namespace EnhancedSlingshots
{
    public class ModEntry : Mod
    {
        internal ITranslationHelper i18n => Helper.Translation;
        internal Config config;
        private Harmony harmony;
        private Texture2D InfinitySlingTexture;
        private string EnchantmentsKey => $"{ModManifest.UniqueID}_ToolEnchantments";       
        private List<Tool> changedTools;
        public static ModEntry Instance;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            harmony = new Harmony(ModManifest.UniqueID);
            config = Helper.ReadConfig<Config>();
            InfinitySlingTexture = helper.ModContent.Load<Texture2D>("assets/InfinitySlingshot.png");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            InitializeMonitors();

            Helper.Events.Display.MenuChanged += OnMenuChanged;
            Helper.Events.GameLoop.Saving += OnSaving;
            Helper.Events.GameLoop.Saved += OnSaved;
            Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            Helper.Events.Content.AssetRequested += OnAssetRequested;            
        }

        public void OnSaved(object sender, SavedEventArgs e)
        {
            foreach (var tool in changedTools)
                LoadEnchantments(tool);
        }
        public void OnSaving(object sender, SavingEventArgs e)
        {
            changedTools = new();
            Utility.iterateAllItems(item =>
            {
                if (item is Tool tool)
                {
                    SaveEnchantments(tool);
                    changedTools.Add(tool);
                    //Monitor.Log($"Tool data: {tool.modData[EnchantmentsKey]}", LogLevel.Info);
                }
            });
        }

        public void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {            
            Utility.iterateAllItems(item =>
            {
                if (item is Tool tool && tool.modData.ContainsKey(EnchantmentsKey) && !String.IsNullOrEmpty(tool.modData[EnchantmentsKey]))
                    LoadEnchantments(tool);
            });
        }

        public void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (Game1.currentLocation is AdventureGuild && e?.NewMenu is ShopMenu shopMenu)
            {
                if (config.EnableGalaxySligshot && Game1.player.hasSkullKey)
                {
                    Tool slingshot = new Slingshot(Slingshot.galaxySlingshot);
                    shopMenu.itemPriceAndStock.Add(slingshot, new int[2] { config.GalaxySlingshotPrice, 1 });
                    shopMenu.forSale.Insert(0, slingshot);
                }
            }
        }

        public void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Weapons"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<int, string>().Data;
                    if (!data.ContainsKey(config.InfinitySlingshotId))
                        data[config.InfinitySlingshotId] = GetInfinitySlingshotData();
                    else 
                        throw new ArgumentException($"The ID for the Infinity Slinghot ({config.InfinitySlingshotId}) is beeing used! Please choose another ID.");
                });
            }

            if (e.Name.IsEquivalentTo("TileSheets/weapons"))
            {
                e.Edit(asset =>
                {
                    var weapons = asset.AsImage();
                    var area = GetTargetArea(config.InfinitySlingshotId);
                    if (weapons.Data.Height < area.Y + 16)
                        weapons.ExtendImage(minWidth: weapons.Data.Width, minHeight: area.Y + 16);

                    weapons.PatchImage(InfinitySlingTexture, targetArea: area);
                });
            }
        }

        private void LoadEnchantments(Tool tool)
        {
            var enchantments = tool.modData[EnchantmentsKey];
            //Monitor.Log($"Loading enchantments: {tool.Name} - {enchantments}", LogLevel.Info);
            foreach (var enchantmentKey in enchantments.Split(','))
            {
                var key = (EnchantmentKey)Enum.Parse(typeof(EnchantmentKey), enchantmentKey);
                ToolEnchantment.Enchantments.TryGetValue(key, out BaseEnchantment enchantment);
                if (enchantment != null)
                {
                    tool.enchantments.Add(enchantment);
                    enchantment.ApplyTo(tool, tool.getLastFarmerToUse());
                }
            }
        }
        private void SaveEnchantments(Tool tool)
        {
            //Monitor.Log($"Saving enchantments: {tool.Name}", LogLevel.Info);
            var toolEnchants = tool.enchantments;
            List<EnchantmentKey> enchantsKeys = new();
            foreach (var enchant in toolEnchants)
            {
                enchantsKeys.Add(ToolEnchantment.GetKeyByEnchantmentType(enchant));
                tool.enchantments.Remove(enchant);
                enchant.UnapplyTo(tool, tool.getLastFarmerToUse());
            }
            tool.modData[EnchantmentsKey] = String.Join(',', enchantsKeys);
        }

        private Rectangle GetTargetArea(int id)
        {
            return new ((id % 8) * 16, (id / 8) * 16, InfinitySlingTexture.Width, InfinitySlingTexture.Height);
        }

        private string GetInfinitySlingshotData()
        {
            return $"Infinity Slingshot/{i18n.Get("InfinitySlingshotDescription")}/1/3/1/308/0/0/4/-1/-1/0/.02/3/{i18n.Get("InfinitySlingshotName")}";
        }
        private void InitializeMonitors()
        {
            BaseEnchantmentPatchs.Initialize(Monitor);           
            GameLocationPatchs.Initialize(Monitor);
            ProjectilePatchs.Initialize(Monitor);
            SlingshotPatchs.Initialize(Monitor);
            ToolPatchs.Initialize(Monitor);
        }
    }
}
