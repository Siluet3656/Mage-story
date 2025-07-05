using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "LaserSpell", menuName = "Spells/LaserSpell", order = 52)]
    public class LaserSpellConfig : SpellConfig, ICast
    {
        [Header("Line")]
        [SerializeField] private Gradient _color;
        [SerializeField, Min(0.1f)] private float _duration;
        
        [Header("Casting")]
        [SerializeField, Min(0.1f)] private float _castTime;
        [SerializeField, Min(0)] private float _cooldown;
        [SerializeField] private Color _castBarColor;
        
        [Header("Damaging")]
        [SerializeField, Min(0)] private float _damage;
        [SerializeField, Range(0f, 1f)] private float _criticalChance;
        [SerializeField, Min(1)] private float _criticalMultiply;
        
        public override SpellType GetSPellType()
        {
            return SpellType.LineSpell;
        }
        
        public float Damage => _damage;
        public float CriticalChance => _criticalChance;
        public float CriticalMultiply => _criticalMultiply;

        public Gradient Color => _color;
        public float Duration => _duration;

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
