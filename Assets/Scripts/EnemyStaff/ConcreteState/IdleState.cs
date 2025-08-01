namespace EnemyStaff.ConcreteState
{
    public class IdleState : EnemyState
    {
        private EnemyMovement _enemyMovement;
        public IdleState(EnemyMovement enemyMovement, EnemyStateMachine enemyStateMachine) : base(enemyMovement, enemyStateMachine)
        {
            _enemyMovement = enemyMovement;
        }

        public override void FrameUpdate()
        {
            
        }
    }
}
