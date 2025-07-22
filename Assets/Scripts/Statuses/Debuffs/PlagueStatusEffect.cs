using UnityEngine;
using Data.StatusConfigs;
using EntityResources;
using Statuses.Other;

namespace Statuses.Debuffs
{
    public class PlagueStatusEffect : StatusEffect
    {
        private readonly float _tickInterval;
        private readonly float _damagePerTick;
        private readonly float _criticalChance;
        private readonly float _criticalMultiplier;
        private readonly Hp _hp;
        private readonly GameObject _plagueSpreadPrefab;
        private readonly StatusController _statusController;
        
        private float _tickTime;
        
        private void DoDamage()
        {
            if (_hp != null)
            {
                _hp.TryToTakeCriticalDamage(_damagePerTick, _criticalMultiplier, _criticalChance);
            }
        }

        private void SpawnPlagueSpread()
        {
            PlagueSpread plagueSpread = Object.Instantiate(_plagueSpreadPrefab, _hp.gameObject.transform.position, Quaternion.identity, null).GetComponent<PlagueSpread>();
            plagueSpread.SetCurrentTarget(_hp.gameObject);
        }

        public PlagueStatusEffect(TickingDamageStatusEffectData data, Hp hp, StatusController statusController) : base(data)
        {
            _tickInterval = data.TickInterval;
            _damagePerTick = data.DamagePerTick;
            _criticalChance = data.CriticalChance;
            _criticalMultiplier = data.CriticalMultiplier;
            _hp = hp;
            _statusController = statusController;
            _plagueSpreadPrefab = Resources.Load<GameObject>("Other/PlagueSpread");
            
            if (_plagueSpreadPrefab == null)
            {
                Debug.LogError("No plague spread prefab");
            }
            
            _tickTime = _tickInterval;

            _hp.OnCriticalDamageReceived += SpawnPlagueSpread;
        }
        
        public override void Update(float deltaTime)
        {
            _tickTime -= deltaTime;

            if (_tickTime <= 0)
            {
                _tickTime = _tickInterval;
                DoDamage();
            }
            
            base.Update(deltaTime);
        }
        
        public override void Remove(GameObject target)
        {
            StatusEffectData data = Resources.Load<StatusEffectData>("Other/PlagueImmunity");
            
            _statusController.ApplyStatus(data);
            
            base.Remove(target);
        }
    }
}
