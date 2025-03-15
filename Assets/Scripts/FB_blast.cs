using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_blast : MonoBehaviour
{
    [SerializeField] private RangeType rangeType;
    [SerializeField] private float BlastingTime = 0f;
    private float Damage = 0;
    private float radius = 0;

    private void Start() {
        radius = RangeTypeData.GetDataByID(rangeType);
        transform.localScale = new Vector3(radius*2,radius*2,1);

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= radius)
            {
                enemy.gameObject.GetComponent<HP>().TakeDamage(Damage);
            }
        }

        StartCoroutine("Blasting");
    }

    public void SetDamage(float Damage)
    {
        this.Damage = Damage;
    }

    private IEnumerator Blasting()
    {
        yield return new WaitForSeconds(BlastingTime);
        Destroy(this.gameObject);
    }
}
