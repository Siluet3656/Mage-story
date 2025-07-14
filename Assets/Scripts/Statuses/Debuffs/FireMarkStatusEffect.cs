using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using Data.StatusConfigs;
using EntityResources;
using Spells;


namespace Statuses.Debuffs
{
    public class FireMarkStatusEffect : StatusEffect
    {
        private Hp _targetsHp; 
        public FireMarkStatusEffect(StatusEffectData data) : base(data)
        {
        }
        private void Explode()
        {
            SpellConfig config = SpellData.Instance.GetSpellConfig(SpellName.Explosion);
            Spell fireMarkExplosion = SpellFactory.Instance.PoolSpell(SpellName.Explosion);
            
            if (fireMarkExplosion != null)
            {
                fireMarkExplosion.transform.position = _targetsHp.gameObject.transform.position;
                fireMarkExplosion.Initialize(config, 1f, 0f);
                fireMarkExplosion.gameObject.SetActive(true);
                fireMarkExplosion.DoSpell();
            }
        }
        
        public override void Apply(GameObject target)
        {
            base.Apply(target);

            _targetsHp = target.GetComponent<Hp>();

            if (_targetsHp != null)
            {
                _targetsHp.OnCriticalDamageReceived += Explode;
            }
        }

        public override void Remove(GameObject target)
        {
            _targetsHp.OnCriticalDamageReceived -= Explode;
            
            base.Remove(target);
        }
    }
}
