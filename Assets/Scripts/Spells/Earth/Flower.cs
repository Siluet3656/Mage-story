using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Enums;
using UnityEngine;
using EnemyStaff;

namespace Spells.Earth
{
    public class Flower : SummoningSpell
    {
        private readonly float _size = 5f;
        private readonly float _spikeCooldown = 2f;
        private readonly float _spikeCriticalChance = 0.2f;
        private readonly float _spikeCriticalMultiply = 1.2f;
        private readonly List<Enemy> _nearbyEnemies = new List<Enemy>();

        private float _time;
        private bool _isReadyToShoot;
        
        protected override void Awake()
        {
            transform.localScale *= _size;
            _time = 0f;
            _isReadyToShoot = true;
            
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
    }
}
