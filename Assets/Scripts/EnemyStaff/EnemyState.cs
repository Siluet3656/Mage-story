namespace EnemyStaff
{
    public class EnemyState
    {
        protected EnemyMovement My;
        protected EnemyStateMachine EnemyStateMachine;

        public EnemyState(EnemyMovement my, EnemyStateMachine enemyStateMachine)
        {
            My = my;
            EnemyStateMachine = enemyStateMachine;
        }
        
        public virtual void EnterState() {}
        public virtual void ExitState() {}
        public virtual void FrameUpdate() {}
        public virtual void PhysicsUpdate(){}
        public virtual void AnimationTriggerEvent() {}
    }
}
