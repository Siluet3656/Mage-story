using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Data;
using EntityStaff;
using Pathfinding;

namespace EnemyStaff.ConcreteState
{
    public class WanderingState : EnemyState
    {
        private readonly float _thresholdHpPercent = 0.15f;
        
        private readonly float _sightRange = 20f;
        private readonly float _yOffset = 0.5f;
        
        private readonly int _playerMask = LayerMask.GetMask("Player");
        private readonly int _wallMask = LayerMask.GetMask("Walls");
        
        private readonly EnemyMovement _myMovement;
        private readonly Transform _playerTransform;
        private readonly Hp _myHp;
       
        private List<Node> _path = new List<Node>();
        
        private Vector2 _moveDirection;
        private Vector2 _playerPosition;
        private Node _nodePlayerOn;
        private Vector2 _myPosition;
        private Node _nodeMeOn;
        private float _distanceToNode;
        private bool _isNeedPath;
        
        private void StartRetreat()
        {
            Me.StateMachine.ChangeState(Me.RetreatState);
        }
        
        private void StartAttack()
        {
            Me.StateMachine.ChangeState(Me.AttackState);
        }

        private void StartEngage()
        {
            Me.StateMachine.ChangeState(Me.EngageState);
        }
        
        private void GiveUp()
        {
            Me.StateMachine.ChangeState(Me.IdleState);
        }
        
        private bool CheckLineOfSight()
        {
            for (int i = 1; i <= 2; i++)
            {
                float offset = _yOffset * math.pow(-1, i);
                
                Vector2 startPosition = new Vector2(Me.transform.position.x, Me.transform.position.y + offset);
                Vector2 direction = new Vector2(_playerTransform.position.x, _playerTransform.position.y - _yOffset) - startPosition;
            
                RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, _sightRange, _playerMask | _wallMask);

                var isNeedToReturn = hit.collider && ((1 << hit.collider.gameObject.layer) & _playerMask) != 0;

                if (isNeedToReturn) return true;
            }

            return false;
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
        
        private void FindPathToPlayer()
        {
            _playerPosition = G.PlayersHp.transform.position;
            _nodePlayerOn = AStar.Instance.FindNearestNode(_playerPosition);
            _myPosition = Me.transform.position;
            _nodeMeOn = AStar.Instance.FindNearestNode(_myPosition);
            _path = AStar.Instance.GeneratePath(_nodeMeOn, _nodePlayerOn);
            
            if (_path != null && _path.Count > 0) _isNeedPath = false;
        }
        
        public WanderingState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _myMovement = me.GetComponent<EnemyMovement>();
            _myHp = me.GetComponent<Hp>();
            _playerTransform = G.PlayersHp.transform;
        }

        public override void EnterState()
        {
            _isNeedPath = true;
        }

        public override void FrameUpdate(float deltaTime)
        {
            if (_myHp.CurrentHealth < _myHp.MaxHealth * _thresholdHpPercent) StartRetreat();

            if (CheckLineOfSight())
            {
                if (Me.AttackCircle.NearbyPlayers.Count > 0) StartAttack();
                
                StartEngage();
            }

            if (_isNeedPath) FindPathToPlayer();
            
            if (_isNeedPath == false) MoveAlongPath();
            
            if (_path.Count == 0) GiveUp();
        }
    }
}