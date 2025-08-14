namespace EnemyStaff.ConcreteState
{
    public class RetreatState : EnemyState
    {
        public RetreatState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
        }
        
        public override void EnterState()
        {
            Me.EnemyRetreatInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            Me.EnemyRetreatInstance.DoExitLogic();
        }

        public override void FrameUpdate(float deltaTime)
        {
            Me.EnemyRetreatInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            Me.EnemyRetreatInstance.DoPhysicsLogic();
        }

        public override void AnimationTriggerEvent()
        {
            Me.EnemyRetreatInstance.DoAnimationTriggerEventLogic();
        }
    }
}
