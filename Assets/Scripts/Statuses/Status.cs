using Data.Enums;
using UnityEngine;
using UnityEngine.Serialization;

public class Status : MonoBehaviour
{
    [FormerlySerializedAs("dt")] [SerializeField]
    public DebuffType _dt;
    [FormerlySerializedAs("bt")] [SerializeField]
    public BuffType _bt;
}
