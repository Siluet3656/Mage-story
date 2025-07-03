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
                fireballExplosion.transform.position = transform.position;
                fireballExplosion.Initialize(config);
                fireballExplosion.gameObject.SetActive(true);
                fireballExplosion.DoSpell();
            }
            
            base.OnReachTarget(target);
        }
    }
}