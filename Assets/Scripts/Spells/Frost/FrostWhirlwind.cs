using Data.Enums;
using EnemyStaff;

namespace Spells.Frost
{
    public class FrostWhirlwind : ProjectileSpell
    {
        public override SpellName SpellName => SpellName.FrostWhirlwind;
        protected override void OnReachTarget(ITargetble target)
        {
            ApplyDebuff(target as Enemy, StatusType.Slow);
            base.OnReachTarget(target);
        }
    }
}
