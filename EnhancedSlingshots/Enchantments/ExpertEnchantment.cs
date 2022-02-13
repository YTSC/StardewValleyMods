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
    //auto reload
    [XmlType("Mods_ytsc_ExpertEnchantment")]
    public class ExpertEnchantment : BaseEnchantment
    {
        public override bool CanApplyTo(Item item)
        {
            if (item is Slingshot)
                return true;

            return false;
        }


        public override string GetName()
        {
            return ModEntry.Instance.i18n.Get("ExpertEnchantment");
        }
    }
}
