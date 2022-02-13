using StardewValley;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnhancedSlingshots.Enchantments
{
    //drops 1 more ore
    [XmlType("Mods_ytsc_MinerEnchantment")]
    public class MinerEnchantment : BaseEnchantment
    {
        public override bool CanApplyTo(Item item)
        {
            if (item is Slingshot)
                return true;

            return false;
        }


        public override string GetName()
        {
            return ModEntry.Instance.i18n.Get("MinerEnchantment");
        }
    }
}
