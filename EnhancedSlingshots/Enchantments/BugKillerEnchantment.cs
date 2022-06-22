using StardewValley;
using StardewValley.Monsters;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnhancedSlingshots.Enchantments
{
	//50% more damage to insect enemies
	[XmlType("Mods_ytsc_BugKillerEnchantment")]
	public class BugKillerEnchantment : BaseEnchantment
	{		
		public override bool CanApplyTo(Item item)
		{
			return item is Slingshot;
		}
		protected override void _OnDealDamage(Monster monster, GameLocation location, Farmer who, ref int amount)
		{
			if (monster is Grub || monster is Fly || monster is Bug || monster is Leaper || monster is LavaCrab || monster is RockCrab)
			{
				amount = (int)(amount * 1.5f);
			}
		}

		public override string GetName()
		{
			return ModEntry.Instance.i18n.Get("BugKillerEnchantment");
		}
	}
}
