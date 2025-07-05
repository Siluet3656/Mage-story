using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    [CreateAssetMenu(fileName = "SelfBuffSpellConfig", menuName = "Spells/SelfBuffSpellConfig", order = 52)]
    public class SelfBuffSpellConfig : SpellConfig
    {
        [Header("Buff")]
        [SerializeField] private StatusType _buffName;

        public StatusType Buff => _buffName;
        
        public override SpellType GetSPellType()
        {
            return SpellType.SelfInstantSpell;
        }
    }
}
