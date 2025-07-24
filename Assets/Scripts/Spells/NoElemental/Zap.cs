using EntityResources;

namespace Spells.NoElemental
{
    public class Zap : LineSpell
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
