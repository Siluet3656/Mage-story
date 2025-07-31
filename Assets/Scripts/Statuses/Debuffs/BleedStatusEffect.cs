using Data.StatusConfigs;
using EntityStaff;
using UnityEngine;

namespace Statuses.Debuffs
{
    public class BleedStatusEffect : StatusEffect
    {
        private readonly Hp _hp;
        private readonly float _additionalDamage = 10f;
        public BleedStatusEffect(StatusEffectData data, Hp hp) : base(data)
        {
            _hp = hp;
            _hp.OnAnyDamageReceived += DoAdditionalDamage;
        }

        public override void Remove(GameObject target)
        {
            _hp.OnAnyDamageReceived -= DoAdditionalDamage;
            
            base.Remove(target);
        }

        private void DoAdditionalDamage(float originDamage)
        {
            _hp.TryToTakeDamage(_additionalDamage, true);
        }
    }
}
