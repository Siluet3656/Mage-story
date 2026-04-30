using System;
using System.Linq;
using AllyStaff;
using UnityEngine;
using Data;
using Data.Enums;
using EnemyStaff;
using EntityStaff;
using PlayerStaff;

namespace Spells.Earth
{
    [RequireComponent(typeof(Ally))]
    public class Flower : SummoningSpell, ITargetable
    {
        private readonly float _spikeCooldown = 2f;
        private readonly float _spikeCriticalChance = 0.2f;
        private readonly float _spikeCriticalMultiply = 1.2f;

        private TargetingCircle _targetingCircle;

        private float _time;
        private bool _isReadyToShoot;

        private bool _isTargeted;
        private Ally _ally;
        private SpriteRenderer _spriteRenderer;
        private Color _originalColor;
        private Color _targetedColor;
        
        protected override void Awake()
        {
            _ally = GetComponent<Ally>();
            _targetingCircle = GetComponentInChildren<TargetingCircle>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
            _targetedColor = Color.red; // Highlight color when targeted
            
            base.Awake();
        }

        private void OnEnable()
        {
            // Set the targeting circle radius based on the spell's attack radius
            if (_targetingCircle != null && _targetingCircle.Collider != null)
            {
                _targetingCircle.Collider.radius = Radius;
            }
            
            _spriteRenderer.color = _originalColor;
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
            if (_targetingCircle.NearbyTargets.Count == 0) return;
            
            ITargetable nearestEnemy = _targetingCircle.NearbyTargets
                .Where(enemy => enemy is EnemyTargeting)
                .OrderBy(enemy => Vector2.Distance(transform.position, enemy.GameObject.transform.position))
                .FirstOrDefault();

            if (nearestEnemy == null) return;
            
            Spell spell = SpellFactory.Instance.PoolSpell(SpellName.Spike);

            if ((spell as ProjectileSpell)?.TrySetTarget(nearestEnemy) == false) return;
            spell.transform.position = transform.position;
            spell.Initialize(SpellData.Instance.GetSpellConfig(SpellName.Spike), _spikeCriticalMultiply, _spikeCriticalChance);
            spell.DoSpell();
            
            _isReadyToShoot = false;
        }

        private void OnDestroy()
        {
            OnTargetDie?.Invoke();
            OnTargetDie = null; // Clear event subscriptions to prevent memory leaks
        }

        public bool IsTargetable => true;
        public bool IsTargeted => _isTargeted;
        
        public void OnTargeted()
        {
            _isTargeted = true;
            _spriteRenderer.color = _targetedColor;
        }

        public void OnUntargeted()
        {
            _isTargeted = false;
            _spriteRenderer.color = _originalColor;
        }

        public GameObject GameObject => gameObject;
        public event Action OnTargetDie;
        
        /// <summary>
        /// Called when the flower is returned to pool to reset its state
        /// </summary>
        protected override void ResetSummonState()
        {
            base.ResetSummonState();
            
            _isReadyToShoot = true;
            _time = 0f;
            _isTargeted = false;
            
            // Reset color to original
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _originalColor;
            }
            
            // Clear any existing target subscriptions
            if (_targetingCircle != null && _targetingCircle.NearbyTargets != null)
            {
                _targetingCircle.NearbyTargets.Clear();
            }
        }
    }
}
