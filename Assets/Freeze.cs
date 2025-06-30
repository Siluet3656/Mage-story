using System.Collections;
using Data.Enums;
using EntityResources;
using Statuses;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    [SerializeField] private RangeType _rangeType;
    [SerializeField] private float _blastingTime;
    private float _damage;
    private float _critmultiply;
    private float _critchance;

    private void Start()
    {
        switch (_rangeType)
        {
            case RangeType.Small:
                this.transform.localScale = new Vector3(0.5f,0.5f,1);
                break;
            case RangeType.Medium:
                this.transform.localScale = new Vector3(1,1,1);
                break;
            case RangeType.Large:
                this.transform.localScale = new Vector3(2,2,1);
                break;
            case RangeType.Giant:
                this.transform.localScale = new Vector3(3,3,1);
                break;
        }
        StartCoroutine("Blasting");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Hp hp = other.gameObject.GetComponent<Hp>();
        //Debuff dbf = other.gameObject.GetComponent<Debuff>();
        Enemy target = other.gameObject.GetComponent<Enemy>();
        //Debug.Log(hp & dbf & target);
        /*if (hp & dbf & target)
        {
            hp.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_damage, _critmultiply, _critchance);
            dbf.DebuffTarget(DebuffType.Slow, target);
        }*/
    }
    
    private IEnumerator Blasting()
    {
        yield return new WaitForSeconds(_blastingTime);
        Destroy(this.gameObject);
    }
    
    public void SetDamage(float damage, float critMultiplly, float critChance)
    {
        _damage = damage;
        _critmultiply = critMultiplly;
        _critchance = critChance;
    }
}
