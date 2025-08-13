namespace EnemyStaff.ConcreteState
{
    public class EngageState : EnemyState
    {
        public EngageState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
        }
        
        public override void EnterState()
        {
            Me.EnemyEngageInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            Me.EnemyEngageInstance.DoExitLogic();
        }

        public override void FrameUpdate(float deltaTime)
        {
            Me.EnemyEngageInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            Me.EnemyEngageInstance.DoPhysicsLogic();
        }

        public override void AnimationTriggerEvent()
        {
            Me.EnemyEngageInstance.DoAnimationTriggerEventLogic();
        }
    }
}
