using UnityEngine;

namespace Spells.NoElemental
{
    public class MagicArrow : ProjectileSpell
    {
        protected override void Awake()
        {
            base.Awake();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("MagicArrowEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        
        public override void DoSpell()
        {
            SpellResources.RestoreLastSpentShard();
            base.DoSpell();
        }
    }
}
