using System.Collections;
using Data.Enums;
using Statuses;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace EntityResources
{
    public class Hp : MonoBehaviour
    {
        [Header("PRESET")]
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _additionalHealthBar;
        [SerializeField] private Image _mainShieldStackBar;
        [SerializeField] private Image [] _shieldStackBars;
        [Space]
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _frostAegisOverHpAmount;
        [SerializeField] private int _earthShieldStacksAmount;
        [Header("CURRENT STATE")]
        [SerializeField] private float _currentHp;
        [SerializeField] private float _overHp;
        [SerializeField] private int _shieldStacks;
        private bool _isTakingDamageEachSecond;
        private bool _isWaitingOneSecond;
        private bool _isInvulnerable;
        private float _tickingDamage;
        private float _tickingCriticalChance;
        private float _tickingCriticalMultiply;

        //private Buff _buffs;

        private void Awake()
        {
            /*_buffs = this.GetComponent<Buff>();
            if (_buffs)
            {
                _buffs.OnPlayerFreeze += GetInvulnerability;
                _buffs.OnPlayerUnFreeze += RemoveInvulnerability;
                _buffs.OnEnemyFreeze += GetInvulnerability;
                _buffs.OnEnemyUnFreeze += RemoveInvulnerability;
            }*/
        }

        private void Start() {
            if (_maxHealth > 0)
            {
                _currentHp = _maxHealth;
            }
            else
            {
                _currentHp = 1;
            }
        }

        private void Update() {
            _healthBar.fillAmount = (float)_currentHp/_maxHealth;
            _additionalHealthBar.fillAmount = (float)_overHp/_frostAegisOverHpAmount;

            if (_isTakingDamageEachSecond)
            {
                if (!_isWaitingOneSecond)
                {
                    StartCoroutine(WaitOneSecondAndTakeDamage());
                }
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    
        private IEnumerator WaitOneSecondAndTakeDamage()
        {
            _isWaitingOneSecond = true;
            yield return new WaitForSeconds(1);
            TryToTakeCriticalDamage(_tickingDamage, _tickingCriticalMultiply, _tickingCriticalChance);
            _isWaitingOneSecond = false;
        
        }
    
        private void TakeDamage(float damage)
        {
            if (_shieldStacks > 0)
            {
                RemoveOneShieldStack();
                return;
            }
        
            if (_overHp - damage > 0)
            {
                _overHp -= damage;
            }
            else if (_overHp > 0)
            {
                damage -= _overHp;
                _overHp = 0;
                if (_currentHp - damage > 0)
                {
                    _currentHp -= damage;
                }
                else
                {
                    _currentHp = 0;
                    Die();
                }
            }
            else
            {
                if (_currentHp - damage > 0)
                {
                    _currentHp -= damage;
                }
                else
                {
                    _currentHp = 0;
                    Die();
                }
            }
        }

        private void GetInvulnerability()
        {
            _isInvulnerable = true;
        }
    
        private void RemoveInvulnerability()
        {
            _isInvulnerable = false;
        }

        private void RemoveOneShieldStack()
        {
            _shieldStacks -= 1;
        
            _shieldStackBars[_shieldStacks].gameObject.SetActive(false);
            if (_shieldStacks <= 0)
            {
                _mainShieldStackBar.gameObject.SetActive(false);
            }
        }

        public void TryToTakeCriticalDamage(float damage, float multiply, float chance)
        {
            if (_isInvulnerable == false)
            {
                float proc = Random.Range(0,100);
                bool isProced = (proc - chance * 100) < 0;
                //Debug.Log(multiply + " / " + chance);
                if (isProced)
                {
                    TakeDamage(damage * multiply);
                    //GetComponent<Debuff>().GotCrit();
                }
                else
                {
                    TakeDamage(damage);
                }
            }
        }

        public void StartTakingDamageEachSecond(float damage, float multiply, float chance)
        {
            _isTakingDamageEachSecond = true;
            _tickingDamage = damage;
            _tickingCriticalMultiply = multiply;
            _tickingCriticalChance = chance;
        }
    
        public void StopTakingDamageEachSecond()
        {
            _isTakingDamageEachSecond = false;
            StopCoroutine(WaitOneSecondAndTakeDamage());
        }

        public void GetOverHp(SpellName source)
        {
            switch (source)
            {
                case SpellName.FrostAegis:
                    _overHp = _frostAegisOverHpAmount;
                    break;
            }
        }

        public void GetShieldStacks(SpellName source)
        {
            switch (source)
            {
                case SpellName.EarthShield:
                    _shieldStacks = _earthShieldStacksAmount;
                    _mainShieldStackBar.gameObject.SetActive(true);
                    foreach (var shieldStack in _shieldStackBars)
                    {
                        shieldStack.gameObject.SetActive(true);
                    }
                    break;
            }
        }
    }
}
