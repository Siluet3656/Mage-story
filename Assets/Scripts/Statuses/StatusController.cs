using System.Collections.Generic;
using UnityEngine;
using Data.Enums;
using Data.StatusConfigs;
using EntityResources;
using Spells.NoElemental;
using Statuses.Buffs;
using Statuses.Debuffs;
using View;


namespace Statuses
{
    [RequireComponent(typeof(StatusView))]
    public class StatusController : MonoBehaviour
    {
        private readonly Dictionary<StatusType, IStatusEffect> _activeStatusEffects = new Dictionary<StatusType, IStatusEffect>();
        private StatusView _statusView;
        private Hp _hp;
    
        private void Awake()
        {
            _statusView = GetComponent<StatusView>();
            if (_statusView == null)
            {
                _statusView = gameObject.AddComponent<StatusView>();
            }

            _hp = gameObject.GetComponent<Hp>();
            if (_hp == null)
            {
                _hp = gameObject.AddComponent<Hp>();
            }
        }
    
        private void Update()
        {
            List<IStatusEffect> effects = new List<IStatusEffect>(_activeStatusEffects.Values);
            foreach (var effect in effects)
            {
                effect.Update(Time.deltaTime);
                
                if (effect.IsActive == false)
                {
                    RemoveStatus(effect.Type);
                }
            }
        }
        
        private IStatusEffect CreateStatusEffect(StatusEffectData data)
        {
            switch (data.Type)
            {
                case StatusType.Slow:
                    return new SlowStatusEffect(data);
                case StatusType.FireAura:
                    return new FireAuraStatusEffect(data);
                case StatusType.FireMark:
                    return new FireMarkStatusEffect(data);
                case StatusType.StasisFreeze:
                    return new StasisFreezeEffect(data);
                case StatusType.Poison:
                    return new PoisonStatusEffect(data as TickingDamageStatusEffectData, _hp);
                case StatusType.Plague:
                    if (HasStatus(StatusType.Immunity)) return null;
                    return new PlagueStatusEffect(data as TickingDamageStatusEffectData, _hp, this);
                case StatusType.Immunity:
                    return new ImmunityStatusEffect(data);
                case StatusType.RootCore:
                    return new RootCoreStatusEffect(data, gameObject);
                case StatusType.Root:
                    if (HasStatus(StatusType.RootCore)) return null;
                    return new RootStatusEffect(data, gameObject);
                case StatusType.Bleed:
                    return new BleedStatusEffect(data, _hp);
                case StatusType.Silence:
                    return new SilenceStatusEffect(data);
                default:
                    Debug.LogWarning($"No implementation for status Type: {data.Type}");
                    return null;
            }
        }
        
        private void RemoveStatus(StatusType type)
        {
            if (_activeStatusEffects.ContainsKey(type))
            {
                _activeStatusEffects.Remove(type);
                _statusView?.RemoveStatusEffect(type, gameObject);
            }
        }

    
        public void ApplyStatus(StatusEffectData data)
        {
            if (_activeStatusEffects.ContainsKey(data.Type))
            {
                // Option: Refresh duration instead of ignoring
                return;
            }
        
            IStatusEffect effect = CreateStatusEffect(data);
            if (effect != null)
            {
                _activeStatusEffects[data.Type] = effect;
                effect.Apply(gameObject);
                _statusView?.AddStatusEffect(data, gameObject);
            }
        }
        
        public bool HasStatus(StatusType type)
        {
            return _activeStatusEffects.ContainsKey(type);
        }
        
        public bool HasStatus(StatusType type, out StatusEffect status)
        {
            bool isStatusExists = _activeStatusEffects.TryGetValue(type, out IStatusEffect statusEffect);
            status = statusEffect as StatusEffect;
            return isStatusExists;
        }

        public void RefreshAllStatuses()
        {
            foreach (var statusEffect in _activeStatusEffects)
            {
                statusEffect.Value.RefreshDuration();
            }
        }
    }
}