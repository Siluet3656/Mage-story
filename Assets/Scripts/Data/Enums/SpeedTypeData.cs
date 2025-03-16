using UnityEngine;

public enum SpeedType
    {
        Slow,
        Medium,
        Fast
    }

public class SpeedTypeData : MonoBehaviour
{
    const float slow = 2;
    const float medium = 6;
    const float fast = 10;
    public static float GetDataByID(SpeedType id)
    {
        switch (id)
        {
            case SpeedType.Slow:
                return slow;
            case SpeedType.Medium:
                return medium;
            case SpeedType.Fast:
                return fast;
        }
        return 0;
    }
}