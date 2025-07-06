using Data;

namespace Statuses.Debuffs
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