using Data;
using UnityEngine;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityStaff;

namespace Spells.NoElemental
{
    public class SharpDisc : ProjectileSpell
    {

        private SpellConfig _config;
        
        protected override SpellName SpellName => SpellName.SharpDisk;

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
            MyAnimator.enabled = true;
            
            _config = SpellData.Instance.GetSpellConfig(SpellName);

            if (_config is ProjectileSpellConfig config)
            {
                MyRenderer.sprite = config.ProjectileSprite;
            }
            
            base.DoSpell();
            
            MyAnimator.Play("SharpDiscRotation", 0, 0f); 
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
