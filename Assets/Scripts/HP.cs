using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] private int Max_HP = 0;
    [SerializeField] private Image HealthBar = null;
    [SerializeField] private int Current_HP = 0;

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
    }

    public void TakeDamage(int damage)
    {
        Current_HP = Current_HP - damage;
        if (Current_HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
