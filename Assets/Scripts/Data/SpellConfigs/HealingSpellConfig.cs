using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "HealingSpell", menuName = "Spells/HealingSpell", order = 52)]
    public class HealingSpellConfig : SpellConfig
    {
        [Header("Healing")] 
        [SerializeField] private float _healAmount;

        public float HealAmount => _healAmount;
        
        public override SpellType GetSPellType()
        {
            return SpellType.HealSpell;
        }
    }
}
