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
    //extra damage to flying enemies
    [XmlType("Mods_ytsc_PreciseEnchantment")]
    public class PreciseEnchantment : BaseEnchantment
    {
        public override bool CanApplyTo(Item item)
        {
            return item is Slingshot;
        }
        protected override void _OnDealDamage(Monster monster, GameLocation location, Farmer who, ref int amount)
        {
            if (monster is Ghost || monster is Bat || monster is Fly || monster is Serpent || monster is BlueSquid || monster is Bug || monster is SquidKid)
            {
                amount = (int)(amount * 1.5f);
            }
        }

        public override string GetName()
        {
            return ModEntry.Instance.i18n.Get("PreciseEnchantment");
        }
    }
}
