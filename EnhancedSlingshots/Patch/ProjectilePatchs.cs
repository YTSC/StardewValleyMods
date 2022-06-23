using EnhancedSlingshots.Enchantments;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using StardewValley.Projectiles;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedSlingshots.Patch
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
                foreach (var obj in location.objects.Pairs)
                {
                    if(obj.Value.getBoundingBox(obj.Key).Intersects(__instance.getBoundingBox()) &&
                        ModEntry.Instance.config.MagneticEnchantmentStones.Contains(location.objects[obj.Key].ParentSheetIndex))
                    {
                        __result = true;
                        return;
                    }
                }               
            }           
        }

    }
}
