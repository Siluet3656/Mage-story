using UnityEngine;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;

namespace Spells.Fire
{
    public class Fireball : ProjectileSpell
    {
        public override SpellName SpellName => SpellName.Fireball;
        protected override void OnReachTarget(ITargetble target)
        {
            //SpellConfig config = SpellData.Instance.GetSpellConfig(SpellName.Boom);
            //Spell fireballExplosion = SpellFactory.Instance.CreateSpell(SpellName.Boom);
            Debug.Log("fireball");
            //fireballExplosion.Initialize(config);
            //fireballExplosion.gameObject.SetActive(true);
            base.OnReachTarget(target);
        }
    }
}