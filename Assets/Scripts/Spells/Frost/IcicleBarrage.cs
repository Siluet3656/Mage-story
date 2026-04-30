using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Enums;
using Data.SpellConfigs;
using EnemyStaff;
using EntityStaff;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spells.Frost
{
    public class IcicleBarrage : SummoningSpell
    {
        private ITargetable _target;
        private readonly int _amountOfIcicles = 20;
        private float _delay;

        private readonly List<ProjectileSpell> _subscribedSpells = new List<ProjectileSpell>();

        private IEnumerator SpawnIcicles()
        {
            for (int i = 0; i < _amountOfIcicles; i++)
            {
                Spell icicle = SpellFactory.Instance.PoolSpell(SpellName.Icicle);
                SpellConfig spellConfig = SpellData.Instance.GetSpellConfig(SpellName.Icicle);

                if (icicle is ProjectileSpell projectile)
                {
                    if (projectile.TrySetTarget(_target))
                    {
                        float randomPositionX = Random.Range(transform.position.x - 1f, transform.position.x + 1f);
                        float randomPositionY = Random.Range(transform.position.y - 1f, transform.position.y + 1f);
                        
                        projectile.Initialize(spellConfig, CriticalMultiply, CriticalChance);
                        projectile.transform.position = new Vector3(randomPositionX, randomPositionY, transform.position.z);
                        
                        _target.GameObject.GetComponent<Hp>().OnDeath += projectile.OnTargetDeath;
                        
                        _subscribedSpells.Add(projectile);
                        
                        projectile.DoSpell();
                    }
                }

                yield return new WaitForSeconds(_delay);
            }
        }

        private void OnEnable()
        {
            if (_target != null)
                _target.GameObject.GetComponent<Hp>().OnDeath += OnExistingInterrupt;
        }

        private void OnDisable()
        {
            if (_target != null)
                _target.GameObject.GetComponent<Hp>().OnDeath -= OnExistingInterrupt;
        }

        private void OnExistingInterrupt()
        {
            OnExistingEnd(null);
        }
        
        private void Update()
        {
            Vector3 direction = _target.GameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        protected override void Awake()
        {
            base.Awake();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);
        
            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                        
                if (child.CompareTag("IcecleBarrageEffects"))
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        protected override void OnExistingEnd(EnemyTargeting enemyTargeting)
        {
            // Unsubscribe from all spell death events
            foreach (var spell in _subscribedSpells)
            {
                if (_target != null && _target.GameObject != null) 
                {
                    var targetHp = _target.GameObject.GetComponent<Hp>();
                    if (targetHp != null)
                    {
                        targetHp.OnDeath -= spell.OnTargetDeath;
                    }
                }
            }
            _subscribedSpells.Clear();
            
            // Unsubscribe from target death event
            if (_target != null && _target.GameObject != null)
            {
                var targetHp = _target.GameObject.GetComponent<Hp>();
                if (targetHp != null)
                {
                    targetHp.OnDeath -= () => _target = null;
                }
            }
            
            base.OnExistingEnd(enemyTargeting);
        }

        protected override void ResetSummonState()
        {
            base.ResetSummonState();
            
            // Clear target reference
            _target = null;
            
            // Ensure all subscriptions are cleared
            _subscribedSpells.Clear();
        }

        public override void DoSpell()
        {
            Vector3 direction = _target.GameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _target.GameObject.GetComponent<Hp>().OnDeath += () => _target = null; //это мб тоже потом удалить
            
            base.DoSpell();

            _delay = ExistTime / (_amountOfIcicles + 1);
            StartCoroutine(SpawnIcicles());
        }

        public bool TrySetTarget(ITargetable target)
        {
            if (target == null) return false;
            
            _target = target;

            return true;
        }
    }
}
