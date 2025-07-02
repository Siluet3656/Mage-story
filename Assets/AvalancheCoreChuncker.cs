using System.Collections;
using System.Collections.Generic;
using Data.Enums;
using EnemyStaff;
using EntityResources;
using UnityEngine;
using UnityEngine.Serialization;

public class AvalancheCoreChuncker : MonoBehaviour
{
    [FormerlySerializedAs("chunck")] [SerializeField] private GameObject _chunck;
    [FormerlySerializedAs("rangeType")]
    [Space]
    [SerializeField] private RangeType _rangeType;
    [SerializeField] private float _blastingTime;
    private float _damage;
    private float _critmultiply;
    private float _critchance;
    private List<Enemy> _enemies;
    private Enemy _origin;
    
    private void Start()
    {
        _enemies = new List<Enemy>();
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

    private void OnTriggerStay2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        Hp hp = other.gameObject.GetComponent<Hp>();
        if (enemy & hp)
        {
            if (enemy != _origin)
            {
                if (_enemies.Find(enemy1 => enemy1.gameObject == enemy.gameObject) == false)
                {
                    _enemies.Add(enemy);
                }
            }
        }
    }
    
    /*private IEnumerator Blasting()
    {
        Spell spell;
        yield return new WaitForSeconds(_blastingTime);
        if (_enemies.Count > 0)
        {
            foreach (var enemy in _enemies)
            {
                spell = Instantiate(_chunck, transform.position, Quaternion.identity).GetComponent<Spell>();
                spell.SetTarget(enemy);
                spell.AdjustCrit(_critmultiply,_critchance);
                spell.ChunkDamage(_damage / _enemies.Count);
            }
        }
        Destroy(this.gameObject);
    }*/

    public void SetDamage(float damage, float critMultiplly, float critChance)
    {
        _damage = damage;
        _critmultiply = critMultiplly;
        _critchance = critChance;
    }

    public void SetOrigin(Enemy enemy)
    {
        _origin = enemy;
    }
}
