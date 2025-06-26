using System.Collections;
using System.Collections.Generic;
using Data.Enums;
using EntityResources;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class SpellOLD : MonoBehaviour
{
    //[SerializeField] private SpellTypeData _data;
    [FormerlySerializedAs("_spellType")] [SerializeField] private SpellName _spellName;
    [SerializeField] private float _spellSpeed = 0;
    [SerializeField] private GameObject _entity;
    [SerializeField] private float _defaultCritChance;
    [SerializeField] private float _defaultCritMultyply;
    
    private Rigidbody2D _spellrb = null;
    private Enemy _target = null;
    private float _distanceToTarget = 0;
    private Vector2 _direction = new Vector2(0,0);
    private float _angle = 0;
    private float _spellDamage;
    private bool _isCasted = false;
    private float _critChance;
    private float _critMultyply;
    private float _chunckdamage;

    private float _minimumDist = 0.3f;
    private float _instantSpellsDuration = 0.2f;
    
    private PlayerInputActions _playerInputActions;
    private bool _isDragging = true;
    private Camera _cam;

    private List<Enemy> _enemiesToFollow = new List<Enemy>();

    private void Awake()
    {
        _cam = Camera.main;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.CastInterrupt.started += CancelPlacing;
        
        switch (_spellName)
        {
            case SpellName.Firewall:
                _playerInputActions.UI.Lbm.started += PlaceFirewall;
                break;
            
            case SpellName.DeathZone:
                _playerInputActions.UI.Lbm.started += PlaceDeathZone;
                break;
        }
    }
    
    private void OnEnable()
    {
        if (_spellName == SpellName.Firewall)
        {
            _playerInputActions.UI.Enable();
            _playerInputActions.Player.Enable();
        }
    }

    private void OnDisable()
    {
        if (_spellName == SpellName.Firewall)
        {
            _playerInputActions.UI.Disable();
            _playerInputActions.Player.Disable();
        }
    }
    
    private void Start() {
        _spellrb = GetComponent<Rigidbody2D>();
        if (_spellName == SpellName.AvalancheCoreChunk)
        {
            _spellDamage = _chunckdamage;
        }
        else
        {
            //_spellDamage = _data.GetDataByType(_spellType).Damage;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_spellName == SpellName.FireSpirit)
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
        if (_spellName == SpellName.FireSpirit)
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
        if (_target == null)
        {
            switch (_spellName)
            {
                case SpellName.Firewall:
                    if (_isDragging)
                    {
                        Vector3 point = _cam.ScreenToWorldPoint(_playerInputActions.UI.MousePosition.ReadValue<Vector2>());
                        point.z = 0f;
                        transform.position = point;
                    }
                    break;
                case SpellName.FireSpirit:
                    if (_enemiesToFollow.Count > 0)
                    {
                        float shortiestdistance = 9999999f;
                        foreach (var enemy in _enemiesToFollow)
                        {
                            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                            if (distanceToEnemy < shortiestdistance)
                            {
                                shortiestdistance = distanceToEnemy;
                                _target = enemy;
                            }
                        }
                    }
                    break;
                case SpellName.Boom:
                    PlaceEntity();
                    break;
                case SpellName.FlashFreeze:
                    PlaceEntity();
                    break;
                default:
                    Destroy(this.gameObject);
                    break;
            }
        }
        else
        {
            switch (_spellName)
            {
                case SpellName.FireSpirit:
                    float shortiestdistance = float.MaxValue;
                    foreach (var enemy in _enemiesToFollow)
                    {
                        float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < shortiestdistance)
                        {
                            shortiestdistance = distanceToEnemy;
                            _target = enemy;
                        }
                    }
                    break;
                case SpellName.Zap:
                    this.GetComponent<LineRenderer>().SetPosition(1,_target.transform.position);
                    if (!_isCasted)
                    {
                        _target.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _critMultyply, _critChance);
                        _isCasted = true;
                    }
                    StartCoroutine("InstantSpellAnimation");
                    break;
                case SpellName.FireLaser:
                    this.GetComponent<LineRenderer>().SetPosition(1,_target.transform.position);
                    if (!_isCasted)
                    {
                        _target.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _critMultyply, _critChance);
                        _isCasted = true;
                    }
                    StartCoroutine("InstantSpellAnimation");
                    break;
                case SpellName.CryoLeach:
                    this.GetComponent<LineRenderer>().SetPosition(1,_target.transform.position);
                    Player player = FindObjectOfType<Player>();
                    if (!_isCasted)
                    {
                        //Target.gameObject.GetComponent<HP>().TryToTakeCriticalDamage(SpellDamage, critMultyply, critChance);
                        _target.GetComponent<Buff>().GetBuff(BuffType.StasisFreeze,_target);
                        player.ElementalInvocation(2);
                        _isCasted = true;
                    }
                    StartCoroutine("InstantSpellAnimation");
                    break;
            }
        }
    }

    private void FixedUpdate() {
        if (_target == null)
        {
            
        }
        else
        {
            switch (_spellName)
            {
                case SpellName.Fireball:
                    _distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
                    if (_distanceToTarget < _minimumDist)
                    {
                        PlaceEntity();
                    }
                    _direction = _target.transform.position - transform.position;
                    _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    _spellrb.velocity = _direction.normalized * _spellSpeed;
                    transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    break;
                //direction = direction.normalized * (SpellSpeed * Time.deltaTime);
                //transform.Translate(direction, Space.World);
                case SpellName.FrostWhirlwind:
                    _distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
                    if (_distanceToTarget < _minimumDist)
                    {
                        _target.gameObject.GetComponent<Debuff>().DebuffTarget(DebuffType.Slow, _target);
                        _target.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _critMultyply, _critChance);
                        Destroy(this.gameObject);
                    }
                    _direction = _target.transform.position - transform.position;
                    _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    _spellrb.velocity = _direction.normalized * _spellSpeed;
                    transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    break;
                case SpellName.Spike:
                    _distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
                    if (_distanceToTarget < _minimumDist)
                    {
                        _target.gameObject.GetComponent<Debuff>().DebuffTarget(DebuffType.Poison, _target);
                        _target.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _critMultyply, _critChance);
                        Destroy(this.gameObject);
                    }
                    _direction = _target.transform.position - transform.position;
                    _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    _spellrb.velocity = _direction.normalized * _spellSpeed;
                    transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    break;
                case SpellName.FireSpirit:
                    _distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
                    if (_distanceToTarget < _minimumDist)
                    {
                        PlaceEntity();
                    }
                    _direction = _target.transform.position - transform.position;
                    _spellrb.velocity = _direction.normalized * _spellSpeed;
                    break;
                case SpellName.FireMark:
                    _distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
                    if (_distanceToTarget < _minimumDist)
                    {
                        _target.gameObject.GetComponent<Debuff>().DebuffTarget(DebuffType.FireMark, _target);
                        _target.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _critMultyply, _critChance);
                        Destroy(this.gameObject);
                    }
                    _direction = _target.transform.position - transform.position;
                    _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    _spellrb.velocity = _direction.normalized * _spellSpeed;
                    transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    break;
                case SpellName.IcicleBarrage:
                    _distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
                    if (_distanceToTarget < _minimumDist)
                    {
                        _target.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _critMultyply, _critChance);
                        Destroy(this.gameObject);
                    }
                    _direction = _target.transform.position - transform.position;
                    _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    _spellrb.velocity = _direction.normalized * _spellSpeed;
                    transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    break;
                case SpellName.AvalancheCore:
                    _distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
                    if (_distanceToTarget < _minimumDist)
                    {
                        _target.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _critMultyply, _critChance);
                        PlaceEntity();
                    }
                    _direction = _target.transform.position - transform.position;
                    _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    _spellrb.velocity = _direction.normalized * _spellSpeed;
                    transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    break;
                case SpellName.AvalancheCoreChunk:
                    _distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
                    if (_distanceToTarget < _minimumDist)
                    {
                        _target.gameObject.GetComponent<Hp>().TryToTakeCriticalDamage(_spellDamage, _critMultyply, _critChance);
                        Destroy(this.gameObject);
                    }
                    _direction = _target.transform.position - transform.position;
                    _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    _spellrb.velocity = _direction.normalized * _spellSpeed;
                    transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    break;
            }
        }
    }

    private void PlaceEntity()
    {
        GameObject entity = Instantiate(_entity, transform.position, Quaternion.identity);
        FbBlast fireballBlast = entity.GetComponent<FbBlast>();
        Firewall firewall = entity.GetComponent<Firewall>();
        Freeze freeze = entity.GetComponent<Freeze>();
        AvalancheCoreChuncker avalancheCoreChuncker = entity.GetComponent<AvalancheCoreChuncker>();
        if (fireballBlast)
        {
            fireballBlast.SetDamage(this._spellDamage, this._critMultyply,this._critChance);
        }
        else if (firewall)
        {
            firewall.SetDamage(this._spellDamage, this._critMultyply,this._critChance);
        }
        else if (freeze)
        {
            freeze.SetDamage(this._spellDamage, _critMultyply, _critChance);
        }
        else if (avalancheCoreChuncker)
        {
            avalancheCoreChuncker.SetDamage(this._spellDamage, _critMultyply, _critChance);
            avalancheCoreChuncker.SetOrigin(_target);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator InstantSpellAnimation()
    {
        yield return new WaitForSeconds(_instantSpellsDuration);
        Destroy(this.gameObject);
    }
    
    private void PlaceFirewall(InputAction.CallbackContext context)
    {
        _isDragging = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1);
        PlaceEntity();
    }
   
    private void PlaceDeathZone(InputAction.CallbackContext context)
    {
        ///////////////////
    }

    private void CancelPlacing(InputAction.CallbackContext context)
    {
        Destroy(this.gameObject);
    }

    public void SetTarget (Enemy target)
    {
        this._target = target;
    }

    public Enemy GetTarget()
    {
        return this._target;
    }

    public void AdjustCrit(float multAdjust, float chanceAdjust)
    {
        ResetCritAdjust();
        _critMultyply = multAdjust * _defaultCritMultyply;
        _critChance = chanceAdjust * _defaultCritChance;
    }

    public void ResetCritAdjust()
    {
        _critMultyply = _defaultCritMultyply;
        _critChance = _defaultCritChance;
    }

    public void ChunkDamage(float damage)
    {
        if (damage > 0)
        {
            _chunckdamage = damage;
        }
    }
}
