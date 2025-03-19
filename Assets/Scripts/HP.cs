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
        TakeDamage(_tickingDamage);
        _isWaitingOneSecond = false;
        
    }
    
    public void TakeDamage(float damage)
    {
        Current_HP = Current_HP - damage;
        if (Current_HP <= 0)
        {
            Die();
        }
    }

    public void StartTakingDamageEachSecond(float damage)
    {
        _isTakingDamageEachSecond = true;
        _tickingDamage = damage;
    }
    
    public void StopTakingDamageEachSecond()
    {
        _isTakingDamageEachSecond = false;
        StopCoroutine(WaitOneSecondAndTakeDamage());
    }
}
