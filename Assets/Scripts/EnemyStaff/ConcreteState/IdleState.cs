using PlayerStaff;

namespace EnemyStaff.ConcreteState
{
    public class IdleState : EnemyState
    {
        private readonly EnemyMovement _myMovement;
        private readonly EnemyStateMachine _enemyStateMachine;

        private void StartEngage(PlayerMovement player)
        {
            _myMovement.StateMachine.ChangeState(_myMovement.EngageState);
        }
        
        public IdleState(EnemyMovement myMovement, EnemyStateMachine enemyStateMachine) : base(myMovement, enemyStateMachine)
        {
            _myMovement = myMovement;
            _enemyStateMachine = enemyStateMachine;
        }

        public override void EnterState()
        {
            _myMovement.EngageCircle.OnPlayerEnterCircle += StartEngage;
        }

        public override void ExitState()
        {
            _myMovement.EngageCircle.OnPlayerEnterCircle -= StartEngage;
        }
    }
}
