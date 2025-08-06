using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class NodeFinderCircle : MonoBehaviour
    {
        private readonly List<Node> _nearbyNodes = new List<Node>();
        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Node node = other.GetComponent<Node>();
            if (node != null && _nearbyNodes.Contains(node) == false)
            {
                _nearbyNodes.Add(node);
                OnEnemyEnterCircle?.Invoke(node);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Node node = other.GetComponent<Node>();
            if (node != null && _nearbyNodes.Contains(node))
            {
                _nearbyNodes.Remove(node);
                OnEnemyExitCircle?.Invoke(node);
            }
        }
        
        public List<Node> NearbyNodes => _nearbyNodes;
        public CircleCollider2D Collider => _collider;
        public Action<Node> OnEnemyEnterCircle;
        public Action<Node> OnEnemyExitCircle;
    }
}