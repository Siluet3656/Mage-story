using Data.Enums;
using EnemyStaff;

namespace Spells.Earth
{
    public class Spike : ProjectileSpell
    {
        protected override SpellName SpellName => SpellName.Spike;
        protected override void OnReachTarget(ITargetble target)
        {
            ApplyDebuff(target as Enemy, StatusType.Poison);
            base.OnReachTarget(target);
        }
    }
}
