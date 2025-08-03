using PlayerStaff;

namespace EnemyStaff.ConcreteState
{
    public class IdleState : EnemyState
    {
        private readonly EnemyStateMachine _enemyStateMachine;

        private void StartEngage(PlayerMovement player)
        {
            Me.StateMachine.ChangeState(Me.EngageState);
        }
        
        public IdleState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
            _enemyStateMachine = enemyStateMachine;
        }

        public override void EnterState()
        {
            Me.EngageCircle.OnPlayerEnterCircle += StartEngage;
        }

        public override void ExitState()
        {
            Me.EngageCircle.OnPlayerEnterCircle -= StartEngage;
        }
    }
}
