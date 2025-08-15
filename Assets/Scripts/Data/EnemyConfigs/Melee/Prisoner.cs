using UnityEngine;
using Data.Enums;

namespace Data.EnemyConfigs.Melee
{
    [CreateAssetMenu(fileName = "Prisoner", menuName = "EnemyConfigs/Melee/Prisoner", order = 20)]
    public class Prisoner : EnemyConfig, IMelee
    {
        [Header("Melee attack")]
        [SerializeField] private float _attackRate;
        [SerializeField] private float _attackDamage;
        
        public override EnemyType GetEnemyType()
        {
            return EnemyType.Melee;
        }

        public float AttackRate => _attackRate;
        public float AttackDamage => _attackDamage;
    }
}
