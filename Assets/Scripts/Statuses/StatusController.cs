using System.Collections.Generic;
using Data;
using Data.Enums;
using UI;
using UnityEngine;

namespace Statuses
{
    public class StatusController : MonoBehaviour
    {
        private readonly Dictionary<StatusType, IStatusEffect> _activeStatusEffects = new Dictionary<StatusType, IStatusEffect>();
        private StatusView _statusView;
    
        private void Awake()
        {
            _statusView = GetComponent<StatusView>();
            if (_statusView == null)
            {
                _statusView = gameObject.AddComponent<StatusView>();
            }
        }
    
        private void Update()
        {
            // Create a copy to avoid modification during iteration
            List<IStatusEffect> effects = new List<IStatusEffect>(_activeStatusEffects.Values);
            foreach (var effect in effects)
            {
                effect.Update(Time.deltaTime);
            }
        }
    
        public void ApplyStatus(StatusEffectData data, params object[] parameters)
        {
            if (_activeStatusEffects.ContainsKey(data.Type))
            {
                // Option: Refresh duration instead of ignoring
                return;
            }
        
            IStatusEffect effect = CreateStatusEffect(data, parameters);
            if (effect != null)
            {
                _activeStatusEffects[data.Type] = effect;
                effect.Apply(gameObject);
                _statusView?.AddStatusEffect(data.Type, gameObject);
            }
        }
    
        public void RemoveStatus(StatusType type)
        {
            if (_activeStatusEffects.TryGetValue(type, out var effect))
            {
                effect.Remove(gameObject);
                _activeStatusEffects.Remove(type);
                _statusView?.RemoveStatusEffect(type, gameObject);
            }
        }
    
        private IStatusEffect CreateStatusEffect(StatusEffectData data, object[] parameters)
        {
            switch (data.Type)
            {
                case StatusType.Slow:
                    return new SlowStatusEffect(data, (float)parameters[0]);
                case StatusType.Poison:
                    return new PoisonStatusEffect(data, 
                        (float)parameters[0], (float)parameters[1], 
                        (float)parameters[2], (float)parameters[3]);
                // Add cases for other status Types
                default:
                    Debug.LogWarning($"No implementation for status Type: {data.Type}");
                    return null;
            }
        }
    
        public bool HasStatus(StatusType type)
        {
            return _activeStatusEffects.ContainsKey(type);
        }
    }
}