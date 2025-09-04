using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EntityStaff;

namespace Spells.Fire
{
    public class Fireball : ProjectileSpell
    {
        protected override SpellName SpellName => SpellName.Fireball;

        protected override void Awake()
        {
            base.Awake();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("FireballEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        protected override void OnReachTarget(ITargetable target)
        {
            SpellConfig config = SpellData.Instance.GetSpellConfig(SpellName.Explosion);
            Spell fireballExplosion = SpellFactory.Instance.PoolSpell(SpellName.Explosion);

            if (fireballExplosion != null)
            {
                fireballExplosion.transform.position = transform.position;
                fireballExplosion.Initialize(config, CriticalMultiply, CriticalChance);
                fireballExplosion.gameObject.SetActive(true);
                fireballExplosion.DoSpell();
            }
            
            base.OnReachTarget(target);
        }
    }
}