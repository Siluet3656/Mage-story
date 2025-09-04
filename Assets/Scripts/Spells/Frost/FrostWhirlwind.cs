using UnityEngine;
using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.Frost
{
    public class FrostWhirlwind : ProjectileSpell
    {
        protected override SpellName SpellName => SpellName.FrostWhirlwind;

        protected override void Awake()
        {
            base.Awake();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("FrostWhirlwindEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        protected override void OnReachTarget(ITargetable target)
        {
            ApplyDebuff(target as EnemyTargeting, StatusType.Slow);
            base.OnReachTarget(target);
        }
    }
}
