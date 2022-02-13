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
    //fire 2x faster
    [XmlType("Mods_ytsc_SwiftEnchantment")]
    public class SwiftEnchantment : BaseEnchantment
    {
        public override bool CanApplyTo(Item item)
        {
            if (item is Slingshot)
                return true;

            return false;
        }


        public override string GetName()
        {
            return ModEntry.Instance.i18n.Get("SwiftEnchantment");
        }
    }
}
