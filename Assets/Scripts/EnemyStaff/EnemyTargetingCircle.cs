using System;
using System.Collections.Generic;
using UnityEngine;
using PlayerStaff;

namespace EnemyStaff
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class EnemyTargetingCircle : MonoBehaviour
    {
        private readonly List<PlayerMovement> _nearbyPlayers = new List<PlayerMovement>();
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerMovement target = other.GetComponent<PlayerMovement>();
            if (target != null && _nearbyPlayers.Contains(target) == false)
            {
                _nearbyPlayers.Add(target);
                OnPlayerEnterCircle?.Invoke(target);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerMovement target = other.GetComponent<PlayerMovement>();
            if (target != null && _nearbyPlayers.Contains(target))
            {
                _nearbyPlayers.Remove(target);
                OnPlayerExitCircle?.Invoke(target);
            }
        }
        
        public List<PlayerMovement> NearbyPlayers => _nearbyPlayers;
        public CircleCollider2D Collider => _collider;
        public Action<PlayerMovement> OnPlayerEnterCircle;
        public Action<PlayerMovement> OnPlayerExitCircle;
    }
}