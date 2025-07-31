using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private List<Node> _connections = new List<Node>();
        
        public List<Node> Connections => _connections;
        public Node _cameFrom;
        public float _gScore;
        public float _hScore;
        
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
