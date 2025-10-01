using UnityEngine;
using Data.Enums;
using EnemyStaff;
using Statuses;

namespace Spells.Frost
{
    [RequireComponent(typeof(StatusApplier))]
    public class FlashFreeze : AoeInstantSpell
    {
        private static readonly int IsFreeze = Animator.StringToHash("IsFreeze");
        
        private StatusApplier _statusApplier;
        protected override void Awake()
        {
            base.Awake();

            _statusApplier = GetComponent<StatusApplier>();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);
        
            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                        
                if (child.CompareTag("FlashFreezeEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        
        protected override void OnAnimationEnd()
        {
            Animator.SetBool(IsFreeze, false);
            
            base.OnAnimationEnd();
        }
        
        private void OnEnable()
        {
            AnimationEventCatcher.OnAnimationEnd += OnAnimationEnd;
        }

        private void OnDisable()
        {
            AnimationEventCatcher.OnAnimationEnd -= OnAnimationEnd;
        }
        
        public override void DoSpell()
        {
            base.DoSpell();
            Animator.SetBool(IsFreeze, true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyTargeting>(out _))
            {
                if (other.TryGetComponent(typeof(StatusController),out _))
                {
                    _statusApplier.ApplyStatusToTarget(StatusType.Slow, other.gameObject);
                }
            }
        }
    }
}
