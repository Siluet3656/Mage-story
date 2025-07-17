using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "InstantDebuffSpell", menuName = "Spells/InstantDebuffSpell", order = 52)]
    public class InstantDebuffSpellConfig : SpellConfig
    {
        [Header("Debuff")]
        [SerializeField] private StatusType _debuff;

        public StatusType Debuff => _debuff;
        public override SpellType GetSPellType()
        {
            return SpellType.InstantDebuffSpell;
        }
    }
}
