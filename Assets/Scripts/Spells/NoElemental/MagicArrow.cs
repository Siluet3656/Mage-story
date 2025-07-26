namespace Spells.NoElemental
{
    public class MagicArrow : ProjectileSpell
    {
        public override void DoSpell()
        {
            SpellResources.RestoreLastSpentShard();
            base.DoSpell();
        }
    }
}
