using System.Collections.Generic;
using Data.Enums;

namespace Data
{
    public static class SpeedData
    {
        private static readonly Dictionary<SpeedType, float> SpeedValues = new Dictionary<SpeedType, float>
        {
            { SpeedType.Slow, 2f },
            { SpeedType.Medium, 6f },
            { SpeedType.Fast, 10f }
        };

        public static float GetDataByType(SpeedType type)
        {
            return SpeedValues.TryGetValue(type, out float speed) ? speed : 0f;
        }
    }
}
