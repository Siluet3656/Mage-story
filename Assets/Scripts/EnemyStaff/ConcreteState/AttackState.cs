namespace EnemyStaff.ConcreteState
{
    public class AttackState : EnemyState
    {
        public AttackState(Enemy me, EnemyStateMachine enemyStateMachine) : base(me, enemyStateMachine)
        {
        }
        
        public override void EnterState()
        {
            Me.EnemyAttackInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            Me.EnemyAttackInstance.DoExitLogic();
        }

        public override void FrameUpdate(float deltaTime)
        {
            Me.EnemyAttackInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            Me.EnemyAttackInstance.DoPhysicsLogic();
        }

        public override void AnimationTriggerEvent()
        {
            Me.EnemyAttackInstance.DoAnimationTriggerEventLogic();
        }
    }
}
