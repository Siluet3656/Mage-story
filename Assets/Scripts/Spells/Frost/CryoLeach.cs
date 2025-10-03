using UnityEngine;
using Data.Enums;
using EnemyStaff;

namespace Spells.Frost
{
    public class CryoLeach : LineSpell
    {
        protected override void Awake()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("CryoLeachEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
            
            base.Awake();
        }
        
        public override void DoSpell()
        {
            ApplyDebuff(Target as EnemyTargeting, StatusType.StasisFreeze);
            
            base.DoSpell();
        }
    }
}
