using UnityEngine;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Spells/ProjectileSpell", order = 52)]
    public class ProjectileSpellConfig : SpellConfig, ICast
    {
        [SerializeField] private Sprite _projectileSprite;
        public Sprite ProjectileSprite => _projectileSprite;
        
        [Header("Casting")]
        [SerializeField, Min(0.1f)] private float _castTime;
        [SerializeField, Min(0)] private float _cooldown;
        [SerializeField] private Color _castBarColor;
        
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
        
        [Header("Damaging")]
        [SerializeField, Min(0)] private float _damage;
        public float Damage => _damage;
        
        [SerializeField, Min(0)] private float _criticalChance;
        public float CriticalChance => _criticalChance;
        
        [SerializeField, Min(1)] private float _criticalMultiply;
        public float CriticalMultiply => _criticalMultiply;
    }
}