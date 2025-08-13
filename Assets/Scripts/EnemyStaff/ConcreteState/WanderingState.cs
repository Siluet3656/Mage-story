namespace EnemyStaff.ConcreteState
{
    public class WanderingState : EnemyState
    {
        public WanderingState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
        }
        
        public override void EnterState()
        {
            Me.EnemyWanderingInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            Me.EnemyWanderingInstance.DoExitLogic();
        }

        public override void FrameUpdate(float deltaTime)
        {
            Me.EnemyWanderingInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            Me.EnemyWanderingInstance.DoPhysicsLogic();
        }

        public override void AnimationTriggerEvent()
        {
            Me.EnemyWanderingInstance.DoAnimationTriggerEventLogic();
        }
    }
}