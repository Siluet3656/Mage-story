using Data.Enums;
using EnemyStaff;

namespace Spells.Frost
{
    public class CryoLeach : LineSpell
    {
        public override void DoSpell()
        {
            ApplyDebuff(Target as Enemy, StatusType.StasisFreeze);
            
            base.DoSpell();
        }
    }
}
