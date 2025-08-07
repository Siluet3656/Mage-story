using Data;
using UnityEngine;
using EntityStaff;
using PlayerStaff;

namespace EnemyStaff.ConcreteState
{
    public class EngageState : EnemyState
    {
        private readonly EnemyMovement _myMovement;
        private readonly Hp _myHp;

        private Vector2 _moveDirection;
        
        private readonly Transform _playerTransform;
        private readonly int _playerLayer;

        private readonly float _thresholdHpPercent = 0.15f;
        private readonly float _sightRange = 15f;
        private readonly float _offsetY = 0.4f;

        private readonly int _layerMask = LayerMask.GetMask("Player")
                                        + LayerMask.GetMask("Walls");
        
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

        private void MoveDirectlyToPlayer()
        {
            _moveDirection = (_playerTransform.position - Me.transform.position).normalized;
            
            _myMovement.Move(_moveDirection);
        }

        private bool CheckLineOfSite()
        {
            Vector2 startPosition = new Vector2(Me.transform.position.x, Me.transform.position.y);
            Vector2 direction = new Vector2(_playerTransform .position.x, _playerTransform.position.y - _offsetY) - startPosition;
            
            RaycastHit2D hit = Physics2D.Raycast(Me.transform.position, direction, _sightRange, _layerMask);
            
            return hit.collider && hit.collider.gameObject.layer == _playerLayer;
        }
        
        public EngageState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _myMovement = me.GetComponent<EnemyMovement>();
            _myHp = me.GetComponent<Hp>();
            _playerTransform = G.PlayersHp.transform;
            _playerLayer = LayerMask.NameToLayer("Player");
        }

        public override void EnterState()
        {
            Me.AttackCircle.OnPlayerEnterCircle += StartAttack;
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
                EndEngage();
            }
        }
    }
}
