using StardewModdingAPI;

namespace EnhancedSlingshots
{
    class Config
    {
        public bool EnableGalaxySligshot { get; set; }
        public int GalaxySlingshotPrice { get; set; }
        public int InfinitySlingshotId { get; set; }       

        public Config()
        {
            EnableGalaxySligshot = true;
            GalaxySlingshotPrice = 75000;
            InfinitySlingshotId = 135;
        }
    }
}
