using System.Collections.Generic;
using Data.Enums;

namespace Data
{
    public static class RangeData
    {
        private static readonly Dictionary<RangeType, float> RangeValues = new Dictionary<RangeType, float>
        {
            { RangeType.Small, 2f },
            { RangeType.Medium, 4f },
            { RangeType.Large, 8f },
            { RangeType.Giant, 12f}
        };
   
        public static float GetDataByType(RangeType type)
        {
            return RangeValues.TryGetValue(type, out float range) ? range : 0f;
        }
    }
}