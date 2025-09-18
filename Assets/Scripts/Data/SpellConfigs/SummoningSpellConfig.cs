using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "SummonSpell", menuName = "Spells/SummonSpell", order = 52)]
    public class SummoningSpellConfig : SpellConfig, ICast, INeedPrefab
    {
        [Header("Summon")]
        [SerializeField] private Sprite _summonSprite;
        [SerializeField, Min(1)] private float _attackRadius;
        [SerializeField, Min(1)] private float _existingTime;
        [SerializeField] private bool _isNeedHp;
        [SerializeField] private bool _isTargetable;
        [SerializeField, Min(1)] private int _summonHp;
        [SerializeField, Min(0)] private float _summonSpeed;
        
        [Header("Casting")]
        [SerializeField, Min(0.1f)] private float _castTime;
        [SerializeField, Min(0)] private float _cooldown;
        [SerializeField] private Color _castBarColor;
        
        [Header("Damaging")]
        [SerializeField, Min(0)] private float _damage;
        
        public Sprite SummonSprite => _summonSprite;
        public float AttackRadius => _attackRadius;
        public float ExistingTime => _existingTime;
        
        public float Damage => _damage;
        public override SpellType GetSPellType() => SpellType.SummonSpell;

        public bool IsNeedHp => _isNeedHp;
        public bool IsTargetable => _isTargetable;
        public int SummonHp => _summonHp;
        public float SummonSpeed => _summonSpeed;
        
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
