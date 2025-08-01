using PlayerStaff;

namespace EnemyStaff.ConcreteState
{
    public class AttackState : EnemyState
    {
        private readonly EnemyMovement _myMovement;
        
        private void StopAttack(PlayerMovement player)
        {
            _myMovement.StateMachine.ChangeState(_myMovement.EngageState);
        }
        
        public AttackState(EnemyMovement myMovement, EnemyStateMachine enemyStateMachine) : base(myMovement, enemyStateMachine)
        {
            _myMovement = myMovement;
        }
        
        public override void EnterState()
        {
            _myMovement.AttackCircle.OnPlayerExitCircle += StopAttack;
        }

        public override void ExitState()
        {
            _myMovement.AttackCircle.OnPlayerExitCircle -= StopAttack;
        }
    }
}
