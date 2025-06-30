using Data;
using Data.Enums;
using UnityEngine;

namespace Statuses
{
    public abstract class StatusEffect : IStatusEffect
    {
        private float _remainingDuration;
        private bool _isActive;
        private GameObject _target;
    
        public StatusType Type { get; protected set; }
        public StatusCategory Category { get; protected set; }
        public float Duration => _remainingDuration;
        public bool IsActive => _isActive;
    
        protected StatusEffect(StatusEffectData data)
        {
            Type = data.Type;
            Category = data.Category;
            _remainingDuration = data.BaseDuration;
        }
    
        public virtual void Apply(GameObject target)
        {
            this._target = target;
            _isActive = true;
        }
    
        public virtual void Remove(GameObject target)
        {
            _isActive = false;
        }
    
        public virtual void Update(float deltaTime)
        {
            if (!_isActive) return;
        
            _remainingDuration -= deltaTime;
            if (_remainingDuration <= 0)
            {
                Remove(_target);
            }
        }
    }
}