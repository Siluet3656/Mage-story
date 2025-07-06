using Data.Enums;
using EnemyStaff;

namespace Spells.Fire
{
    public class FireMark : ProjectileSpell
    {
        protected override void OnReachTarget(ITargetble target)
        {
            ApplyDebuff(target as Enemy, StatusType.FireMark);
            base.OnReachTarget(target);
        }
    }
}
