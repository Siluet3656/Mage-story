namespace EnemyStaff
{
    public class EnemyState
    {
        protected EnemyMovement Enemy;
        protected EnemyStateMachine EnemyStateMachine;

        public EnemyState(EnemyMovement enemy, EnemyStateMachine enemyStateMachine)
        {
            Enemy = enemy;
            EnemyStateMachine = enemyStateMachine;
        }
        
        public virtual void EnterState() {}
        public virtual void ExitState() {}
        public virtual void FrameUpdate() {}
        public virtual void PhysicsUpdate(){}
        public virtual void AnimationTriggerEvent() {}
    }
}
