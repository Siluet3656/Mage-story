using UnityEngine;
using EntityStaff;

namespace Spells.Fire
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Explosion : AoeInstantSpell
    {
        private static readonly int IsExploding = Animator.StringToHash("isExploding");

        private void OnTriggerEnter2D(Collider2D other)
        {
            Hp otherHp = other.GetComponent<Hp>();
            if (otherHp != null)
            {
                otherHp.TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
            }
        }

        private void OnEnable()
        {
            AnimationEventCatcher.OnAnimationEnd += OnAnimationEnd;
        }

        private void OnDisable()
        {
            AnimationEventCatcher.OnAnimationEnd -= OnAnimationEnd;
        }

        protected override void Awake()
        {
            base.Awake();
                    
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);
        
            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                        
                if (child.CompareTag("ExplosionEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        protected override void OnAnimationEnd()
        {
            Animator.SetBool(IsExploding, false);
            
            base.OnAnimationEnd();
        }

        public override void DoSpell()
        {
            base.DoSpell();
            Animator.SetBool(IsExploding, true);
        }
    }
}
