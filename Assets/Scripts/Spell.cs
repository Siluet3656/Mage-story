using System.Collections;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private SpellType spellType;
    [Space]
    [SerializeField] private int SpellDamage = 0;
    [SerializeField] private float SpellSpeed = 0;
    [SerializeField] private GameObject FB_blastPrefub = null;
    
    private Rigidbody2D spellrb = null;
    private Enemy Target = null;
    private float distanceToTarget = 0;
    Vector2 direction = new Vector2(0,0);
    float angle = 0;

    private float MinimumDist = 0.2f;
    private float InstantSpellsDuration = 0.2f;

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
                    this.GetComponent<LineRenderer>().SetPosition(1,Target.transform.position);
                    StartCoroutine("InstantSpellAnimation");
                    break;
                case SpellType.Frost_whirlwind:
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
                    distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);
                    if (distanceToTarget < MinimumDist)
                    {
                        Target.gameObject.GetComponent<Debuff>().DebuffTarget(DebuffType.Poison, Target);
                        Target.gameObject.GetComponent<HP>().TakeDamage(this.SpellDamage);
                        Destroy(this.gameObject);
                    }
                    direction = Target.transform.position - transform.position;
                    angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    spellrb.velocity = direction.normalized * SpellSpeed;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    break;
            }
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

    private IEnumerator InstantSpellAnimation()
    {
        yield return new WaitForSeconds(InstantSpellsDuration);
        Target.gameObject.GetComponent<HP>().TakeDamage(this.SpellDamage);
        Destroy(this.gameObject);
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
