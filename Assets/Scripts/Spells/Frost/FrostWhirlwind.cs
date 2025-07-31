using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.Frost
{
    public class FrostWhirlwind : ProjectileSpell
    {
        protected override SpellName SpellName => SpellName.FrostWhirlwind;
        protected override void OnReachTarget(ITargetable target)
        {
            ApplyDebuff(target as Enemy, StatusType.Slow);
            base.OnReachTarget(target);
        }
    }
}
