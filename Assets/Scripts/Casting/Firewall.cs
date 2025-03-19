using UnityEngine;

public class Firewall : MonoBehaviour
{
    [SerializeField] private float duration;
    private float damage;
    private bool _isWaiting;

    private void OnCollisionEnter(Collision other)
    {
        HP hp = other.gameObject.GetComponent<HP>();
        if (hp)
        {
            hp.StartTakingDamageEachSecond(damage);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        HP hp = other.gameObject.GetComponent<HP>();
        if (hp)
        {
            hp.StopTakingDamageEachSecond();
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
