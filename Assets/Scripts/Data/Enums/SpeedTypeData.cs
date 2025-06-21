using System.Collections.Generic;

public enum SpeedType
    {
        Slow,
        Medium,
        Fast
    }

public static class SpeedTypeData
{
    private static readonly Dictionary<SpeedType, float> SpeedValues = new Dictionary<SpeedType, float>
    {
        { SpeedType.Slow, 2f },
        { SpeedType.Medium, 6f },
        { SpeedType.Fast, 10f }
    };

    public static float GetDataByType(SpeedType id)
    {
        return SpeedValues.TryGetValue(id, out float speed) ? speed : 0f;
    }
}