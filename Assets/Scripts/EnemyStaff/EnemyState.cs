namespace EnemyStaff
{
    public class EnemyState
    {
        protected readonly Enemy Me;
        protected EnemyStateMachine EnemyStateMachine;

        protected EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
        {
            Me = enemy;
            EnemyStateMachine = enemyStateMachine;
        }
        
        public virtual void EnterState() {}
        public virtual void ExitState() {}
        public virtual void FrameUpdate(float deltaTime) {}
        public virtual void PhysicsUpdate(){}
        public virtual void AnimationTriggerEvent() {}
    }
}
