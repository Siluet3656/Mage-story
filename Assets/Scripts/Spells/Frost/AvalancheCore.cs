using System.Collections.Generic;
using Data;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityStaff;

namespace Spells.Frost
{
    public class AvalancheCore : ProjectileSpell
    {
        private readonly List<ITargetable> _nearbyTargets = new List<ITargetable>();

        public List<ITargetable> NearbyTargets => _nearbyTargets;
        
        protected override void Awake()
        {
            base.Awake();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("DefaultEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            ITargetable target = other.GetComponent<ITargetable>();
            if (target != null && _nearbyTargets.Contains(target) == false)
            {
                _nearbyTargets.Add(target);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            ITargetable target = other.GetComponent<ITargetable>();
            if (target != null && _nearbyTargets.Contains(target))
            {
                _nearbyTargets.Remove(target);
            }
        }
        protected override void OnReachTarget(ITargetable target)
        {
            float chunkDamage = SpellDamage / _nearbyTargets.Count;
            SpellConfig spellConfig = SpellData.Instance.GetSpellConfig(SpellName.AvalancheCoreChunk);
            
            foreach (var subtarget in _nearbyTargets)
            {
                if (subtarget != target)
                {
                    var chunk = SpellFactory.Instance.PoolSpell(SpellName.AvalancheCoreChunk);
                    
                    if (chunk != null && chunk is ProjectileSpell chunkProjectile)
                    {
                        chunkProjectile.Initialize(spellConfig, CriticalMultiply, CriticalChance);
                        chunkProjectile.TrySetTarget(subtarget);
                        chunkProjectile.SetDamage(chunkDamage);
                        chunkProjectile.transform.position = transform.position;
                        chunkProjectile.DoSpell();
                    }
                }
            }
            
            base.OnReachTarget(target);
        }
    }
}
