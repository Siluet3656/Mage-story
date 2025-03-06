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
    public static float GetDataByID(RangeType id)
    {
        const float small = 2;
        const float medium = 4;
        const float large = 8;
        const float giant = 12;
        switch (id)
        {
            case RangeType.Small:
                return small;
                break;
            case RangeType.Medium:
                return medium;
                break;
            case RangeType.Large:
                return large;
                break;
            case RangeType.Giant:
                return giant;
                break;
        }
        return 0;
    }
}