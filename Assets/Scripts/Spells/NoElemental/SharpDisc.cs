using UnityEngine;
using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.NoElemental
{
    public class SharpDisc : ProjectileSpell
    {
        protected override void Awake()
        {
            base.Awake();

            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;

                if (child.CompareTag("SharpDiscEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
                
            Animator animator = GetComponent<Animator>();
            if (animator != null && !animator.enabled)
            {
                animator.enabled = true;
            }
        }
        
        protected override void OnReachTarget(ITargetable target)
        {
            if (target is EnemyTargeting enemy)
            {
                ApplyDebuff(enemy, StatusType.Bleed);
            }
            
            base.OnReachTarget(target);
        }
    }
}
