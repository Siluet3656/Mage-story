using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using UnityEngine;

namespace Spells.Earth
{
    public class Spike : ProjectileSpell
    {
        public override SpellName SpellName => SpellName.Spike;
        protected override void OnReachTarget(ITargetble target)
        {
            //SpellConfig config = SpellData.Instance.GetSpellConfig(SpellName.Boom);
            //Spell fireballExplosion = SpellFactory.Instance.CreateSpell(SpellName.Boom);
            Debug.Log("spike");
            //fireballExplosion.Initialize(config);
            //fireballExplosion.gameObject.SetActive(true);
            base.OnReachTarget(target);
        }
    }
}
