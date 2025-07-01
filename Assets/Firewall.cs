using System.Collections;
using EntityResources;
using UnityEngine;
using UnityEngine.Serialization;

public class Firewall : MonoBehaviour
{
    [FormerlySerializedAs("duration")] [SerializeField] private float _duration;
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
        Hp hp = other.gameObject.GetComponent<Hp>();
        if (hp != null)
        {
            //hp.StartTakingDamageEachSecond(_damage, _critmultiply, _critchance);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Hp hp = other.gameObject.GetComponent<Hp>();
        if (hp != null)
        {
            //hp.StopTakingDamageEachSecond();
        }
    }

    private IEnumerator FirewallDuration()
    {
        yield return new WaitForSeconds(_duration);
        Destroy(this.gameObject);
    }

    public void SetDamage(float damage, float multiply, float chance)
    {
    _damage = damage;
    _critmultiply = multiply;
    _critchance = chance;
    }
}
