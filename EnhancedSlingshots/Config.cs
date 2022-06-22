using StardewModdingAPI;

namespace EnhancedSlingshots
{
    class Config
    {
        public bool EnableGalaxySligshot { get; set; }
        public int GalaxySlingshotPrice { get; set; }
        public int InfinitySlingshotId { get; set; } 
        public int[] MagneticEnchantmentStones { get; set; }
        public Config()
        {
            EnableGalaxySligshot = true;
            GalaxySlingshotPrice = 75000;
            InfinitySlingshotId = 135;
            MagneticEnchantmentStones = new int[]
            {
                 2, //Diamond
                 4, //Ruby
                 6, //Jade
                 8, //Amethyst
                 10, //Topaz
                 12, //Emerald
                 14, //Aquamarine
                 44, //Gem Node
                 46, //Mystic Stone
                 75, //Geode
                 76, //Frozen Geode
                 77, //Magma Geode
                 95, //Radioactive ore
                 290, //Iron ore
                 751, //Copper ore
                 764, //Gold ore
                 765, //Iridium ore
                 819, //Omnigeode
                 843, //Cinder Shard
                 844, //Cinder Shard
                 849, //Copper ore (volcano version)                       
                 850 //Iron ore (volcano version)
            };
        }
    }
}
