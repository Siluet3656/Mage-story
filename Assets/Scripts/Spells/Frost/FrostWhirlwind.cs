using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityStaff;

namespace Spells.Frost
{
    public class FrostWhirlwind : ProjectileSpell
    {
        private SpellConfig _config;
        
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
        
        public override void DoSpell()
        {
            _config = SpellData.Instance.GetSpellConfig(SpellName);

            if (_config is ProjectileSpellConfig config)
            {
                MyRenderer.sprite = config.ProjectileSprite;
            }
            
            base.DoSpell();
        }

        protected override void OnReachTarget(ITargetable target)
        {
            ApplyDebuff(target as EnemyTargeting, StatusType.Slow);
            base.OnReachTarget(target);
        }
    }
}
