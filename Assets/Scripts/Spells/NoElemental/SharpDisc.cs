using Data.Enums;
using EnemyStaff;

namespace Spells.NoElemental
{
    public class SharpDisc : ProjectileSpell
    {
        protected override void OnReachTarget(ITargetble target)
        {
            if (target is Enemy enemy)
            {
                ApplyDebuff(enemy, StatusType.Bleed);
            }
            
            base.OnReachTarget(target);
        }
    }
}
