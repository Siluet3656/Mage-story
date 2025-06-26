using Data;
using Data.Enums;
using EntityResources;
using UnityEngine;

namespace Spells
{
    public abstract class Spell : MonoBehaviour
    {
        private float _spellDamage;
        private float _criticalChance;
        private float _criticalMultiply;
        
        protected abstract void Awake();
        
        protected virtual void Initialize(SpellConfig config)
        {
            _spellDamage = config.Damage;
            _criticalChance = config.CriticalChance;
            _criticalMultiply = config.CriticalMultiply;
        }
       
        protected virtual void ApplyDamage(Enemy target)
        {
            if (target != null)
            {
                target.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _criticalMultiply, _criticalChance);
            }
        }

        protected virtual void ApplyDebuff(Enemy target, DebuffType debuffType)
        {
            if (target != null)
            {
                target.GetComponent<Debuff>().DebuffTarget(debuffType, target);
            }
        }

        public abstract void DoSpell();

        protected virtual void ReturnToPool()
        {
            SpellFactory.Instance.ReturnSpell(this);
        }

        public virtual void OnSpawned()
        {

        }

        public virtual void OnReturned()
        {

        }
    }
}