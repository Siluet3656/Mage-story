using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "ShieldSpellConfig", menuName = "Spells/ShieldSpellConfig", order = 52)]
    public class ShieldSpellConfig : SpellConfig
    {
        [SerializeField] private ShieldType _shieldType;
        [SerializeField] private float _amount;

        public ShieldType ShieldType => _shieldType;
        public float Amount => _amount;
        public override SpellType GetSPellType()
        {
            return SpellType.ShieldSpell;
        }
    }
}
