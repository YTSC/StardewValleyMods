﻿using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Tools;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnhancedSlingshots.Framework.Enchantments
{
    //chance of recover health on monster kill
    [XmlType("Mods_ytsc_VampiricEnchantment")]
    public class VampiricEnchantment : BaseEnchantment
    {
        public override bool CanApplyTo(Item item)
        {
            return item is Slingshot;           
        }

        protected override void _OnMonsterSlay(Monster m, GameLocation location, Farmer who)
        {
            Random random = new Random(DateTime.Now.Millisecond);           
            if (random.NextDouble() < ModEntry.Instance.config.VampiricEnchantment_RecoveryChance)
            {
                int amount = Math.Max(1, (int)((m.MaxHealth + random.Next(-m.MaxHealth / 10, m.MaxHealth / 15 + 1)) * 0.1f));
                who.health = Math.Min(who.maxHealth, Game1.player.health + amount);
                location.debris.Add(new Debris(amount, new Vector2(Game1.player.getStandingX(), Game1.player.getStandingY()), Color.Lime, 1f, who));
                Game1.playSound("healSound");
            }
        }

        public override string GetName()
        {
            return ModEntry.Instance.i18n.Get("VampiricEnchantment");
        }
    }
}
