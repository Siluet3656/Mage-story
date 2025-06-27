using Data;
using Data.Enums;
using Data.SpellConfigs;

namespace Spells.Fire
{
    public class Fireball : ProjectileSpell
    {
        protected override void OnReachTarget()
        {
            SpellConfig config = SpellData.Instance.GetSpellConfig(SpellName.Boom);
            Spell fireballExplosion = SpellFactory.Instance.CreateSpell(config);
            fireballExplosion.Initialize(config);
            fireballExplosion.gameObject.SetActive(true);
            base.OnReachTarget();
        }
    }
}
