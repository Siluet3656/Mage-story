using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.Fire
{
    public class FireMark : ProjectileSpell
    {
        protected override void OnReachTarget(ITargetable target)
        {
            ApplyDebuff(target as Enemy, StatusType.FireMark);
            base.OnReachTarget(target);
        }
    }
}
