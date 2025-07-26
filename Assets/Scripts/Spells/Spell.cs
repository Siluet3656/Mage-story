using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityResources;
using PlayerStaff;
using Statuses;

namespace Spells
{
    [RequireComponent(typeof(StatusApplier))]
    public abstract class Spell : MonoBehaviour
    {
        private StatusApplier _statusApplier;
        private SpellResources _spellResources;
        
        protected float SpellDamage;
        protected float CriticalChance;
        protected float CriticalMultiply;

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

        protected abstract SpellName SpellName { get; set; }

        protected SpellResources SpellResources => _spellResources;
        public virtual void Initialize(SpellConfig config, float adjustedCriticalMultiply, float adjustedCriticalChance)
        {
            SpellName = config.SpellName;
            CriticalChance = adjustedCriticalChance;
            CriticalMultiply = adjustedCriticalMultiply;
        }

        public abstract void DoSpell();

        public void SetDamage(float damage)
        {
            SpellDamage = damage;
        }

        public void SetSpellResources(SpellResources resources)
        {
            _spellResources = resources;
        }
    }
}