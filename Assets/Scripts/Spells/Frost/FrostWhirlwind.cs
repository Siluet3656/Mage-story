using Data.Enums;
using UnityEngine;

namespace Spells.Frost
{
    public class FrostWhirlwind : ProjectileSpell
    {
        public override SpellName SpellName => SpellName.FrostWhirlwind;
        protected override void OnReachTarget(Enemy target)
        {
            //ApplyDebuff(target, StatusType.Slow);
            base.OnReachTarget(target);
        }
    }
}
