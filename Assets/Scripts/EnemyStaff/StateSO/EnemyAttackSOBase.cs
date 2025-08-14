using UnityEngine;
using Data;
using EntityStaff;

namespace EnemyStaff.StateSO
{
    public class EnemyAttackSoBase : ScriptableObject
    {
        protected Enemy Me;
        protected Transform MyTransform;
        protected GameObject MyGameObject;
        protected EnemyAttack MyAttack;
        protected Hp MyHp;
        protected EnemyTargetingCircle AttackCircle;

        protected Transform PlayerTransform;
        
        protected int PlayerMask;
        protected int WallMask;

        protected float DefaultRadius;

        public virtual void Initialize(GameObject gameObject, Enemy enemy)
        {
            MyGameObject = gameObject;
            MyTransform = gameObject.transform;
            Me = enemy;
            MyAttack = gameObject.GetComponent<EnemyAttack>();
            MyHp = gameObject.GetComponent<Hp>();
            AttackCircle = Me.AttackCircle;

            PlayerTransform = G.Player.transform;
            
            PlayerMask = LayerMask.GetMask("Player");
            WallMask = LayerMask.GetMask("Walls");

            DefaultRadius = AttackCircle.Collider.radius;
        }
        
        public virtual void DoEnterLogic(){}
        public virtual void DoExitLogic(){}
        public virtual void DoFrameUpdateLogic(){}
        public virtual void DoPhysicsLogic(){}
        public virtual void DoAnimationTriggerEventLogic(){}

    }
}
