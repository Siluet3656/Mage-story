using UnityEngine;
using EntityResources;

namespace Spells.Fire
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Explosion : AoeInstantSpell
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Hp otherHp = other.GetComponent<Hp>();
            if (otherHp != null)
            {
                otherHp.TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
            }
        }
    }
}
