using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private float SpellSpeed = 0;
    [SerializeField] private int SpellDamage = 0;
    [SerializeField] private GameObject FB_blastPrefub = null;
    [SerializeField] private SpellType spellType;
    private Rigidbody2D spellrb = null;
    private Enemy Target = null;
    private float distanceToTarget = 0;
    Vector2 direction = new Vector2(0,0);
    float angle = 0;

    private float MinimumDist = 0.2f;

    private void Start() {
        spellrb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (Target == null)
        {
            Destroy(this.gameObject);
        }
        switch (spellType)
        {
            case SpellType.Fireball:
                distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);
                if (distanceToTarget < MinimumDist)
                {
                    Blast();
                }
                direction = Target.transform.position - transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spellrb.velocity = direction.normalized * SpellSpeed;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            case SpellType.Zap:
                //HP.TakeDamage(5);
                //enemy.Slow(2.0f);
                break;
            case SpellType.frost_whirlwind:
                distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);
                if (distanceToTarget < MinimumDist)
                {
                    Target.gameObject.GetComponent<Debuff>().DebuffTarget(DebuffType.Slow, Target);
                    Target.gameObject.GetComponent<HP>().TakeDamage(this.SpellDamage);
                    Destroy(this.gameObject);
                }
                direction = Target.transform.position - transform.position;
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spellrb.velocity = direction.normalized * SpellSpeed;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            case SpellType.Spike:
                //enemy.ApplyPoison(5, 3);
                break;
        }
    }

    private void Blast()
    {
        if(Target != null)
        {
            Instantiate(FB_blastPrefub, transform.position, Quaternion.identity).GetComponent<FB_blast>().SetDamage(this.SpellDamage);
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
