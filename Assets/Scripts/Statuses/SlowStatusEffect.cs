using Data;
using PlayerStaff;
using UnityEngine;

namespace Statuses
{
    public class SlowStatusEffect : StatusEffect
    {
        private float _speedReduction;
    
        public SlowStatusEffect(StatusEffectData data, float speedReduction) : base(data)
        {
            _speedReduction = speedReduction;
        }
    
        public override void Apply(GameObject target)
        {
            base.Apply(target);
            if (target.TryGetComponent(out PlayerMovement movement))
            {
                movement.UpdateMovementSpeed(movement.GetAdjustedPlayerSpeed() - 1);
            }
            /*else if (target.TryGetComponent(out EnemyMovement movement))
            {
                movement.UpdateMovementSpeed(movement.GetAdjustedPlayerSpeed() - 1);
            }*/
        }
    
        public override void Remove(GameObject target)
        {
            if (target.TryGetComponent(out PlayerMovement movement))
            {
                movement.UpdateMovementSpeed(movement.GetAdjustedPlayerSpeed());
            }
            /*else if (target.TryGetComponent(out EnemyMovement movement))
            {
                movement.UpdateMovementSpeed(movement.GetAdjustedPlayerSpeed());
            }*/
            base.Remove(target);
        }
    }
}