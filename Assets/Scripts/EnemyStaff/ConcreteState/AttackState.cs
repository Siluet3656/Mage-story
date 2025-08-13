using UnityEngine;
using Unity.Mathematics;
using Data;
using EntityStaff;
using PlayerStaff;

namespace EnemyStaff.ConcreteState
{
    public class AttackState : EnemyState
    {
        private readonly EnemyAttack _enemyAttack;
        private readonly Hp _myHp;

        private readonly Transform _playerTransform;
        
        private readonly float _attackCooldownTime = 5f;
        private readonly float _attackDamage = 50f;
        private readonly float _thresholdHpPercent = 0.1f;
        
        private readonly float _sightRange = 20f;
        private readonly float _yOffset = 0.5f;
        
        private readonly int _playerMask = LayerMask.GetMask("Player");
        private readonly int _wallMask = LayerMask.GetMask("Walls");
        
        private void StopAttack(PlayerMovement player)
        {
            Me.StateMachine.ChangeState(Me.EngageState);
        }
        
        private void StartRetreat()
        {
            Me.StateMachine.ChangeState(Me.RetreatState);
        }
        
        private void StartWandering()
        {
            Me.StateMachine.ChangeState(Me.WanderingState);
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
        
        public AttackState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _enemyAttack = me.GetComponent<EnemyAttack>();
            _myHp = me.GetComponent<Hp>();
            _playerTransform = G.Player.transform;
        }
        
        public override void EnterState()
        {
            Me.AttackCircle.OnPlayerExitCircle += StopAttack;
        }

        public override void ExitState()
        {
            Me.AttackCircle.OnPlayerExitCircle -= StopAttack;
        }

        public override void FrameUpdate(float deltaTime)
        {
            if (_myHp.CurrentHealth < _myHp.MaxHealth * _thresholdHpPercent) StartRetreat();

            if (CheckLineOfSight() == false) StartWandering();
            
            if (_enemyAttack.IsReadyToAttack) _enemyAttack.PerformAttack(_attackDamage, _attackCooldownTime);
        }
    }
}
