﻿using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnhancedSlingshots.Framework.Enchantments
{
    //monster drops (1) more loot (stack with Burglar's Ring)
    [XmlType("Mods_ytsc_HunterEnchantment")]
    public class HunterEnchantment : BaseEnchantment
    {
        public override bool CanApplyTo(Item item)
        {
            return item is Slingshot;
        }

        public override string GetName()
        {
            return ModEntry.Instance.i18n.Get("HunterEnchantment");
        }
    }
}
