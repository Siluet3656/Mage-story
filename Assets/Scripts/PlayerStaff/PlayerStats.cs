namespace PlayerStaff
{
    public class PlayerStats
    {
        public float GlobalCooldown { get; private set; } = 0.5f;
        public float FireCritMultiplier { get; private set; } = 1f;
        public float FireCritChance { get; private set; } = 1f;
        public float FrostCritMultiplier { get; private set; } = 1f;
        public float FrostCritChance { get; private set; } = 1f;
        public float EarthCritMultiplier { get; private set; } = 1f;
        public float EarthCritChance { get; private set; } = 1f;
        public float NoElementCritMultiplier { get; private set; } = 1f;
        public float NoElementCritChance { get; private set; } = 1f;

        public void SetFireCritStats(float multiplier, float chance)
        {
            FireCritMultiplier = multiplier;
            FireCritChance = chance;
        }

        public void SetFrostCritStats(float multiplier, float chance)
        {
            FrostCritMultiplier = multiplier;
            FrostCritChance = chance;
        }

        public void SetEarthCritStats(float multiplier, float chance)
        {
            EarthCritMultiplier = multiplier;
            EarthCritChance = chance;
        }

        public void SetNoElementCritStats(float multiplier, float chance)
        {
            NoElementCritMultiplier = multiplier;
            NoElementCritChance = chance;
        }
    }
}