using Data;
using EnemyStaff;
using PlayerStaff;
using UnityEngine;

namespace Statuses.Debuffs
{
    public class SlowStatusEffect : StatusEffect
    {
        public SlowStatusEffect(StatusEffectData data) : base(data)
        {
        }

        public override void Apply(GameObject target)
        {
            base.Apply(target);
            if (target.TryGetComponent(out PlayerMovement player))
            {
                player.SetSpeed(player.GetAdjustedPlayerSpeed() - 1);
            }
            else if (target.TryGetComponent(out Enemy enemy))
            {
                enemy.SetSpeed(enemy.CurrentSpeed - 1);
            }
        }
    
        public override void Remove(GameObject target)
        {
            if (target.TryGetComponent(out PlayerMovement movement))
            {
                movement.SetSpeed(movement.GetAdjustedPlayerSpeed());
            }
            else if (target.TryGetComponent(out Enemy enemy))
            {
                enemy.SetSpeed(enemy.DefaultSpeed);
            }
            base.Remove(target);
        }
    }
}