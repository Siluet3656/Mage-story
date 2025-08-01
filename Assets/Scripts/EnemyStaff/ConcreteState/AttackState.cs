namespace EnemyStaff.ConcreteState
{
    public class AttackState : EnemyState
    {
        public AttackState(EnemyMovement enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }
    }
}
