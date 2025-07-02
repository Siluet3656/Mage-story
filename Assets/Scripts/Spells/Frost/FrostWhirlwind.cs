using Data.Enums;
using EnemyStaff;
using UnityEngine;

namespace Spells.Frost
{
    public class FrostWhirlwind : ProjectileSpell
    {
        public override SpellName SpellName => SpellName.FrostWhirlwind;
        protected override void OnReachTarget(ITargetble target)
        {
            //ApplyDebuff(target, StatusType.Slow);
            base.OnReachTarget(target);
        }
    }
}
