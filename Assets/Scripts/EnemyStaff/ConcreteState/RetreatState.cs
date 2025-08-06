using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data;
using Pathfinding;

namespace EnemyStaff.ConcreteState
{
    public class RetreatState : EnemyState
    {
        private readonly EnemyMovement _myMovement;
        
        private Vector2 _playerPosition;
        private Vector2 _myPosition;
        private Node _nodePlayerOn;
        private Node _nodeMeOn;
        private NodeFinderCircle _nodeFinderCircle;
        
        private List<Node> _path = new List<Node>();
        
        private Vector3 _moveDirection;
        private float _distanceToNode;

        private void FindFarthestPointFromPlayer()
        {
            _playerPosition = G.PlayersHp.transform.position;
            _nodeFinderCircle = G.PlayersHp.gameObject.GetComponentInChildren<NodeFinderCircle>();
            _nodePlayerOn = AStar.Instance.FindNearestNode(_playerPosition, _nodeFinderCircle.NearbyNodes);
            
            _myPosition = Me.transform.position;
            _nodeFinderCircle = Me.GetComponentInChildren<NodeFinderCircle>();
            _nodeMeOn = AStar.Instance.FindFurthestNode(_myPosition, _nodeFinderCircle.NearbyNodes);
            
            _path = AStar.Instance.GeneratePath(_nodeMeOn, _nodePlayerOn);
        }
        
        private void MoveAlongPath()
        {
            _moveDirection = (_path.First().transform.position - Me.transform.position).normalized;
            _distanceToNode = Vector2.Distance(_path.First().transform.position, Me.transform.position);
                
            if (_distanceToNode > AStar.Instance.MinNodeDistance)
            {
                _myMovement.Move(_moveDirection);
            }
            else
            {
                _path.Remove(_path.First());
            }
        }
        
        public RetreatState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
            _myMovement = enemy.GetComponent<EnemyMovement>();
        }

        public override void FrameUpdate(float deltaTime)
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
