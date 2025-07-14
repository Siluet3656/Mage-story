using UnityEngine;
using Data;
using Data.Enums;
using Data.StatusConfigs;
using PlayerStaff;

namespace Statuses.Buffs
{
    public class FireAuraStatusEffect : StatusEffect
    {
        public FireAuraStatusEffect(StatusEffectData data) : base(data)
        {
        }
        
        public override void Apply(GameObject target)
        {
            base.Apply(target);
            
            if (target.TryGetComponent(out PlayerSpellCasting playerSpellCasting))
            {
                playerSpellCasting.AdjustCriticalDamage(SpellElementType.Fire,playerSpellCasting.AdjustedFireCriticalMultiply, playerSpellCasting.AdjustedFireCriticalChance * 2);
            } 
        }
    
        public override void Remove(GameObject target)
        {
            if (target.TryGetComponent(out PlayerSpellCasting playerSpellCasting))
            {
                playerSpellCasting.AdjustCriticalDamage(SpellElementType.Fire,playerSpellCasting.AdjustedFireCriticalMultiply, playerSpellCasting.AdjustedFireCriticalChance / 2);
            }
            
            base.Remove(target);
        }
    }
}
