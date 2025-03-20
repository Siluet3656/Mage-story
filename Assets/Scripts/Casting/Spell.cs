using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Spell : MonoBehaviour
{
    [SerializeField] private SpellTypeData data;
    [SerializeField] private SpellType spellType;
    [SerializeField] private float SpellSpeed = 0;
    [SerializeField] private GameObject Entity;
    
    private Rigidbody2D spellrb = null;
    private Enemy Target = null;
    private float distanceToTarget = 0;
    private Vector2 direction = new Vector2(0,0);
    private float angle = 0;
    private float SpellDamage;
    private bool isCasted = false;

    private float MinimumDist = 0.2f;
    private float InstantSpellsDuration = 0.2f;
    
    private PlayerInputActions _playerInputActions;
    private bool _isDragging = true;
    private Camera cam;

    private void Awake()
    {
        if (spellType == SpellType.Firewall)
        {
            cam = Camera.main;
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.UI.LBM.started += PlaceFirewall;
        }
    }
    
    private void OnEnable()
    {
        _playerInputActions.UI.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.UI.Disable();
    }
    
    private void Start() {
        spellrb = GetComponent<Rigidbody2D>();
        SpellDamage = data.GetDataByType(spellType).Damage;
    }

    private void Update()
    {
        if (spellType == SpellType.Firewall)
        {
            if (_isDragging)
            {
                Vector3 point = cam.ScreenToWorldPoint(_playerInputActions.UI.MousePosition.ReadValue<Vector2>());
                point.z = 0f;
                transform.position = point;
            }
        }
    }

    private void FixedUpdate() {
        if (Target == null)
        {
            switch (spellType)
            {
                case SpellType.Boom:
                    PlaceEntity();
                    break;
                case SpellType.Firewall:
                    break;
                default:
                    Destroy(this.gameObject);
                    break;
            }
        }
        else
        {
            switch (spellType)
            {
                case SpellType.Fireball:
                    distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);
                    if (distanceToTarget < MinimumDist)
                    {
                        PlaceEntity();
                    }
                    direction = Target.transform.position - transform.position;
                    angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    spellrb.velocity = direction.normalized * SpellSpeed;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    break;
                case SpellType.Zap:
                    this.GetComponent<LineRenderer>().SetPosition(1,Target.transform.position);
                    if (!isCasted)
                    {
                        Target.gameObject.GetComponent<HP>().TakeDamage(this.SpellDamage);
                        isCasted = true;
                    }
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

    private void PlaceEntity()
    {
        GameObject inst = Instantiate(Entity, transform.position, Quaternion.identity);
        FB_blast fbb = inst.GetComponent<FB_blast>();
        Firewall fwl = inst.GetComponent<Firewall>();
        if (fbb)
        {
            fbb.SetDamage(this.SpellDamage);
        }
        if (fwl)
        {
            fwl.SetDamage(this.SpellDamage);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator InstantSpellAnimation()
    {
        yield return new WaitForSeconds(InstantSpellsDuration);
        Destroy(this.gameObject);
    }
    
    private void PlaceFirewall(InputAction.CallbackContext context)
    {
        _isDragging = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1);
        PlaceEntity();
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
