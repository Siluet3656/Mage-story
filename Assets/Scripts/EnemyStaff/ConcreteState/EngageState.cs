using UnityEngine;
using EntityStaff;
using Data;
using Unity.Mathematics;

namespace EnemyStaff.ConcreteState
{
    public class EngageState : EnemyState
    {
        private readonly EnemyMovement _myMovement;
        private readonly Hp _myHp;

        private Vector2 _moveDirection;
        
        private readonly Transform _playerTransform;

        private readonly float _thresholdHpPercent = 0.15f;
        
        private readonly float _sightRange = 20f;
        private readonly float _yOffset = 0.5f;
        
        private readonly int _playerMask = LayerMask.GetMask("Player");
        private readonly int _wallMask = LayerMask.GetMask("Walls");

        private void StartRetreat()
        {
            Me.StateMachine.ChangeState(Me.RetreatState);
        }

        private void StartWandering()
        {
            Me.StateMachine.ChangeState(Me.WanderingState);
        }
        
        private void StartAttack()
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
            bool toReturn = false;
            
            for (int i = 1; i <= 2; i++)
            {
                float offset = _yOffset * math.pow(-1, i);
                
                Vector2 startPosition = new Vector2(Me.transform.position.x, Me.transform.position.y - _yOffset);
                Vector2 direction = new Vector2(_playerTransform.position.x, _playerTransform.position.y - _yOffset) - startPosition;
            
                RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, _sightRange, _playerMask | _wallMask);

                toReturn = hit.collider && ((1 << hit.collider.gameObject.layer) & _playerMask) != 0;
            }

            return toReturn;
        }
        
        public EngageState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _myMovement = me.GetComponent<EnemyMovement>();
            _myHp = me.GetComponent<Hp>();
            _playerTransform = G.PlayersHp.transform;
        }

        public override void FrameUpdate(float deltaTime)
        {
            if (_myHp.CurrentHealth < _myHp.MaxHealth * _thresholdHpPercent) StartRetreat();
            
            if (Me.AttackCircle.NearbyPlayers.Count > 0) StartAttack();
            
            if (CheckLineOfSite())
            {
                MoveDirectlyToPlayer();
            }
            else
            {
                StartWandering();
            }
        }
    }
}
