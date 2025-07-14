using UnityEngine;
using Data.Enums;
using Data.StatusConfigs;

namespace Statuses
{
    public abstract class StatusEffect : IStatusEffect
    {
        private readonly float _baseDuration;
        
        private float _remainingDuration;
        
        private bool _isActive;
        private GameObject _target;
    
        protected StatusEffect(StatusEffectData data)
        {
            Type = data.Type;
            Category = data.Category;
            _baseDuration = data.BaseDuration;

            RefreshDuration();
        }
        
        public StatusType Type { get; protected set; }
        public StatusCategory Category { get; protected set; }
        public float Duration => _remainingDuration;
        public bool IsActive => _isActive;
    
        public virtual void Apply(GameObject target)
        {
            _target = target;
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

        public void RefreshDuration()
        {
            _remainingDuration = _baseDuration;
        }
    }
}