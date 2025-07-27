using System;
using System.Collections.Generic;
using UnityEngine;
using EnemyStaff;

namespace PlayerStaff
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class TargetingCircle : MonoBehaviour
    {
        private readonly List<ITargetable> _nearbyTargets = new List<ITargetable>();
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ITargetable target = other.GetComponent<ITargetable>();
            if (target != null && _nearbyTargets.Contains(target) == false)
            {
                _nearbyTargets.Add(target);
                OnEnemyEnterCircle?.Invoke(target);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            ITargetable target = other.GetComponent<ITargetable>();
            if (target != null && _nearbyTargets.Contains(target))
            {
                _nearbyTargets.Remove(target);
                OnEnemyExitCircle?.Invoke(target);
            }
        }
        
        public List<ITargetable> NearbyTargets => _nearbyTargets;
        public CircleCollider2D Collider => _collider;
        public Action<ITargetable> OnEnemyEnterCircle;
        public Action<ITargetable> OnEnemyExitCircle;
    }
}
