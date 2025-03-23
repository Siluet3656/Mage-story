using System.Collections;
using UnityEngine;

public class Firewall : MonoBehaviour
{
    [SerializeField] private float duration;
    private float _damage;
    private bool _isWaiting;
    private float _critmultiply;
    private float _critchance;

    private void Start()
    {
        StartCoroutine(FirewallDuration());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HP hp = other.gameObject.GetComponent<HP>();
        if (hp != null)
        {
            hp.StartTakingDamageEachSecond(_damage, _critmultiply, _critchance);
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

    public void SetDamage(float damage, float multiply, float chance)
    {
    _damage = damage;
    _critmultiply = multiply;
    _critchance = chance;
    }
}
