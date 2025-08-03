using PlayerStaff;

namespace EnemyStaff.ConcreteState
{
    public class AttackState : EnemyState
    {
        private readonly EnemyAttack _enemyAttack;
        
        private readonly float _attackCooldownTime = 5f;
        private readonly float _attackDamage = 50f;
        
        private void StopAttack(PlayerMovement player)
        {
            Me.StateMachine.ChangeState(Me.EngageState);
        }
        
        public AttackState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _enemyAttack = me.GetComponent<EnemyAttack>();
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
            if (_enemyAttack.IsReadyToAttack)
            {
                _enemyAttack.PerformAttack(_attackDamage, _attackCooldownTime);
            }
        }
    }
}
