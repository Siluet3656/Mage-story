using System.Collections.Generic;
using System.Linq;
using Data;
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
        private NodeFinderCircle _nodeFinderCircle;

        private Vector2 _moveDirection;
        private float _distanceToNode;

        private List<Node> _path = new List<Node>();

        private readonly float _thresholdHpPercent = 0.15f;
        private readonly float _sightRange = 15f;
        private readonly float _offsetY = 0.4f;

        private readonly int _layerMask = LayerMask.GetMask("Player");
        
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

        private void MoveDirectlyToPlayer()
        {
            _moveDirection = (G.PlayersHp.transform.position - Me.transform.position).normalized;
            
            _myMovement.Move(_moveDirection);
        }

        private bool CheckLineOfSite()
        {
            Vector2 startPosition = new Vector2(Me.transform.position.x, Me.transform.position.y);
            Vector2 direction = new Vector2(G.PlayersHp.transform.position.x, G.PlayersHp.transform.position.y- _offsetY) - startPosition;
            
            RaycastHit2D hit = Physics2D.Raycast(Me.transform.position, direction, _sightRange, _layerMask);

            if (hit.collider != null)
            {
                Debug.Log("Препятствие на пути: " + hit.collider.name);
                
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("Цель в пределах видимости!");
                    return true;
                }
            }
            
            return false;
        }
        
        private void TryToFindPathToPlayer()
        {
            if (Me.EngageCircle.NearbyPlayers.Count > 0)
            {
                _playerPosition = G.PlayersHp.transform.position;
                _nodeFinderCircle = G.PlayersHp.gameObject.GetComponentInChildren<NodeFinderCircle>();
                _nodePlayerOn = AStar.Instance.FindNearestNode(_playerPosition, _nodeFinderCircle.NearbyNodes);
            
                _myPosition = Me.transform.position;
                _nodeFinderCircle = Me.GetComponentInChildren<NodeFinderCircle>();
                _nodeMeOn = AStar.Instance.FindNearestNode(_myPosition, _nodeFinderCircle.NearbyNodes);
            
                _path = AStar.Instance.GeneratePath(_nodeMeOn, _nodePlayerOn);
            }
            else
            {
                EndEngage();
            }
        }
        
        public EngageState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _myMovement = me.GetComponent<EnemyMovement>();
            _myHp = me.GetComponent<Hp>();
        }

        public override void EnterState()
        {
            Me.AttackCircle.OnPlayerEnterCircle += StartAttack;
            
            TryToFindPathToPlayer();
        }

        public override void ExitState()
        {
            Me.AttackCircle.OnPlayerEnterCircle -= StartAttack;
        }

        public override void FrameUpdate(float deltaTime)
        {
            if (_myHp.CurrentHealth < _myHp.MaxHealth * _thresholdHpPercent) StartRetreat();

            if (CheckLineOfSite())
            {
                MoveDirectlyToPlayer();
            }
            else
            {
                /*if (_path != null && _path.Count > 0)
                {
                    MoveAlongPath();
                }
                else
                {
                    TryToFindPathToPlayer();
                }*/
            }
        }
    }
}
