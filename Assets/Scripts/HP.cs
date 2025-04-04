using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HP : MonoBehaviour
{
    [FormerlySerializedAs("HealthBar")] [SerializeField] private Image healthBar = null;
    [FormerlySerializedAs("OverHPBar")] [SerializeField] private Image overhpBar = null;
    [Space]
    [FormerlySerializedAs("Max_HP")] [SerializeField] private int maxHp = 0;
    [FormerlySerializedAs("FrostAegisOverHPAmount")] [SerializeField] private float frostAegisOverHpAmount;
    [Space]
    [FormerlySerializedAs("Current_HP")] [SerializeField] private float currentHp = 0;
    [FormerlySerializedAs("OverHP")] [SerializeField] private float overHp = 0;
    private bool _isTakingDamageEachSecond = false;
    private bool _isWaitingOneSecond = false;
    private bool _isInvulnerable = false;
    private float _tickingDamage;
    private float _tickingCritChance;
    private float _tickingCritMultiply;

    private Buff Buffs;

    private void Awake()
    {
        Buffs = this.GetComponent<Buff>();
        if (Buffs)
        {
            Buffs.OnPlayerFreeze += GetInvulnerability;
            Buffs.OnPlayerUnFreeze += RemoveInvulnerability;
            Buffs.OnEnemyFreeze += GetInvulnerability;
            Buffs.OnEnemyUnFreeze += RemoveInvulnerability;
        }
    }

    private void Start() {
        if (maxHp > 0)
        {
            currentHp = maxHp;
        }
        else
        {
            currentHp = 1;
        }
    }

    private void Update() {
        healthBar.fillAmount = (float)currentHp/maxHp;
        overhpBar.fillAmount = (float)overHp/frostAegisOverHpAmount;

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
        if (overHp - damage > 0)
        {
            overHp -= damage;
        }
        else if (overHp > 0)
        {
            damage -= overHp;
            overHp = 0;
            if (currentHp - damage > 0)
            {
                currentHp -= damage;
            }
            else
            {
                currentHp = 0;
                Die();
            }
        }
        else
        {
            if (currentHp - damage > 0)
            {
                currentHp -= damage;
            }
            else
            {
                currentHp = 0;
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

    public void GetOverHP(SpellType source)
    {
        switch (source)
        {
            case SpellType.FrostAegis:
                overHp = frostAegisOverHpAmount;
                break;
        }
    }
}
