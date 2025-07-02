using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;

namespace Spells.Fire
{
    public class Fireball : ProjectileSpell
    {
        protected override SpellName SpellName => SpellName.Fireball;
        protected override void OnReachTarget(ITargetble target)
        {
            SpellConfig config = SpellData.Instance.GetSpellConfig(SpellName.Boom);
            Spell fireballExplosion = SpellFactory.Instance.CreateSpell(SpellName.Boom);

            if (fireballExplosion != null)
            {
                fireballExplosion.Initialize(config);
                fireballExplosion.gameObject.SetActive(true);
            }
            
            base.OnReachTarget(target);
        }
    }
}