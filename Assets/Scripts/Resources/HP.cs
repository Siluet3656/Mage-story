using System.Collections;
using System.Linq;
using Data.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Hp : MonoBehaviour
{
    [FormerlySerializedAs("healthBar")]
    [Header("PRESET")]
    [FormerlySerializedAs("HealthBar")] [SerializeField] private Image _healthBar = null;
    [FormerlySerializedAs("overhpBar")] [FormerlySerializedAs("OverHPBar")] [SerializeField] private Image _overhpBar = null;
    [FormerlySerializedAs("mainShieldStackBar")] [FormerlySerializedAs("MainShieldStackBar")] [SerializeField] private Image _mainShieldStackBar = null;
    [FormerlySerializedAs("shieldStackBars")] [FormerlySerializedAs("ShieldStackBars")] [SerializeField] private Image [] _shieldStackBars = null;
    [FormerlySerializedAs("maxHp")]
    [Space]
    [FormerlySerializedAs("Max_HP")] [SerializeField] private int _maxHp = 0;
    [FormerlySerializedAs("frostAegisOverHpAmount")] [FormerlySerializedAs("FrostAegisOverHPAmount")] [SerializeField] private float _frostAegisOverHpAmount;
    [FormerlySerializedAs("earthShieldStacksAmount")] [FormerlySerializedAs("EarthShieldStacksAmount")] [SerializeField] private int _earthShieldStacksAmount;
    [FormerlySerializedAs("currentHp")]
    [Header("CURRENT STATE")]
    [FormerlySerializedAs("Current_HP")] [SerializeField] private float _currentHp = 0;
    [FormerlySerializedAs("overHp")] [FormerlySerializedAs("OverHP")] [SerializeField] private float _overHp = 0;
    [FormerlySerializedAs("shieldStacks")] [FormerlySerializedAs("ShieldStacks")] [SerializeField] private int _shieldStacks = 0;
    private bool _isTakingDamageEachSecond = false;
    private bool _isWaitingOneSecond = false;
    private bool _isInvulnerable = false;
    private float _tickingDamage;
    private float _tickingCritChance;
    private float _tickingCritMultiply;

    private Buff _buffs;

    private void Awake()
    {
        _buffs = this.GetComponent<Buff>();
        if (_buffs)
        {
            _buffs.OnPlayerFreeze += GetInvulnerability;
            _buffs.OnPlayerUnFreeze += RemoveInvulnerability;
            _buffs.OnEnemyFreeze += GetInvulnerability;
            _buffs.OnEnemyUnFreeze += RemoveInvulnerability;
        }
    }

    private void Start() {
        if (_maxHp > 0)
        {
            _currentHp = _maxHp;
        }
        else
        {
            _currentHp = 1;
        }
    }

    private void Update() {
        _healthBar.fillAmount = (float)_currentHp/_maxHp;
        _overhpBar.fillAmount = (float)_overHp/_frostAegisOverHpAmount;

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
        TryToTakeCriticalDamage(_tickingDamage, _tickingCritMultiply, _tickingCritChance);
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
                GetComponent<Debuff>().GotCrit();
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
        _tickingCritMultiply = multiply;
        _tickingCritChance = chance;
    }
    
    public void StopTakingDamageEachSecond()
    {
        _isTakingDamageEachSecond = false;
        StopCoroutine(WaitOneSecondAndTakeDamage());
    }

    public void GetOverHp(SpellType source)
    {
        switch (source)
        {
            case SpellType.FrostAegis:
                _overHp = _frostAegisOverHpAmount;
                break;
        }
    }

    public void GetShieldStacks(SpellType source)
    {
        switch (source)
        {
            case SpellType.EarthShield:
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
