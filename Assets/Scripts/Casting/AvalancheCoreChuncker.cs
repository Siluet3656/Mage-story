using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheCoreChuncker : MonoBehaviour
{
    [SerializeField] private GameObject chunck;
    [Space]
    [SerializeField] private RangeType rangeType;
    [SerializeField] private float _blastingTime;
    private float _damage;
    private float _critmultiply;
    private float _critchance;
    private List<Enemy> enemies;
    private Enemy origin;
    
    private void Start()
    {
        enemies = new List<Enemy>();
        switch (rangeType)
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
        HP hp = other.gameObject.GetComponent<HP>();
        if (enemy & hp)
        {
            if (enemy != origin)
            {
                if (enemies.Find(enemy1 => enemy1.gameObject == enemy.gameObject) == false)
                {
                    enemies.Add(enemy);
                }
            }
        }
    }
    
    private IEnumerator Blasting()
    {
        Spell spell;
        yield return new WaitForSeconds(_blastingTime);
        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                spell = Instantiate(chunck, transform.position, Quaternion.identity).GetComponent<Spell>();
                spell.SetTarget(enemy);
                spell.AdjustCrit(_critmultiply,_critchance);
                spell.ChunkDamage(_damage / enemies.Count);
            }
        }
        Destroy(this.gameObject);
    }

    public void SetDamage(float damage, float critMultiplly, float critChance)
    {
        _damage = damage;
        _critmultiply = critMultiplly;
        _critchance = critChance;
    }

    public void SetOrigin(Enemy enemy)
    {
        origin = enemy;
    }
}
