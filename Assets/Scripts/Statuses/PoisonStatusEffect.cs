using Data;

namespace Statuses
{
    public class PoisonStatusEffect : StatusEffect
    {
        private readonly float _tickInterval;
        private float _damagePerTick;
        private float _criticalChance;
        private float _criticalMultiplier;
        private float _nextTickTime;
    
        public PoisonStatusEffect(StatusEffectData data, float tickInterval, float damagePerTick, 
            float criticalChance, float criticalMultiplier) : base(data)
        {
            _tickInterval = tickInterval;
            _damagePerTick = damagePerTick;
            _criticalChance = criticalChance;
            _criticalMultiplier = criticalMultiplier;
            _nextTickTime = tickInterval;
        }
    
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        
            if (!IsActive) return;
        
            _nextTickTime -= deltaTime;
            if (_nextTickTime <= 0)
            {
                ApplyDamage();
                _nextTickTime = _tickInterval;
            }
        }
    
        private void ApplyDamage()
        {
            /*var health = target.GetComponent<IHealth>();
            if (health != null)
            {
                bool isCrit = Random.value < critChance;
                float damage = isCrit ? damagePerTick * critMultiplier : damagePerTick;
                health.TakeDamage(damage, isCrit);
            }*/
        }
    }
}