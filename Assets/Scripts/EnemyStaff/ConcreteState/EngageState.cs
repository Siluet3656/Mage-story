using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using PlayerStaff;
using UnityEngine;

namespace EnemyStaff.ConcreteState
{
    public class EngageState : EnemyState
    {
        private readonly EnemyMovement _myMovement;
        
        private Vector2 _playerPosition;
        private Node _nodePlayerOn;
        
        private Vector2 _myPosition;
        private Node _nodeMeOn;

        private Vector2 _moveDirection;
        private float _distanceToNode;

        private List<Node> _path = new List<Node>();
        
        private void EndEngage()
        {
            _myMovement.StateMachine.ChangeState(_myMovement.IdleState);
        }

        private void StartAttack(PlayerMovement player)
        {
            _myMovement.StateMachine.ChangeState(_myMovement.AttackState);
        }
        
        public EngageState(EnemyMovement myMovement, EnemyStateMachine enemyStateMachine) : base(myMovement, enemyStateMachine)
        {
            _myMovement = myMovement;
        }

        private void FindPlayer()
        {
            _playerPosition = Object.FindObjectOfType<PlayerMovement>().transform.position;
            _myPosition = _myMovement.transform.position;
            
            _nodePlayerOn = AStar.Instance.FindNearestNode(_playerPosition);
            _nodeMeOn = AStar.Instance.FindNearestNode(_myPosition);

            _path = AStar.Instance.GeneratePath(_nodeMeOn, _nodePlayerOn);
        }

        public override void EnterState()
        {
            _myMovement.AttackCircle.OnPlayerEnterCircle += StartAttack;
            
            FindPlayer();
        }

        public override void ExitState()
        {
            _myMovement.AttackCircle.OnPlayerEnterCircle -= StartAttack;
        }

        public override void FrameUpdate()
        {
            if (_path != null && _path.Count > 0)
            {
                _moveDirection = (_path.First().transform.position - _myMovement.transform.position).normalized;
                _distanceToNode = Vector2.Distance(_path.First().transform.position, _myMovement.transform.position);
                
                if (_distanceToNode > AStar.Instance.MinNodeDistance)
                {
                    _myMovement.Move(_moveDirection);
                }
                else
                {
                    _path.Remove(_path.First());
                }
            }
            else
            {
                if (_myMovement.EngageCircle.NearbyPlayers.Count > 0)
                {
                    FindPlayer();
                }
                else
                {
                    EndEngage();
                }
            }
        }
    }
}
