using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "ShieldSpellConfig", menuName = "Spells/ShieldSpellConfig", order = 52)]
    public class ShieldSpellConfig : SpellConfig
    {
        [SerializeField] private ShieldType _shieldType;

        public ShieldType ShieldType => _shieldType;
        public override SpellType GetSPellType()
        {
            return SpellType.ShieldSpell;
        }
    }
}
