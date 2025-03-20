using System.Collections;
using UnityEngine;

public class Firewall : MonoBehaviour
{
    [SerializeField] private float duration;
    private float _damage;
    private bool _isWaiting;

    private void Start()
    {
        StartCoroutine(FirewallDuration());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HP hp = other.gameObject.GetComponent<HP>();
        if (hp != null)
        {
            hp.StartTakingDamageEachSecond(_damage);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        HP hp = other.gameObject.GetComponent<HP>();
        if (hp != null)
        {
            hp.StopTakingDamageEachSecond();
        }
    }

    private IEnumerator FirewallDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
}
