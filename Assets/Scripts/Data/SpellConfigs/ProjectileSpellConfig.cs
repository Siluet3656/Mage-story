using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Spells/ProjectileSpell", order = 52)]
    public class ProjectileSpellConfig : SpellConfig, ICast, INeedPrefab
    {
        [Header("Projectile")]
        [SerializeField] private Sprite _projectileSprite;
        
        [Header("Casting")]
        [SerializeField, Min(0.1f)] private float _castTime;
        [SerializeField, Min(0)] private float _cooldown;
        [SerializeField] private Color _castBarColor;
        
        [Header("Damaging")]
        [SerializeField, Min(0)] private float _damage;
        [SerializeField, Range(0f, 1f)] private float _criticalChance;
        [SerializeField, Min(1)] private float _criticalMultiply;
        
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
        
        public float Damage => _damage;
        public float CriticalChance => _criticalChance;
        public float CriticalMultiply => _criticalMultiply;
        
        public Sprite ProjectileSprite => _projectileSprite;
        
        public override SpellType GetSPellType() => SpellType.Projectile;
    }
}