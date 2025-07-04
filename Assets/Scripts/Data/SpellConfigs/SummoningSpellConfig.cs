using UnityEngine;
using Data.Enums;
using Spells;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "SummonSpell", menuName = "Spells/SummonSpell", order = 52)]
    public class SummoningSpellConfig : SpellConfig, ICast
    {
        [Header("Summon")]
        [SerializeField] private Sprite _summonSprite;
        [SerializeField, Min(1)] private float _attackRadius;
        [SerializeField, Min(1)] private float _existingTime;
        
        public Sprite SummonSprite => _summonSprite;
        public float AttackRadius => _attackRadius;
        public float ExistingTime => _existingTime;
        
        [Header("Casting")]
        [SerializeField, Min(0.1f)] private float _castTime;
        [SerializeField, Min(0)] private float _cooldown;
        [SerializeField] private Color _castBarColor;
        
        [Header("Damaging")]
        [SerializeField, Min(0)] private float _damage;
        [SerializeField, Range(0f, 1f)] private float _criticalChance;
        [SerializeField, Min(1)] private float _criticalMultiply;
        
        public float Damage => _damage;
        public float CriticalChance => _criticalChance;
        public float CriticalMultiply => _criticalMultiply;
        
        public override SpellType GetSPellType() => SpellType.SummonSpell;

        public float GetCastTime()
        {
            return _castTime;
        }

        public float GetCooldown()
        {
            return _cooldown;
        }

        public Color GetCastBarColor()
        {
            return _castBarColor;
        }
    }
}
