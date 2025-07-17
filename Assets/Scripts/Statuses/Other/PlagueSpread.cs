using System.Collections;
using System.Collections.Generic;
using Data.Enums;
using EnemyStaff;
using UnityEngine;

namespace Statuses.Other
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(StatusApplier))]
    public class PlagueSpread : MonoBehaviour
    {
        private StatusApplier _statusApplier;
        private readonly List<Enemy> _nearbyEnemies = new List<Enemy>();
        private GameObject _currentTarget;
        private void Awake()
        {
            _statusApplier = GetComponent<StatusApplier>();
            StartCoroutine(WaitForFramesAndApplyPlague(5));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
        
            if (enemy != null && other.gameObject != _currentTarget)
            {
                _nearbyEnemies.Add(enemy);
            }
        }
        
        private IEnumerator WaitForFramesAndApplyPlague(int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            ApplyPlague();
        }

        private void ApplyPlague()
        {
            if (_nearbyEnemies.Count > 0)
            {
                int number = Random.Range(0, _nearbyEnemies.Count);
                Enemy target = _nearbyEnemies[number];
            
                _statusApplier.ApplyStatusToTarget(StatusType.Plague, target.GameObject);
            }
            
            Destroy(gameObject); //Фабрика
        }

        public void SetTarget(GameObject target)
        {
            _currentTarget = target;
        }
    }
}
