using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_blast : MonoBehaviour
{
    [SerializeField] private int Radius = 0;
    [SerializeField] private float BlastingTime = 0f;
    private int Damage = 0;

    private void Start() {
        transform.localScale = new Vector3(Radius*2,Radius*2,1);

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= Radius)
            {
                enemy.gameObject.GetComponent<HP>().TakeDamage(Damage);
            }
        }

        StartCoroutine("Blasting");
    }

    public void SetDamage(int Damage)
    {
        this.Damage = Damage;
    }

    private IEnumerator Blasting()
    {
        yield return new WaitForSeconds(BlastingTime);
        Destroy(this.gameObject);
    }
}
