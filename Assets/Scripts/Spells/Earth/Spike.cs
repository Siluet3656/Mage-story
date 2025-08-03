using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.Earth
{
    public class Spike : ProjectileSpell
    {
        protected override SpellName SpellName => SpellName.Spike;
        protected override void OnReachTarget(ITargetable target)
        {
            ApplyDebuff(target as EnemyTargeting, StatusType.Poison);
            base.OnReachTarget(target);
        }
    }
}
