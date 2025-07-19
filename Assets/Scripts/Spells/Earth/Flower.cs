using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Enums;
using UnityEngine;
using EnemyStaff;

namespace Spells.Earth
{
    [RequireComponent(typeof(Ally))]
    public class Flower : SummoningSpell, ITargetble
    {
        private readonly float _size = 5f;
        private readonly float _spikeCooldown = 2f;
        private readonly float _spikeCriticalChance = 0.2f;
        private readonly float _spikeCriticalMultiply = 1.2f;
        private readonly List<Enemy> _nearbyEnemies = new List<Enemy>();

        private float _time;
        private bool _isReadyToShoot;

        private bool _isTargeted;
        private Ally _ally;
        
        protected override void Awake()
        {
            transform.localScale *= _size;
            _time = 0f;
            _isReadyToShoot = true;
            _isTargeted = false;
            _ally = GetComponent<Ally>();
            
            base.Awake();
        }

        private void Update()
        {
            if (_isReadyToShoot)
            {
                TryToShoot();
            }
            else
            {
                Cooldown();
            }
        }

        private void Cooldown()
        {
            _time += Time.deltaTime;
                
            if (_time > _spikeCooldown)
            {
                _time = 0f;
                _isReadyToShoot = true;
            }
        }

        private void TryToShoot()
        {
            if (_nearbyEnemies.Count == 0) return;
            
            Enemy nearestEnemy = _nearbyEnemies.OrderBy(enemy =>
                Vector2.Distance(transform.position, enemy.GameObject.transform.position)
            ).FirstOrDefault();

            Spell spell = SpellFactory.Instance.PoolSpell(SpellName.Spike);

            if ((spell as ProjectileSpell)?.TrySetTarget(nearestEnemy) == false) return;
            spell.transform.position = transform.position;
            spell.Initialize(SpellData.Instance.GetSpellConfig(SpellName.Spike), _spikeCriticalMultiply, _spikeCriticalChance);
            spell.DoSpell();
            
            _isReadyToShoot = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) _nearbyEnemies.Add(enemy);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (_nearbyEnemies.Contains(enemy)) _nearbyEnemies.Remove(enemy);
        }

        private void OnDestroy()
        {
            OnTargetDestroy?.Invoke();
        }

        public bool IsTargetable => true;
        public bool IsTargeted => _isTargeted;
        public void OnTargeted()
        {
            _isTargeted = true;
            _ally.OnTargeted();
        }

        public void OnUntargeted()
        {
            _isTargeted = false;
            _ally.OnUntargeted();
        }

        public GameObject GameObject => gameObject;
        public event Action OnTargetDestroy;
    }
}
