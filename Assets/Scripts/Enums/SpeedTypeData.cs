using UnityEngine;

public enum SpeedType
    {
        Slow,
        Medium,
        Fast
    }

public class SpeedTypeData : MonoBehaviour
{
    public static float GetDataByID(SpeedType id)
    {
        const float slow = 2;
        const float medium = 6;
        const float fast = 10;
        switch (id)
        {
            case SpeedType.Slow:
                return slow;
                break;
            case SpeedType.Medium:
                return medium;
                break;
            case SpeedType.Fast:
                return fast;
                break;
        }
        return 0;
    }
}