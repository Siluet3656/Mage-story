using UnityEngine;
using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.Earth
{
    public class Spike : ProjectileSpell
    {
        protected override SpellName SpellName => SpellName.Spike;

        protected override void Awake()
        {
            base.Awake();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("SpikeWhirlwindEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        protected override void OnReachTarget(ITargetable target)
        {
            ApplyDebuff(target as EnemyTargeting, StatusType.Poison);
            base.OnReachTarget(target);
        }
    }
}
