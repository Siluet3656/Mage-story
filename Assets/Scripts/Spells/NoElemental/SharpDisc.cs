using UnityEngine;
using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.NoElemental
{
    public class SharpDisc : ProjectileSpell
    {
        private static readonly int IsSharpDisc = Animator.StringToHash("isSharpDisc");

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
        }

        public override void DoSpell()
        {
            Animator.SetBool(IsSharpDisc, true);
            
            base.DoSpell();
        }

        protected override void OnReachTarget(ITargetable target)
        {
            if (target is EnemyTargeting enemy)
            {
                ApplyDebuff(enemy, StatusType.Bleed);
            }
            
            Animator.SetBool(IsSharpDisc, false);
            
            base.OnReachTarget(target);
        }
    }
}
