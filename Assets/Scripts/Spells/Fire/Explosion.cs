using System.Collections.Generic;
using EntityResources;
using UnityEngine;

namespace Spells.Fire
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Explosion : InstantSpell
    {
        private List<Hp> ToDamage = new List<Hp>();
        private void OnTriggerEnter2D(Collider2D other)
        {
            Hp otherHp = other.GetComponent<Hp>();
            if (otherHp != null)
            {
                ToDamage.Add(otherHp);
            }
        }

        public override void DoSpell()
        {
            foreach (var entityHp in ToDamage)
            {
                entityHp.TryToTakeCriticalDamage(SpellDamage, CriticalMultiply, CriticalChance);
            }
            base.DoSpell();
        }
    }
}
