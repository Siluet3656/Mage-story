using Data.Enums;
using UnityEngine;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Spells/ProjectileSpell", order = 52)]
    public class ProjectileSpellConfig : ScriptableObject, ISpellConfig
    {
        public SpellType Type
        {
            get => Type;
            private set => Type = SpellType.Projectile;
        }

        [Header("Sprites")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private  Sprite _spellSprite;
        
        [Header("Resources")]
        [SerializeField, Min(0)] private  Vector3Int _shardCost;
        [SerializeField, Min(0)] private  float _reminderCost;
        
        [Header("Casting")]
        [SerializeField, Min(0.1f)] private  float _castTime;
        [SerializeField, Min(0)] private  float _cooldown;
        [SerializeField] private  Color _castBarColor;
        private const bool RequireTarget = true;
        
        [Header("Damaging")]
        [SerializeField, Min(0)] private  float _damage;
        [SerializeField, Min(0)] private  float _criticalChance;
        [SerializeField, Min(1)] private  float _criticalMultiply;
    }
}
