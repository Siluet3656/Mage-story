using UnityEngine;
using Data;

namespace EnemyStaff.StateSO
{
    public class EnemyRetreatSoBase : ScriptableObject
    {
        protected Enemy Me;
        protected Transform MyTransform;
        protected GameObject MyGameObject;
        protected EnemyMovement MyMovement;

        protected Transform PlayerTransform;
        
        protected int PlayerMask;
        protected int WallMask;
        
        public virtual void Initialize(GameObject gameObject, Enemy enemy)
        {
            MyGameObject = gameObject;
            MyTransform = gameObject.transform;
            Me = enemy;
            MyMovement = gameObject.GetComponent<EnemyMovement>();

            PlayerTransform = G.Player.transform;
            
            PlayerMask = LayerMask.GetMask("Player");
            WallMask = LayerMask.GetMask("Walls");
        }
        
        public virtual void DoEnterLogic(){}
        public virtual void DoExitLogic(){}
        public virtual void DoFrameUpdateLogic(){}
        public virtual void DoPhysicsLogic(){}
        public virtual void DoAnimationTriggerEventLogic(){}
    }
}
