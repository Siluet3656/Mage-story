using UnityEngine;
using Data.Enums;

namespace Data.EnemyConfigs.Caster
{
    [CreateAssetMenu(fileName = "Paladin", menuName = "EnemyConfigs/Caster/Paladin", order = 20)]
    public class Paladin : EnemyConfig, ICaster
    {
        [Header("Cast")]
        [SerializeField] private float _castRate;
        [SerializeField] private float _castValue;
        public override EnemyType GetEnemyType()
        {
            return EnemyType.Caster;
        }

        public float CastRate => _castRate;
        public float CastValue => _castValue;
    }
}
