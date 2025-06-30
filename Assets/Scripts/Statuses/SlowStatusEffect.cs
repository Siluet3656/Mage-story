using Data;
using UnityEngine;

namespace Statuses
{
    public class SlowStatusEffect : StatusEffect
    {
        private float _speedReduction;
    
        public SlowStatusEffect(StatusEffectData data, float speedReduction) : base(data)
        {
            _speedReduction = speedReduction;
        }
    
        public override void Apply(GameObject target)
        {
            /*base.Apply(target);
            var movement = target.GetComponent<IMovement>();
            if (movement != null)
            {
                movement.ModifySpeed(-speedReduction);
            }*/
        }
    
        public override void Remove(GameObject target)
        {
            /*var movement = target.GetComponent<IMovement>();
            if (movement != null)
            {
                movement.ResetSpeed();
            }*/
            base.Remove(target);
        }
    }
}