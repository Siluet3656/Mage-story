using UnityEngine;
using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.Fire
{
    public class FireMark : ProjectileSpell
    {
        protected override void Awake()
        {
            base.Awake();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("FireMarkEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        
        protected override void OnReachTarget(ITargetable target)
        {
            ApplyDebuff(target as EnemyTargeting, StatusType.FireMark);
            base.OnReachTarget(target);
        }
    }
}
