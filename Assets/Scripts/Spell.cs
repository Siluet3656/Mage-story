using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float SpellSpeed = 0;
    [SerializeField] private int FireballDamage = 0;
    private Rigidbody2D spellrb = null;
    private Enemy Target = null;

    private float MinimumDist = 0.3f;

    private void Start() {
        spellrb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        Vector2 direction = Target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        spellrb.velocity = direction.normalized * SpellSpeed;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);

        if (distanceToTarget < MinimumDist)
        {
            DoDamage();
        }
    }

    private void DoDamage()
    {
        if(Target != null)
        {
            Target.gameObject.GetComponent<HP>().TakeDamage(FireballDamage);
            Destroy(this.gameObject);
        }
    }

    public void SetTarget (Enemy Target)
    {
        this.Target = Target;
    }
}
