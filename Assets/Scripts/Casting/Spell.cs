using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Spell : MonoBehaviour
{
    [SerializeField] private SpellTypeData data;
    [SerializeField] private SpellType spellType;
    [SerializeField] private float SpellSpeed = 0;
    [SerializeField] private GameObject Entity;
    [SerializeField] private float defaultCritChance;
    [SerializeField] private float defaultCritMultyply;
    
    private Rigidbody2D spellrb = null;
    private Enemy Target = null;
    private float distanceToTarget = 0;
    private Vector2 direction = new Vector2(0,0);
    private float angle = 0;
    private float SpellDamage;
    private bool isCasted = false;
    private float critChance;
    private float critMultyply;

    private float MinimumDist = 0.3f;
    private float InstantSpellsDuration = 0.2f;
    
    private PlayerInputActions _playerInputActions;
    private bool _isDragging = true;
    private Camera cam;

    private List<Enemy> _enemiesToFollow = new List<Enemy>();

    private void Awake()
    {
        if (spellType == SpellType.Firewall)
        {
            cam = Camera.main;
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.UI.LBM.started += PlaceFirewall;
            _playerInputActions.Player.CastInterrupt.started += CancelFirewallPlacing;
        }
    }
    
    private void OnEnable()
    {
        if (spellType == SpellType.Firewall)
        {
            _playerInputActions.UI.Enable();
            _playerInputActions.Player.Enable();
        }
    }

    private void OnDisable()
    {
        if (spellType == SpellType.Firewall)
        {
            _playerInputActions.UI.Disable();
            _playerInputActions.Player.Disable();
        }
    }
    
    private void Start() {
        spellrb = GetComponent<Rigidbody2D>();
        SpellDamage = data.GetDataByType(spellType).Damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (spellType == SpellType.Firespirit)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
            {
                _enemiesToFollow.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (spellType == SpellType.Firespirit)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
            {
                _enemiesToFollow.Remove(enemy);
            }
        }
    }

    private void Update()
    {
        if (Target == null)
        {
            switch (spellType)
            {
                case SpellType.Firewall:
                    if (_isDragging)
                    {
                        Vector3 point = cam.ScreenToWorldPoint(_playerInputActions.UI.MousePosition.ReadValue<Vector2>());
                        point.z = 0f;
                        transform.position = point;
                    }
                    break;
                case SpellType.Firespirit:
                    if (_enemiesToFollow.Count > 0)
                    {
                        float shortiestdistance = 9999999f;
                        foreach (var enemy in _enemiesToFollow)
                        {
                            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                            if (distanceToEnemy < shortiestdistance)
                            {
                                shortiestdistance = distanceToEnemy;
                                Target = enemy;
                            }
                        }
                    }
                    break;
                case SpellType.Boom:
                    PlaceEntity();
                    break;
                case SpellType.FlashFreeze:
                    PlaceEntity();
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
                case SpellType.Firespirit:
                    float shortiestdistance = 9999999f;
                    foreach (var enemy in _enemiesToFollow)
                    {
                        float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < shortiestdistance)
                        {
                            shortiestdistance = distanceToEnemy;
                            Target = enemy;
                        }
                    }
                    break;
                case SpellType.Zap:
                    this.GetComponent<LineRenderer>().SetPosition(1,Target.transform.position);
                    if (!isCasted)
                    {
                        Target.gameObject.GetComponent<HP>().TryToTakeCriticalDamage(SpellDamage, critMultyply, critChance);
                        isCasted = true;
                    }
                    StartCoroutine("InstantSpellAnimation");
                    break;
                case SpellType.Firelaser:
                    this.GetComponent<LineRenderer>().SetPosition(1,Target.transform.position);
                    if (!isCasted)
                    {
                        Target.gameObject.GetComponent<HP>().TryToTakeCriticalDamage(SpellDamage, critMultyply, critChance);
                        isCasted = true;
                    }
                    StartCoroutine("InstantSpellAnimation");
                    break;
            }
        }
    }

    private void FixedUpdate() {
        if (Target == null)
        {
            
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
                case SpellType.Frost_whirlwind:
                    distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);
                    if (distanceToTarget < MinimumDist)
                    {
                        Target.gameObject.GetComponent<Debuff>().DebuffTarget(DebuffType.Slow, Target);
                        Target.gameObject.GetComponent<HP>().TryToTakeCriticalDamage(SpellDamage, critMultyply, critChance);
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
                        Target.gameObject.GetComponent<HP>().TryToTakeCriticalDamage(SpellDamage, critMultyply, critChance);
                        Destroy(this.gameObject);
                    }
                    direction = Target.transform.position - transform.position;
                    angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    spellrb.velocity = direction.normalized * SpellSpeed;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    break;
                case SpellType.Firespirit:
                    distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);
                    if (distanceToTarget < MinimumDist)
                    {
                        PlaceEntity();
                    }
                    direction = Target.transform.position - transform.position;
                    spellrb.velocity = direction.normalized * SpellSpeed;
                    break;
                case SpellType.Firemark:
                    distanceToTarget = Vector2.Distance(transform.position, Target.transform.position);
                    if (distanceToTarget < MinimumDist)
                    {
                        Target.gameObject.GetComponent<Debuff>().DebuffTarget(DebuffType.FireMark, Target);
                        Target.gameObject.GetComponent<HP>().TryToTakeCriticalDamage(SpellDamage, critMultyply, critChance);
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
        Freeze frz = inst.GetComponent<Freeze>();
        if (fbb)
        {
            fbb.SetDamage(this.SpellDamage, this.critMultyply,this.critChance);
        }
        else if (fwl)
        {
            fwl.SetDamage(this.SpellDamage, this.critMultyply,this.critChance);
        }
        else if (frz)
        {
            frz.SetDamage(this.SpellDamage, critMultyply, critChance);
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

    private void CancelFirewallPlacing(InputAction.CallbackContext context)
    {
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

    public void AdjustCrit(float multAdjust, float chanceAdjust)
    {
        ResetCritAdjust();
        critMultyply = multAdjust * defaultCritMultyply;
        critChance = chanceAdjust * defaultCritChance;
    }

    public void ResetCritAdjust()
    {
        critMultyply = defaultCritMultyply;
        critChance = defaultCritChance;
    }
}
