using System.Collections.Generic;
using Data.Enums;
using UnityEngine;
using Data.StatusConfigs;
using EnemyStaff;
using EntityResources;
using Statuses.Other;

namespace Statuses.Buffs
{
    public class RootCoreStatusEffect : StatusEffect
    {
        private readonly GameObject _rootSpreadPrefab;
        private readonly GameObject _objectWithThisStatus;
        private readonly float _size = 2.5f;
        
        private readonly Enemy _enemy;
        private readonly Hp _hp;
        //private readonly Ally _ally;

        private List<Enemy> _enemiesInRoots;

        private readonly float _damageToHeal = 0.5f;
        
        private void SpawnPlagueSpread()
        {
            RootSpread spread = Object.Instantiate(_rootSpreadPrefab, _objectWithThisStatus.transform.position, Quaternion.identity, null).GetComponent<RootSpread>();
            spread.transform.localScale *= _size;
            spread.Initialize(_objectWithThisStatus, this);
        }
        
        public RootCoreStatusEffect(StatusEffectData data, GameObject objectWithThisStatus) : base(data)
        {
            _objectWithThisStatus = objectWithThisStatus;
            
            _enemy = objectWithThisStatus.GetComponent<Enemy>();
            //_ally = objectWithThisStatus.GetComponent<Ally>();
            _hp = objectWithThisStatus.GetComponent<Hp>();
            
            if (_enemy != null)
                _enemy.SetMovementAvailability(false, MovementDisableSource.Roots);
            
            //if (_ally != null)
                //_ally.SetMovementAvailability()???
            
            _rootSpreadPrefab = Resources.Load<GameObject>("Other/RootSpread");
            
            if (_rootSpreadPrefab == null)
            {
                Debug.LogError("No root spread prefab");
            }
            else
            {
                SpawnPlagueSpread();
            }
        }

        public override void Remove(GameObject target)
        {
            if (_enemy != null)
                _enemy.SetMovementAvailability(true,  MovementDisableSource.Roots);
            
            //if (_ally != null)
            //_ally.SetMovementAvailability()???
            
            base.Remove(target);
        }

        public void Heal(float amount)
        {
            _hp.Heal(amount * _damageToHeal);
        }
    }
}
