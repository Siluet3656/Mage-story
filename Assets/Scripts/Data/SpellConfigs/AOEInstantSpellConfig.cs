using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "AOEInstantSpell", menuName = "Spells/AOEInstantSpell", order = 52)]
    public class AoeInstantSpellConfig : SpellConfig, INeedPrefab
    {
        [Header("AOE Instant Cast")]
        [SerializeField] private Sprite _castSprite;
        [SerializeField] private float _existTime;
        [SerializeField] private RangeType _range;
        
        [Header("Damaging")]
        [SerializeField, Min(0)] private float _damage;
        
        public Sprite CastSprite => _castSprite;
        public float ExistTime => _existTime;
        public RangeType Range => _range;
        public float Damage => _damage;
        
        public override SpellType GetSPellType()
        {
            return SpellType.AoeInstantSpell;
        }
    }
}
