using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EntityStaff;
using Pathfinding;
using PlayerStaff;

namespace EnemyStaff.ConcreteState
{
    public class EngageState : EnemyState
    {
        private readonly EnemyMovement _myMovement;
        private readonly Hp _myHp;
        
        private Vector2 _playerPosition;
        private Node _nodePlayerOn;
        private Vector2 _myPosition;
        private Node _nodeMeOn;

        private Vector2 _moveDirection;
        private float _distanceToNode;

        private List<Node> _path = new List<Node>();

        private readonly float _thresholdHpPercent = 0.1f;
        
        private void EndEngage()
        {
            Me.StateMachine.ChangeState(Me.IdleState);
        }

        private void StartRetreat()
        {
            Me.StateMachine.ChangeState(Me.RetreatState);
        }

        private void StartAttack(PlayerMovement player)
        {
            Me.StateMachine.ChangeState(Me.AttackState);
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
        
        public EngageState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _myMovement = me.GetComponent<EnemyMovement>();
            _myHp = me.GetComponent<Hp>();
        }

        private void FindPlayer()
        {
            _playerPosition = Object.FindObjectOfType<PlayerMovement>().transform.position;
            _myPosition = Me.transform.position;
            
            _nodePlayerOn = AStar.Instance.FindNearestNode(_playerPosition);
            _nodeMeOn = AStar.Instance.FindNearestNode(_myPosition);

            _path = AStar.Instance.GeneratePath(_nodeMeOn, _nodePlayerOn);
        }

        public override void EnterState()
        {
            Me.AttackCircle.OnPlayerEnterCircle += StartAttack;
            
            FindPlayer();
        }

        public override void ExitState()
        {
            Me.AttackCircle.OnPlayerEnterCircle -= StartAttack;
        }

        public override void FrameUpdate(float deltaTime)
        {
            if (_myHp.CurrentHealth < _myHp.MaxHealth * _thresholdHpPercent)
            {
                StartRetreat();
            }
            
            if (_path != null && _path.Count > 0)
            {
                MoveAlongPath();
            }
            else
            {
                if (Me.EngageCircle.NearbyPlayers.Count > 0)
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
