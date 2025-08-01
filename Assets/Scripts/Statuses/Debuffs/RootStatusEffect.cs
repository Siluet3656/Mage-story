using Data.Enums;
using UnityEngine;
using Data.StatusConfigs;
using EnemyStaff;
using EntityStaff;
using Statuses.Buffs;

namespace Statuses.Debuffs
{
    public class RootStatusEffect : StatusEffect
    {
        private readonly EnemyMovement _enemyMovement;
        private readonly Hp _hp;

        private RootCoreStatusEffect _rootCoreStatusEffect;

        private void HealRootCore(float amount)
        {
            _rootCoreStatusEffect.Heal(amount);
        }
        
        public RootStatusEffect(StatusEffectData data, GameObject enemy) : base(data)
        {
            _enemyMovement = enemy.GetComponent<EnemyMovement>();
            
            if (_enemyMovement != null)
                _enemyMovement.SetMovementAvailability(false, MovementDisableSource.Roots);
            
            _hp = enemy.GetComponent<Hp>();
        }

        public override void Remove(GameObject target)
        {
            if (_enemyMovement != null)
                _enemyMovement.SetMovementAvailability(true,  MovementDisableSource.Roots);
            
            _hp.OnAnyDamageReceived -= HealRootCore;
            
            base.Remove(target);
        }

        public void SetRootCore(RootCoreStatusEffect statusEffect)
        {
            _rootCoreStatusEffect = statusEffect;
        }

        public void Initialize()
        { 
            if (_hp != null && _rootCoreStatusEffect != null)
                _hp.OnAnyDamageReceived += HealRootCore;
        }
    }
}
