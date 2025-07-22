using Data.Enums;
using UnityEngine;
using Data.StatusConfigs;
using EnemyStaff;

namespace Statuses.Debuffs
{
    public class RootStatusEffect : StatusEffect
    {
        private readonly Enemy _enemy;
        public RootStatusEffect(StatusEffectData data, GameObject enemy) : base(data)
        {
            _enemy = enemy.GetComponent<Enemy>();
            
            if (_enemy != null)
                _enemy.SetMovementAvailability(false, MovementDisableSource.Roots);
        }

        public override void Remove(GameObject target)
        {
            if (_enemy != null)
                _enemy.SetMovementAvailability(true,  MovementDisableSource.Roots);
            
            base.Remove(target);
        }
    }
}
