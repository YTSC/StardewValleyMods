﻿using EnhancedSlingshots.Framework.Enchantments;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using StardewValley.Projectiles;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using SObject = StardewValley.Object;

namespace EnhancedSlingshots.Framework.Patch
{
    [HarmonyPatch(typeof(Projectile))]
    public static class ProjectilePatchs
    {
        private static IMonitor Monitor;

        public static void Initialize(IMonitor monitor)
        {
            Monitor = monitor;
        }
        
        private static AccessTools.FieldRef<Projectile, NetCharacterRef> theOneWhoFiredMe = AccessTools.FieldRefAccess<Projectile, NetCharacterRef>("theOneWhoFiredMe");
       
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Projectile.isColliding))]
        public static void isColliding_Postfix(Projectile __instance, ref bool __result, GameLocation location)
        {
            var who = theOneWhoFiredMe(__instance).Get(location);
            if (who is Farmer player && player.CurrentTool is Slingshot sling && sling.hasEnchantmentOfType<MagneticEnchantment>())
            {
                var result = location.objects.Pairs.FirstOrDefault(obj => obj.Value.getBoundingBox(obj.Key).Intersects(__instance.getBoundingBox()));
                if (default(KeyValuePair<Vector2, SObject>).Equals(result))
                    return;

                if(ModEntry.Instance.config.MagneticEnchantmentStones.Contains(result.Value.ParentSheetIndex))
                    __result = true;
                
                return;
            }           
        }

    }
}