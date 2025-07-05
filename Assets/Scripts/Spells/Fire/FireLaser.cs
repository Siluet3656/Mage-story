using EntityResources;

namespace Spells.Fire
{
    public class FireLaser : LineSpell
    {
        private Hp _targetsHp;

        public override void DoSpell()
        {
            _targetsHp = Target.GameObject.GetComponent<Hp>();
            
            if (_targetsHp != null)
            {
                _targetsHp.TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
            }
            
            base.DoSpell();
        }
    }
}
