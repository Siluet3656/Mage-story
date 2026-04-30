using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Pathfinding
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private List<Node> _connections = new List<Node>();

        private void Awake()
        {
            G.NodesOnScene.Add(this);
        }
        
       private void OnDestroy()
        {
            G.NodesOnScene.Remove(this);
        }

        public List<Node> Connections => _connections;
        public Node _cameFrom;
        public float _gScore;
        public float _hScore;
        
        internal Node CameFrom
        {
            get => _cameFrom;
            set => _cameFrom = value;
        }
        
        internal float GScore
        {
            get => _gScore;
            set => _gScore = value;
        }
        
        internal float HScore
        {
            get => _hScore;
            set => _hScore = value;
        }
        
        public float FScore()
        {
            return _gScore + _hScore;
        }
        
        #region Debug

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            if (_connections.Count > 0)
            {
                foreach (Node connection in _connections)
                {
                    Gizmos.DrawLine(transform.position, connection.transform.position);
                }
            }
        }

        #endregion
    }
}