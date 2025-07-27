using UnityEngine;
using Data.StatusConfigs;
using PlayerStaff;

namespace Statuses.Debuffs
{
    public class QuickThinkingStatusEffect : StatusEffect
    {
        private readonly PlayerSpellCasting _playerSpellCasting;
        private readonly float _globalCooldownChange = 0.5f;
        public QuickThinkingStatusEffect(StatusEffectData data, GameObject objectEffectOn) : base(data)
        {
            _playerSpellCasting = objectEffectOn.GetComponent<PlayerSpellCasting>();
        }

        public override void Apply(GameObject target)
        {
            if (_playerSpellCasting != null)
            {
                float currentGlobalCooldown = _playerSpellCasting.CurrentGlobalCooldown;
                
                _playerSpellCasting.SetGlobalCooldown(currentGlobalCooldown - _globalCooldownChange);
            }
            
            base.Apply(target);
        }

        public override void Remove(GameObject target)
        {
            if (_playerSpellCasting != null)
            {
                float currentGlobalCooldown = _playerSpellCasting.CurrentGlobalCooldown;

                _playerSpellCasting.SetGlobalCooldown(currentGlobalCooldown + _globalCooldownChange);
            }

            base.Remove(target);
        }
    }
}
