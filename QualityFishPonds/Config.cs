namespace QualityFishPonds
{
    class Config
    {
        public bool EnableGaranteedIridum { get; set; }
        public bool EnableQualityFishProduce { get; set; }

        public Config()
        {
            EnableGaranteedIridum = true;
            EnableQualityFishProduce = true;
        }
    }
}
