using UnityEngine;

namespace Data.StatusConfigs
{
    [CreateAssetMenu(fileName = "TickingDamageStatusEffectData", menuName = "StatusEffects/TickingDamageStatusEffectData", order = 51)]
    public class TickingDamageStatusEffectData : StatusEffectData
    {
        [SerializeField] private float _tickInterval;
        [SerializeField] private float _damagePerTick;
        [SerializeField, Range(0f,1f)] private float _criticalChance;
        [SerializeField, Min(1f)] private float _criticalMultiplier;
        
        public float TickInterval => _tickInterval;
        public float DamagePerTick => _damagePerTick;
        public float CriticalChance => _criticalChance;
        public float CriticalMultiplier => _criticalMultiplier;
    }
}
