using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] private int Max_HP = 0;
    [SerializeField] private Image HealthBar = null;
    [SerializeField] private float Current_HP = 0;
    private bool _isTakingDamageEachSecond = false;
    private bool _isWaitingOneSecond = false;
    private float _tickingDamage;
    private float _tickingCritChance;
    private float _tickingCritMultiply;

    private void Start() {
        if (Max_HP > 0)
        {
            Current_HP = Max_HP;
        }
        else
        {
            Current_HP = 1;
        }
    }

    private void Update() {
        HealthBar.fillAmount = (float)Current_HP/Max_HP;

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
        Current_HP -= damage;
        if (Current_HP <= 0)
        {
            Die();
        }
    }

    public void TryToTakeCriticalDamage(float damage, float multiply, float chance)
    {
        
        float proc = Random.Range(0,100);
        bool isProced = (proc - chance * 100) < 0;
        Debug.Log(multiply + " / " + chance);
        if (isProced)
        {
            TakeDamage(damage * multiply);
        }
        else
        {
            TakeDamage(damage);
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
}
