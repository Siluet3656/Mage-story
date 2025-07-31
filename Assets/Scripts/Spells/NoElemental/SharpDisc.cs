using Data.Enums;
using EnemyStaff;
using EntityStaff;

namespace Spells.NoElemental
{
    public class SharpDisc : ProjectileSpell
    {
        protected override void OnReachTarget(ITargetable target)
        {
            if (target is Enemy enemy)
            {
                ApplyDebuff(enemy, StatusType.Bleed);
            }
            
            base.OnReachTarget(target);
        }
    }
}
