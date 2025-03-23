using System.Collections;
using UnityEngine;

public class FB_blast : MonoBehaviour
{
    [SerializeField] private RangeType rangeType;
    [SerializeField] private float BlastingTime = 0f;
    private float Damage = 0;
    private float radius = 0;
    private float critMultyply;
    private float critChance;

    private void Start() {
        radius = RangeTypeData.GetDataByID(rangeType);
        transform.localScale = new Vector3(radius*2,radius*2,1);

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= radius)
            {
                enemy.gameObject.GetComponent<HP>().TryToTakeCriticalDamage(Damage, critMultyply, critChance);
            }
        }

        StartCoroutine("Blasting");
    }

    public void SetDamage(float Damage, float critMultiplly, float critChance)
    {
        this.Damage = Damage;
        this.critMultyply = critMultiplly;
        this.critChance = critChance;
    }

    private IEnumerator Blasting()
    {
        yield return new WaitForSeconds(BlastingTime);
        Destroy(this.gameObject);
    }
}
