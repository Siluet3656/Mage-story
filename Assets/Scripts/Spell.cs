using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float SpellSpeed = 0;
    [SerializeField] private int FireballDamage = 0;
    [SerializeField] private GameObject FB_blastPrefub = null;
    private Rigidbody2D spellrb = null;
    private Enemy Target = null;
    private float distanceToTarget = 0;
    Vector2 direction = new Vector2(0,0);
    float angle = 0;

    private float MinimumDist = 0.3f;

    private void Start() {
        spellrb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {

        if (Target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);
            if (distanceToTarget < MinimumDist)
            {
                Blast();
            }

            direction = Target.transform.position - transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            spellrb.velocity = direction.normalized * SpellSpeed;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void Blast()
    {
        if(Target != null)
        {
            Instantiate(FB_blastPrefub, transform.position, Quaternion.identity).GetComponent<FB_blast>().SetDamage(FireballDamage);
            Destroy(this.gameObject);
        }
    }

    public void SetTarget (Enemy Target)
    {
        this.Target = Target;
    }

    public Enemy GetTarget()
    {
        return this.Target;
    }
}
