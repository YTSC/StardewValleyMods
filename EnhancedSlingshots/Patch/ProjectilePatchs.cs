using EnhancedSlingshots.Enchantments;
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

        private static AccessTools.FieldRef<Projectile, NetPosition> position = AccessTools.FieldRefAccess<Projectile, NetPosition>("position");

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Projectile.isColliding))]
        public static void isColliding_Postfix(Projectile __instance, ref bool __result, GameLocation location)
        {
            if (Game1.player.CurrentTool is Slingshot sling && sling.hasEnchantmentOfType<MagneticEnchantment>())
            {
                Vector2 objectPosition = Vector2.Zero;
                foreach (var obj in location.objects.Pairs)
                {
                    if(obj.Value.getBoundingBox(obj.Key).Intersects(__instance.getBoundingBox()))
                    {
                        objectPosition = obj.Key;
                        break;
                    }
                }
                if (objectPosition != Vector2.Zero)
                {
                    if (ModEntry.Instance.config.MagneticEnchantmentStones.Contains(location.objects[objectPosition].ParentSheetIndex))
                    {
                        __result = true;
                        return;
                    }
                }
            }           
        }
    }
}
