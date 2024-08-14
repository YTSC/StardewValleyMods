using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityFishPonds.Patch
{
    [HarmonyPatch(typeof(PondQueryMenu))]
    public class PondQueryMenuPatchs
    {
		private static IMonitor Monitor;
		public static void Initialize(IMonitor monitor)
		{
			Monitor = monitor;
		}

		[HarmonyPrefix]
		[HarmonyPatch(nameof(PondQueryMenu.draw))]
		public static void draw_Postfix(PondQueryMenu __instance, FishPond ____pond, float ____age, SpriteBatch b)
        {
            if (!Game1.globalFade)
            {				
				int totalSlots = ____pond.maxOccupants.Value;
				int slotsToDraw = ____pond.currentOccupants.Value;
				float slotSpacing = 13f;
				int x = 0, y = 0;
				
				for (int i = 0; i < slotsToDraw; i++)
				{
					float yOffset = (float)Math.Sin(____age * 1f + x * 0.75f + y * 0.25f) * 2f;
					var xPos = __instance.xPositionOnScreen + PondQueryMenu.width / 2 - slotSpacing * Math.Min(totalSlots, 5) * 4f * 0.5f + slotSpacing * 4f * x - 12f;
					var yPos = __instance.yPositionOnScreen + (int)(yOffset * 4f) + y * 4 * slotSpacing + 275.2f;
					Monitor.Log($"xPos: {xPos} - yPos: {yPos} - yOffset: {yOffset}", LogLevel.Info);
					x++;
					if (x == 5)
					{
						x = 0;
						y++;
					}

					Rectangle qualityRect = ____pond.modData[ModEntry.fishPondIdKey][i] switch
					{
						'1' => new Rectangle(338, 400, 8, 8),
						'2' => new Rectangle(346, 400, 8, 8),
						'4' => new Rectangle(346, 392, 8, 8),
						_ => new Rectangle(338, 392, 8, 8)
					};
					Monitor.Log($"qualityRect: {qualityRect}", LogLevel.Info);
					b.Draw(Game1.mouseCursors, new Vector2(xPos, yPos + yOffset +50f), qualityRect, Color.White, 0f, new Vector2(4f, 4f), 3f * 0.75f * (1f + yOffset), SpriteEffects.None, 0.9f);
				}
			}
            __instance.drawMouse(b);
        }
    }
}
