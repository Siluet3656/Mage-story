using UnityEngine;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "InstantSpell", menuName = "Spells/InstantSpell", order = 52)]
    public class InstantSpellConfig : SpellConfig
    {
        [Header("Instant Cast")]
        [SerializeField] private Sprite _castSprite;
        [SerializeField] private float _existTime;
        
        public Sprite CastSprite => _castSprite;
        public float ExistTime => _existTime;
        
        [Header("Damaging")]
        [SerializeField, Min(0)] private float _damage;
        [SerializeField, Range(0f, 1f)] private float _criticalChance;
        [SerializeField, Min(1)] private float _criticalMultiply;
        public float Damage => _damage;
        public float CriticalChance => _criticalChance;
        public float CriticalMultiply => _criticalMultiply;
    }
}
