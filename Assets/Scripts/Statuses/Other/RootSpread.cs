using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Enums;
using EnemyStaff;
using Statuses.Buffs;
using Statuses.Debuffs;

namespace Statuses.Other
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(StatusApplier))]
    public class RootSpread : MonoBehaviour
    {
        private StatusApplier _statusApplier;
        private readonly List<Enemy> _nearbyEnemies = new List<Enemy>();
        private GameObject _currentTarget;
        
        private RootCoreStatusEffect _rootCoreStatusEffect;
        private StatusController _statusController;
        
        private void Awake()
        {
            _statusApplier = GetComponent<StatusApplier>();
            StartCoroutine(WaitForFramesAndApplyRoots(5));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
        
            if (enemy != null && other.gameObject != _currentTarget)
            {
                _nearbyEnemies.Add(enemy);
            }
        }

        private IEnumerator WaitForFramesAndApplyRoots(int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            ApplyRoots();
        }
        
        private void ApplyRoots()
        {
            if (_nearbyEnemies.Count > 0)
            {
                foreach (var enemy in _nearbyEnemies)
                {
                    _statusApplier.ApplyStatusToTarget(StatusType.Root, enemy.GameObject);

                    _statusController = enemy.gameObject.GetComponent<StatusController>();
                    
                    _statusController.HasStatus(StatusType.Root, out StatusEffect statusEffect);

                    if (statusEffect is RootStatusEffect rootStatusEffect)
                    {
                        rootStatusEffect.SetRootCore(_rootCoreStatusEffect);
                        rootStatusEffect.Initialize();
                    }
                }
            }
            
            Destroy(gameObject); //Фабрика
        }
        
        public void Initialize(GameObject currentTarget, RootCoreStatusEffect rootCoreStatusEffect)
        {
            _currentTarget = currentTarget;

            _statusController = currentTarget.gameObject.GetComponent<StatusController>();
            
            _statusController.HasStatus(StatusType.Root, out StatusEffect statusEffect);
            
            _rootCoreStatusEffect =  rootCoreStatusEffect;
        }
    }
}
