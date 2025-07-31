using Data.StatusConfigs;
using EntityStaff;
using UnityEngine;

namespace Statuses.Debuffs
{
    public class PoisonStatusEffect : StatusEffect
    {
        private readonly float _tickInterval;
        private readonly float _damagePerTick;
        private readonly float _criticalChance;
        private readonly float _criticalMultiplier;
        private readonly Hp _hp;
        
        private float _tickTime;
        
        private void DoDamage()
        {
            if (_hp != null)
            {
                _hp.TryToTakeCriticalDamage(_damagePerTick, _criticalMultiplier, _criticalChance);
            }
        }
        
        public PoisonStatusEffect(TickingDamageStatusEffectData data, Hp hp) : base(data)
        {
            _tickInterval = data.TickInterval;
            _damagePerTick = data.DamagePerTick;
            _criticalChance = data.CriticalChance;
            _criticalMultiplier = data.CriticalMultiplier;
            _hp = hp;
            
            _tickTime = _tickInterval;
        }

        public override void Update(float deltaTime)
        {
            _tickTime -= deltaTime;

            if (_tickTime <= 0)
            {
                _tickTime = _tickInterval;
                DoDamage();
            }
            
            base.Update(deltaTime);
        }
    }
}