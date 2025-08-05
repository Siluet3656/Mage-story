using EntityStaff;
using PlayerStaff;

namespace EnemyStaff.ConcreteState
{
    public class AttackState : EnemyState
    {
        private readonly EnemyAttack _enemyAttack;
        private readonly Hp _myHp;
        
        private readonly float _attackCooldownTime = 5f;
        private readonly float _attackDamage = 50f;
        private readonly float _thresholdHpPercent = 0.1f;
        
        private void StopAttack(PlayerMovement player)
        {
            Me.StateMachine.ChangeState(Me.EngageState);
        }
        
        private void StartRetreat()
        {
            Me.StateMachine.ChangeState(Me.RetreatState);
        }
        
        public AttackState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _enemyAttack = me.GetComponent<EnemyAttack>();
            _myHp = me.GetComponent<Hp>();
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
            if (_myHp.CurrentHealth < _myHp.MaxHealth * _thresholdHpPercent)
            {
                StartRetreat();
            }
            
            if (_enemyAttack.IsReadyToAttack)
            {
                _enemyAttack.PerformAttack(_attackDamage, _attackCooldownTime);
            }
        }
    }
}
