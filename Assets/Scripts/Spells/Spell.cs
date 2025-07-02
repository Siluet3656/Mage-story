using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityResources;
using Statuses;
using UnityEngine;

namespace Spells
{
    [RequireComponent(typeof(StatusApplier))]
    public abstract class Spell : MonoBehaviour
    {
        protected float SpellDamage;
        protected float CriticalChance;
        protected float CriticalMultiply;

        private StatusApplier _statusApplier;

        protected virtual void Awake()
        {
            _statusApplier = GetComponent<StatusApplier>();
        }
        
        protected virtual void ApplyDamage(Hp targetHealth)
        {
            if (targetHealth != null)
            {
                targetHealth.TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
            }
        }

        protected virtual void ApplyDebuff(Enemy target, StatusType statusType)
        {
            if (target != null)
            {
                _statusApplier.ApplyStatusToTarget(statusType, target.gameObject);
            }
        }
        
        protected virtual void ReturnToPool()
        {
            SpellFactory.Instance.ReturnSpell(SpellName, this);
        }
        public abstract SpellName SpellName { get; protected set; }
        public abstract SpellType Type { get; }
        public virtual void Initialize(SpellConfig config)
        {
            SpellName = config.SpellName;
        }

        public abstract void DoSpell();

        public virtual void OnSpawned()
        {

        }

        public virtual void OnReturned()
        {

        }
    }
}