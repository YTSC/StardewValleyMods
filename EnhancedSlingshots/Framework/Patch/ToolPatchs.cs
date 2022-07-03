using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;

namespace EnhancedSlingshots.Framework.Patch
{
	[HarmonyPatch(typeof(Tool))]
    public static class ToolPatchs
    {
        private static IMonitor Monitor;

        public static void Initialize(IMonitor monitor)
        {
            Monitor = monitor;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Tool.Forge))]
        public static void Forge_Postfix(Tool __instance, ref bool __result, Item item, bool count_towards_stats)
        {
            if (__instance is Slingshot sling && sling.InitialParentTileIndex == Slingshot.galaxySlingshot)
            {
                BaseEnchantment enchantment = BaseEnchantment.GetEnchantmentFromItem(__instance, item);
                if (enchantment is GalaxySoulEnchantment && sling.GetEnchantmentLevel<GalaxySoulEnchantment>() >= 3)
                {
                    __instance.IndexOfMenuItemView = __instance.InitialParentTileIndex = __instance.CurrentParentTileIndex = ModEntry.Instance.config.InfinitySlingshotId;
                    __instance.BaseName = "Infinity Slingshot";
                    __instance.DisplayName = ModEntry.Instance.i18n.Get("InfinitySlingshotName");
                    __instance.description = ModEntry.Instance.i18n.Get("InfinitySlingshotDescription");

                    GalaxySoulEnchantment enchant = __instance.GetEnchantmentOfType<GalaxySoulEnchantment>();
                    if (enchant != null) __instance.RemoveEnchantment(enchant);

                }
                if (count_towards_stats && !enchantment.IsForge())
                {
                    __instance.previousEnchantments.Insert(0, enchantment.GetName());
                    while (__instance.previousEnchantments.Count > 2)
                        __instance.previousEnchantments.RemoveAt(__instance.previousEnchantments.Count - 1);

                    Game1.stats.incrementStat("timesEnchanted", 1);
                }           
                __result = true;
                return;
            }
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(nameof(Tool.Forge))]
        //public static bool Forge_Prefix(Tool __instance, ref bool __result, Item item, bool count_towards_stats)
        //{
        //	BaseEnchantment enchantment = BaseEnchantment.GetEnchantmentFromItem(__instance, item);
        //	if (__instance is Slingshot sling && __instance.AddEnchantment(enchantment))
        //	{				
        //		if (sling.InitialParentTileIndex == Slingshot.galaxySlingshot && enchantment is GalaxySoulEnchantment && sling.GetEnchantmentLevel<GalaxySoulEnchantment>() >= 3)
        //		{
        //			__instance.IndexOfMenuItemView = __instance.InitialParentTileIndex = __instance.CurrentParentTileIndex = ModEntry.Instance.config.InfinitySlingshotId;
        //			__instance.BaseName = "Infinity Slingshot";
        //			__instance.DisplayName = ModEntry.Instance.i18n.Get("InfinitySlingshotName");
        //			__instance.description = ModEntry.Instance.i18n.Get("InfinitySlingshotDescription");

        //			GalaxySoulEnchantment enchant = __instance.GetEnchantmentOfType<GalaxySoulEnchantment>();
        //			if (enchant != null) __instance.RemoveEnchantment(enchant);
        //		}
        //		if (count_towards_stats && !enchantment.IsForge())
        //		{
        //			__instance.previousEnchantments.Insert(0, enchantment.GetName());
        //			while (__instance.previousEnchantments.Count > 2)
        //				__instance.previousEnchantments.RemoveAt(__instance.previousEnchantments.Count - 1);

        //			Game1.stats.incrementStat("timesEnchanted", 1);
        //		}
        //		__result = true;
        //		return false;
        //	}
        //	return true;
        //}

        [HarmonyPostfix]
        [HarmonyPatch(nameof(Tool.AddEnchantment))]
        public static void AddEnchantment_Postfix(Tool __instance, ref bool __result, Farmer ___lastUser, BaseEnchantment enchantment)
        {
            if (__instance is Slingshot && enchantment != null && enchantment.IsSecondaryEnchantment())
            {
                __instance.enchantments.Remove(enchantment);
                enchantment.UnapplyTo(__instance, ___lastUser);

                foreach (BaseEnchantment existing_enchantment in __instance.enchantments)
                {
                    if (existing_enchantment is GalaxySoulEnchantment)
                    {
                        if (existing_enchantment.GetMaximumLevel() < 0 || existing_enchantment.GetLevel() < existing_enchantment.GetMaximumLevel())
                        {
                            existing_enchantment.SetLevel(__instance, existing_enchantment.GetLevel() + 1);
                            __result = true;
                            return;
                        }
                        __result = false;
                        return;
                    }
                }
                __instance.enchantments.Add(enchantment);
                enchantment.ApplyTo(__instance, ___lastUser);
                __result = true;
                return;
            }

        }

        //[HarmonyPrefix]
        //[HarmonyPatch(nameof(Tool.AddEnchantment))]
        //public static bool AddEnchantment_Prefix(Tool __instance, ref bool __result, Farmer ___lastUser, BaseEnchantment enchantment)
        //      {
        //	if (__instance is Slingshot && enchantment != null && enchantment.IsSecondaryEnchantment())
        //          {
        //		foreach (BaseEnchantment existing_enchantment in __instance.enchantments)
        //		{
        //			if (existing_enchantment is GalaxySoulEnchantment)
        //			{
        //				if (existing_enchantment.GetMaximumLevel() < 0 || existing_enchantment.GetLevel() < existing_enchantment.GetMaximumLevel())
        //				{
        //					existing_enchantment.SetLevel(__instance, existing_enchantment.GetLevel() + 1);
        //					__result = true;
        //					return false;
        //				}
        //				__result = false;
        //				return false;
        //			}
        //		}
        //		__instance.enchantments.Add(enchantment);
        //		enchantment.ApplyTo(__instance, ___lastUser);
        //		__result = true;
        //		return false;
        //	}
        //	return true;
        //}
    }
}
