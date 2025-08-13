namespace EnemyStaff.ConcreteState
{
    public class IdleState : EnemyState
    {
        public IdleState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
        }

        public override void EnterState()
        {
            Me.EnemyIdleInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            Me.EnemyIdleInstance.DoExitLogic();
        }

        public override void FrameUpdate(float deltaTime)
        {
            Me.EnemyIdleInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            Me.EnemyIdleInstance.DoPhysicsLogic();
        }

        public override void AnimationTriggerEvent()
        {
            Me.EnemyIdleInstance.DoAnimationTriggerEventLogic();
        }
    }
}
