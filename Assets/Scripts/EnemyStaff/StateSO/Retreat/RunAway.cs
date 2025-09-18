using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

namespace EnemyStaff.StateSO.Retreat
{
    [CreateAssetMenu(fileName = "RunAway", menuName = "Enemy logic/Retreat/RunAway", order = 30)]
    public class RunAway : EnemyRetreatSoBase
    {
        private Vector2 _playerPosition;
        private Vector2 _myPosition;
        private Node _nodeMeOn;
        private Node _nodeToRetreat;
        
        private List<Node> _path = new List<Node>();
        
        private Vector3 _moveDirection;
        private float _distanceToNode;
        
        private void FindFarthestPointFromPlayer()
        {
            _myPosition = Me.transform.position;
            _nodeMeOn = AStar.Instance.FindNearestNode(_myPosition);
            
            _playerPosition = PlayerTransform.position;
            _nodeToRetreat = AStar.Instance.FindFurthestNode(_playerPosition);
            
            _path = AStar.Instance.GeneratePath(_nodeMeOn, _nodeToRetreat);
        }
        
        private void MoveAlongPath()
        {
            _moveDirection = (_path.First().transform.position - Me.transform.position).normalized;
            _distanceToNode = Vector2.Distance(_path.First().transform.position, Me.transform.position);
                
            if (_distanceToNode > AStar.Instance.MinNodeDistance)
            {
                MyMovement.Move(_moveDirection);
            }
            else
            {
                _path.Remove(_path.First());
            }
        }

        public override void DoFrameUpdateLogic()
        {
            if (_path != null && _path.Count > 0)
            {
                MoveAlongPath();
            }
            else
            {
                FindFarthestPointFromPlayer();
            }
        }
    }
}
