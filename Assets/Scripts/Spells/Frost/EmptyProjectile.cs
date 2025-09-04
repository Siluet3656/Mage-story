using UnityEngine;

namespace Spells.Frost
{
    public class EmptyProjectile : ProjectileSpell
    {
        protected override void Awake()
        {
            base.Awake();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("DefaultEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }
}
