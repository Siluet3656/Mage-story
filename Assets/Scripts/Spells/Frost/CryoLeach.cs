using Data.Enums;
using EnemyStaff;

namespace Spells.Frost
{
    public class CryoLeach : LineSpell
    {
        public override void DoSpell()
        {
            ApplyDebuff(Target as EnemyTargeting, StatusType.StasisFreeze);
            
            base.DoSpell();
        }
    }
}
