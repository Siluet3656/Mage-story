using UnityEngine;
public enum RangeType
    {
        Small,
        Medium,
        Large,
        Giant
    }

public class RangeTypeData : MonoBehaviour
{
    const float small = 2;
    const float medium = 4;
    const float large = 8;
    const float giant = 12;
    public static float GetDataByID(RangeType id)
    {
        switch (id)
        {
            case RangeType.Small:
                return small;
            case RangeType.Medium:
                return medium;
            case RangeType.Large:
                return large;
            case RangeType.Giant:
                return giant;
        }
        return 0;
    }
}