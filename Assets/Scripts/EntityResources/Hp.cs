using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Data.Enums;

namespace EntityResources
{
    public class Hp : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField, Min(1)] private int _maxHealth;
        [SerializeField, Min(1)] private float _frostAegisAdditionalHealthAmount ;
        [SerializeField, Range(1,3)] private int _earthShieldStacksAmount;

        private float _currentHealth;
        private float _additionalHealth;
        private int _shieldStacks;
        private bool _isInvulnerable;
        
        private void Awake()
        {
            InitializeHealth();
        }
        
        private void InitializeHealth()
        {
            _currentHealth = Mathf.Max(_maxHealth, 1);
            OnHealthChanged?.Invoke(_currentHealth);
        }

        private void TakeDamage(float damage)
        {
            if (_shieldStacks > 0)
            {
                RemoveShieldStack();
                return;
            }

            ApplyDamageToHealth(damage);
        }

        private void ApplyDamageToHealth(float damage)
        {
            if (_additionalHealth > 0)
            {
                float additionalHealthDamage = Mathf.Min(damage, _additionalHealth);
                _additionalHealth -= additionalHealthDamage;
                damage -= additionalHealthDamage;
                OnAdditionalHealthChanged?.Invoke(_additionalHealth);
                
                if (damage <= 0) return;
            }
            
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            OnHealthChanged?.Invoke(_currentHealth);
            
            if (_currentHealth <= 0)
                Die();
        }

        private void RemoveShieldStack()
        {
            _shieldStacks = Mathf.Max(0, _shieldStacks - 1);
            OnShieldStacksChanged?.Invoke(_shieldStacks);
        }

        private void Die()
        {
            OnDeath?.Invoke();
            Destroy(gameObject); //Фабрика
        }
        
        public event Action<float> OnHealthChanged;
        public event Action<float> OnAdditionalHealthChanged;
        public event Action<int> OnShieldStacksChanged;
        public event Action OnDeath;

        public float MaxHealth => _maxHealth;

        public float FrostAegisAdditionalHealthAmount => _frostAegisAdditionalHealthAmount;

        public void TryToTakeCriticalDamage(float damage, float criticalMultiply, float criticalChance)
        {
            if (_isInvulnerable) return;
            
            bool isCritical = Random.Range(0f, 1f) < criticalChance;
            float finalDamage = isCritical ? damage * criticalMultiply : damage;
            
            TakeDamage(finalDamage);
        }

        public void GetAdditionalHp(SpellName source)
        {
            switch (source)
            {
                case SpellName.FrostAegis:
                    _additionalHealth = _frostAegisAdditionalHealthAmount;
                    OnAdditionalHealthChanged?.Invoke(_additionalHealth);
                    break;
            }
        }
        
        public void GetShieldStacks(SpellName source)
        {
            switch (source)
            {
                case SpellName.EarthShield:
                    _shieldStacks = _earthShieldStacksAmount;
                    OnShieldStacksChanged?.Invoke(_shieldStacks);
                    break;
            }
        }

        public void SetInvulnerable(bool isInvulnerable)
        {
            _isInvulnerable = isInvulnerable;
        }
    }
}