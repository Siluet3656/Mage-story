using Data;
using Data.Enums;
using EntityResources;
using UnityEngine;

namespace Spells
{
    public abstract class Spell : MonoBehaviour
    {
        protected float SpellDamage;
        protected float CriticalChance;
        protected float CriticalMultiply;
        
        protected abstract void Awake();
        
        protected virtual void ApplyDamage(Hp target)
        {
            if (target != null)
            {
                target.TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
            }
        }

        protected virtual void ApplyDebuff(Enemy target, DebuffType debuffType)
        {
            if (target != null)
            {
                target.GetComponent<Debuff>().DebuffTarget(debuffType, target);
            }
        }
        
        protected virtual void ReturnToPool()
        {
            SpellFactory.Instance.ReturnSpell(this);
        }

        public abstract void Initialize(SpellConfig config);


        public abstract void DoSpell();

        public virtual void OnSpawned()
        {

        }

        public virtual void OnReturned()
        {

        }
    }
}