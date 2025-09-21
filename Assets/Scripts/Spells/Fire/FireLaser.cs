using UnityEngine;
using EntityStaff;

namespace Spells.Fire
{
    public class FireLaser : LineSpell
    {
        private Hp _targetsHp;

        protected override void Awake()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("FireLaserEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
            
            base.Awake();
        }

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
