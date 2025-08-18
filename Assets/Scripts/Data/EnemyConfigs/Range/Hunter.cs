using UnityEngine;
using Data.Enums;

namespace Data.EnemyConfigs.Range
{
    [CreateAssetMenu(fileName = "Hunter", menuName = "EnemyConfigs/Range/Hunter", order = 20)]
    public class Hunter : EnemyConfig, IRange
    {
        [Header("Range attack")]
        [SerializeField] private float _attackRate;
        [SerializeField] private float _attackDamage;
        
        public override EnemyType GetEnemyType()
        {
            return EnemyType.Range;
        }

        public float AttackRate => _attackRate;
        public float AttackDamage => _attackDamage;
    }
}
