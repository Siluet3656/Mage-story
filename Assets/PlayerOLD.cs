using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data.Enums;
using Statuses;
using UI.Buttons;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    
    [FormerlySerializedAs("data")]
    [Header("Spell")]
    //[SerializeField] private SpellTypeData _data;
    [FormerlySerializedAs("CastBar")] [SerializeField] private Image _castBar = null;
    [FormerlySerializedAs("RemainderBar")] [SerializeField] private Image _remainderBar = null;
    [FormerlySerializedAs("spellBarCells")] [SerializeField] private SpellBarButton[] _spellBarCells;
    [FormerlySerializedAs("FireShards")] [SerializeField] private Image[] _fireShards = null;
    [FormerlySerializedAs("FrostShards")] [SerializeField] private Image[] _frostShards = null;
    [FormerlySerializedAs("EarthShards")] [SerializeField] private Image[] _earthShards = null;
    [FormerlySerializedAs("GCD_barz")] [SerializeField] private Image[] _gcdBarz;
    [FormerlySerializedAs("FS_RefreshTime")]
    [Space]
    [SerializeField] private float _fsRefreshTime = 0f;
    [FormerlySerializedAs("FrS_RefreshTime")] [SerializeField] private float _frSRefreshTime = 0f;
    [FormerlySerializedAs("ES_RefreshTime")] [SerializeField] private float _esRefreshTime = 0f;
    [FormerlySerializedAs("GCD")] [SerializeField] private float _gcd;
    [FormerlySerializedAs("iceTomb")]
    [Space] 
    [SerializeField] private GameObject _iceTomb;
    [FormerlySerializedAs("amountOfIcecles")] [SerializeField] private int _amountOfIcecles;

    private IEnumerator[] _fireShardsRefreshRoutine = new IEnumerator[3];
    private IEnumerator[] _frostShardsRefreshRoutine = new IEnumerator[3];
    private IEnumerator[] _earthShardsRefreshRoutine = new IEnumerator[3];

    private bool[] _isfireShardsRefreshRoutineStarted = new bool[3];
    private bool[] _isfrostShardsRefreshRoutineStarted = new bool[3];
    private bool[] _isearthShardsRefreshRoutineStarted = new bool[3];

    private GameObject _fireBallPrefab;
    private GameObject _zapPrefub;
    private GameObject _frostWhirlwindPrefab;
    private GameObject _spikePrefab;
    private GameObject _boomPrefub;
    private GameObject _firewallPrefub;
    private GameObject _firespiritPrefub;
    private GameObject _firelaserPrefub;
    private GameObject _firemarkPrefub;
    private GameObject _flashFreezePrefub;
    private GameObject _icecleBarragePrefub;
    private GameObject _cryoLeachPrefub;
    private GameObject _avalancheCorePrefub;
    private GameObject _deathZonePrefub;
    //private GameObject FlowerPrefub;
    

    private float _fireballCastTime;
    private float _frostWhirlwindCastTime;
    private float _spikeCastTime;
    private float _firewallCastTime;
    private float _firespiritCastTime;
    private float _firelaserCastTime;
    private float _firemarkCastTime;
    private float _icecleBarrageCastTime;
    private float _avalancheCoreCastTime;

    private Vector3Int _fireballCost;
    private Vector3Int _frostWhirlwindCost;
    private Vector3Int _spikeCost;
    private Vector3Int _boomCost;
    private Vector3Int _firewallCost;
    private Vector3Int _firespiritCost;
    private Vector3Int _firelaserCost;
    private Vector3Int _fireauraCost;
    private Vector3Int _firemarkCost;
    private Vector3Int _flashFreezeCost;
    private Vector3Int _stasisFreezeCost;
    private Vector3Int _icecleBarrageCost;
    private Vector3Int _cryoLeachCost;
    private Vector3Int _frostAegisCost;
    private Vector3Int _avalancheCoreCost;
    private Vector3Int _earthShieldCost;
    private Vector3Int _deathZoneCost;
    private float _zapCost;
    
    
    private const int MaxRemainderAmount = 100;
    private const int MaxFsAmount = 3;
    private const int MaxFrSAmount = 3;
    private const int MaxESAmount = 3;
    
    private bool _isCasting = false;
    private float _castProgress = 0f;
    private float _currentCastCastTime = 0f;
    private float[] _fsRefreshProgress = new float[MaxFsAmount];
    private float[] _frSRefreshProgress = new float[MaxFrSAmount];
    private float[] _esRefreshProgress = new float[MaxFrSAmount];
    private float _gcDprogress = 0f;
    private float _remainderAmount = 0;
    private int _fsAmount = 0;
    private int _frSAmount = 0;
    private int _esAmount = 0;
    private ShardType _lastUsedShard = ShardType.None;
    private ShardType _previosShard = ShardType.None;
    
    private float _fireCritMultAdjust = 1;
    private float _fireCritChanceAdjust = 1;
    private float _frostCritMultAdjust = 1;
    private float _frostCritChanceAdjust = 1;
    private float _earthCritMultAdjust = 1;
    private float _earthCritChanceAdjust = 1;
    private float _noelemCritMultAdjust = 1;
    private float _noelemCritChanceAdjust = 1;
    
    
    private Color _fireballCastBarColor = new Color(0.8f,0.1f,0.1f);
    private Color _firewallCastBarColor = new Color(1,0.3f,0.1f);
    private Color _fireSpiritCastBarColor = new Color(1,0.7f,0.4f);
    private Color _firelaserCastBarColor = new Color(1,0f,0.2f);
    private Color _fireMarkCastBarColor = new Color(1,0.6f,0.1f);
    private Color _frostWhirlwindCastBarColor = new Color(0.1f,0.3f,1f);
    private Color _icecleBarrageCastBarColor = new Color(0f, 0.5f, 1f);
    private Color _avalancheCoreCastBarColor = new Color(0.2f, 0.2f, 1f);
    private Color _spikeCastBarColor = new Color(0.3f,1f,0.1f);

    private Buff _playerBuffs;

    [FormerlySerializedAs("speedType")]
    [Header("Movement")] 
    [SerializeField] private SpeedType _speedType;
    [FormerlySerializedAs("BlinkRefreshBar")] [SerializeField] private Image _blinkRefreshBar;
    [FormerlySerializedAs("BlinkDist")]
    [Space]
    [SerializeField] private float _blinkDist = 0f;
    [FormerlySerializedAs("blinkCD")] [SerializeField] private float _blinkCd = 0f;
    private RaycastHit2D _blinkRch;
    private bool _isBlinked = false;
    private float _blinkRefreshProgress = 0f;
    private float _speed = 0;
    private Rigidbody2D _rb;
    private Animator _anim;
    private Vector2 _movement;
    private bool _isAvailableToMove = true;

    [FormerlySerializedAs("interactionRange")]
    [Header("Target system")]
    [SerializeField] private float _interactionRange  = 0;
    private Enemy _currentTarget = null;
    private Enemy _targetCastingTo = null;
    private List<Enemy> _enemiesInRange = new List<Enemy>();
    private int _currentIndex = 0;

    private void Awake()
    {
        /*_playerInputActions = new PlayerInputActions();
        _playerBuffs = this.GetComponent<Buff>();
        _playerInputActions.Player.Blink.started += Blink;
        _playerInputActions.Player.FastTarget.started += TrySelectTarget;
        _playerInputActions.Player.CastInterrupt.started += InterruptCast;
        _playerInputActions.Player.Castbar1.started += SpellBarButtonCast;
        _playerInputActions.Player.Castbar2.started += SpellBarButtonCast;
        _playerInputActions.Player.Castbar3.started += SpellBarButtonCast;
        _playerInputActions.Player.Castbar4.started += SpellBarButtonCast;
        _playerInputActions.Player.Castbar5.started += SpellBarButtonCast;
        _playerInputActions.Player.Castbar6.started += SpellBarButtonCast;
        _playerInputActions.Player.Castbar7.started += SpellBarButtonCast;
        _playerInputActions.UI.Lbm.started += HandleMouseClick;

        _playerBuffs.OnPlayerFreeze += FreezeMovement;
        _playerBuffs.OnPlayerUnFreeze += UnfreezeMovement;
        //Application.targetFrameRate = -1;*/
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.UI.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.UI.Disable();
    }

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _fsAmount = MaxFsAmount;
        _frSAmount = MaxFrSAmount;
        _esAmount = MaxESAmount;
        
        _isfireShardsRefreshRoutineStarted[0] = false;
        _isfireShardsRefreshRoutineStarted[1] = false;
        _isfireShardsRefreshRoutineStarted[2] = false;
        
        _isfrostShardsRefreshRoutineStarted[0] = false;
        _isfrostShardsRefreshRoutineStarted[1] = false;
        _isfrostShardsRefreshRoutineStarted[2] = false;

        _isearthShardsRefreshRoutineStarted[0] = false;
        _isearthShardsRefreshRoutineStarted[1] = false;
        _isearthShardsRefreshRoutineStarted[2] = false;
        
        _fireShardsRefreshRoutine[0] = FS_Refreshing(0);
        _fireShardsRefreshRoutine[1] = FS_Refreshing(1);
        _fireShardsRefreshRoutine[2] = FS_Refreshing(2);
        
        _frostShardsRefreshRoutine[0] = FrS_Refreshing(0);
        _frostShardsRefreshRoutine[1] = FrS_Refreshing(1);
        _frostShardsRefreshRoutine[2] = FrS_Refreshing(2);
        
        _earthShardsRefreshRoutine[0] = ES_Refreshing(0);
        _earthShardsRefreshRoutine[1] = ES_Refreshing(1);
        _earthShardsRefreshRoutine[2] = ES_Refreshing(2);
        
        for (var frp = 0; frp < MaxFsAmount; frp++)
        {
            _fsRefreshProgress[frp] = 1f;
            _frSRefreshProgress[frp] = 1f;
            _esRefreshProgress[frp] = 1f;
        }
        
        _blinkRefreshBar.fillAmount = 1f;

        _gcDprogress = 0f;
        foreach (var bar in _gcdBarz)
        {
            bar.fillAmount = _gcDprogress;
        }

        _lastUsedShard = ShardType.None;

        //SpellData data = this.data.GetDataByType(SpellType.Fireball);
        
        /*_fireBallPrefab = _data.GetDataByType(SpellType.Fireball).PrefubOfSpell;
        _frostWhirlwindPrefab = _data.GetDataByType(SpellType.FrostWhirlwind).PrefubOfSpell;
        _spikePrefab = _data.GetDataByType(SpellType.Spike).PrefubOfSpell;
        _zapPrefub = _data.GetDataByType(SpellType.Zap).PrefubOfSpell;
        _boomPrefub = _data.GetDataByType(SpellType.Boom).PrefubOfSpell;
        _firewallPrefub = _data.GetDataByType(SpellType.Firewall).PrefubOfSpell;
        _firespiritPrefub = _data.GetDataByType(SpellType.Firespirit).PrefubOfSpell;
        _firelaserPrefub = _data.GetDataByType(SpellType.Firelaser).PrefubOfSpell;
        _firemarkPrefub = _data.GetDataByType(SpellType.Firemark).PrefubOfSpell;
        _flashFreezePrefub = _data.GetDataByType(SpellType.FlashFreeze).PrefubOfSpell;
        _icecleBarragePrefub = _data.GetDataByType(SpellType.IcicleBarrage).PrefubOfSpell;
        _cryoLeachPrefub = _data.GetDataByType(SpellType.CryoLeach).PrefubOfSpell;
        _avalancheCorePrefub = _data.GetDataByType(SpellType.AvalancheCore).PrefubOfSpell;
        _deathZonePrefub = _data.GetDataByType(SpellType.DeathZone).PrefubOfSpell;
        
        _fireballCastTime = _data.GetDataByType(SpellType.Fireball).CastTime;
        _frostWhirlwindCastTime = _data.GetDataByType(SpellType.FrostWhirlwind).CastTime;
        _spikeCastTime = _data.GetDataByType(SpellType.Spike).CastTime;
        _firewallCastTime = _data.GetDataByType(SpellType.Firewall).CastTime;
        _firespiritCastTime = _data.GetDataByType(SpellType.Firespirit).CastTime;
        _firelaserCastTime = _data.GetDataByType(SpellType.Firelaser).CastTime;
        _firemarkCastTime = _data.GetDataByType(SpellType.Firemark).CastTime;
        _icecleBarrageCastTime = _data.GetDataByType(SpellType.IcicleBarrage).CastTime;
        _avalancheCoreCastTime = _data.GetDataByType(SpellType.AvalancheCore).CastTime;
        
        _fireballCost = _data.GetDataByType(SpellType.Fireball).ShardsCost;
        _frostWhirlwindCost = _data.GetDataByType(SpellType.FrostWhirlwind).ShardsCost;
        _spikeCost = _data.GetDataByType(SpellType.Spike).ShardsCost;
        _zapCost = _data.GetDataByType(SpellType.Zap).ReminderCost;
        _boomCost = _data.GetDataByType(SpellType.Boom).ShardsCost;
        _firewallCost = _data.GetDataByType(SpellType.Firewall).ShardsCost;
        _firespiritCost = _data.GetDataByType(SpellType.Firespirit).ShardsCost;
        _firelaserCost = _data.GetDataByType(SpellType.Firelaser).ShardsCost;
        _fireauraCost = _data.GetDataByType(SpellType.Fireaura).ShardsCost;
        _firemarkCost = _data.GetDataByType(SpellType.Firemark).ShardsCost;
        _flashFreezeCost = _data.GetDataByType(SpellType.FlashFreeze).ShardsCost;
        _stasisFreezeCost = _data.GetDataByType(SpellType.StasisFreeze).ShardsCost;
        _icecleBarrageCost = _data.GetDataByType(SpellType.IcicleBarrage).ShardsCost;
        _cryoLeachCost = _data.GetDataByType(SpellType.CryoLeach).ShardsCost;
        _frostAegisCost = _data.GetDataByType(SpellType.FrostAegis).ShardsCost;
        _avalancheCoreCost = _data.GetDataByType(SpellType.AvalancheCore).ShardsCost;
        _earthShieldCost = _data.GetDataByType(SpellType.EarthShield).ShardsCost;
        _deathZoneCost = _data.GetDataByType(SpellType.DeathZone).ShardsCost;*/
    }

    private void Update()
    {
        if (_isAvailableToMove)
        {
            _movement = _playerInputActions.Player.Movement.ReadValue<Vector2>();
        }
        else
        {
            _movement = Vector2.zero;
        }
        
        _anim.SetFloat("MoveX", _movement.x);
        _anim.SetFloat("MoveY",  _movement.y);
        
        _remainderBar.fillAmount = _remainderAmount/MaxRemainderAmount;

        if (_isCasting)
        {
            //Speed = SpeedTypeData.GetDataByType(speedType - 1);
            if (_castProgress <= 1f)
            {
                _castProgress += 1f / _currentCastCastTime * Time.deltaTime;
                _castBar.fillAmount = _castProgress;
            }
            else
            {
                _castBar.fillAmount = 1f;
            }
        }
        else
        {
            //Speed = SpeedTypeData.GetDataByType(speedType);
        }

        int j = 0;
        foreach (Image shard in _fireShards)
        {
            if (_fsRefreshProgress[j] < 1f)
            {
                _fsRefreshProgress[j] += 1f / _fsRefreshTime * Time.deltaTime;
                shard.fillAmount = _fsRefreshProgress[j];
            }
            j++;
        }
        j = 0;
        foreach (Image shard in _frostShards)
        {
            if (_frSRefreshProgress[j] < 1f)
            {
                _frSRefreshProgress[j] += 1f / _frSRefreshTime * Time.deltaTime;
                shard.fillAmount = _frSRefreshProgress[j];
            }
            j++;
        }
        j = 0;
        foreach (Image shard in _earthShards)
        {
            if (_esRefreshProgress[j] < 1f)
            {
                _esRefreshProgress[j] += 1f / _esRefreshTime * Time.deltaTime;
                shard.fillAmount = _esRefreshProgress[j];
            }
            j++;
        }
        
        if (_gcDprogress >= 0)
        {
            _gcDprogress -= 1f / _gcd * Time.deltaTime;
        }
        else
        {
            _gcDprogress = 0;
        }
        foreach (Image bar in _gcdBarz)
        {
            bar.fillAmount = _gcDprogress;
        }
        
        if (_currentTarget != null)
        {
            CheckTargetDistance();
        }
        else
        {
            CheckTargetCastingToDistance();
        }
        
        if (_isBlinked)
        {
            if (_blinkRefreshProgress <= 1f)
            {
                _blinkRefreshProgress += 1f / _blinkCd * Time.deltaTime;
                _blinkRefreshBar.fillAmount = _blinkRefreshProgress;
            }
            else
            {
                _blinkRefreshBar.fillAmount = 1f;
            }
        }
    }

    private void FixedUpdate()
    {
        this._rb.MovePosition(this._rb.position + this._movement * (_speed * Time.fixedDeltaTime));
    }

    private void TrySelectTarget(InputAction.CallbackContext context)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        _enemiesInRange.Clear();
        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= _interactionRange)
            {
                _enemiesInRange.Add(enemy);
            }
        }

        if (_enemiesInRange.Count > 0)
        {
            _enemiesInRange = _enemiesInRange.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position)).ToList();

            if (_currentTarget != null)
            {
                _currentIndex = _enemiesInRange.IndexOf(_currentTarget);
                ClearTarget();
                
                if (_currentIndex == -1 || _currentIndex >= _enemiesInRange.Count - 1)
                {
                    _currentTarget = _enemiesInRange[0];
                    _enemiesInRange[0].Target();
                }
                else
                {
                    _currentTarget = _enemiesInRange[_currentIndex + 1];
                    _enemiesInRange[_currentIndex + 1].Target();
                }
            }
            else
            {
                _currentTarget = _enemiesInRange[0];
                _enemiesInRange[0].Target();
            }
        }
    }

    private void CheckTargetDistance()
    {
        float distanceToTarget = Vector2.Distance(this.transform.position, _currentTarget.transform.position);
            if (distanceToTarget > _interactionRange)
            {
                ClearTarget();
                StopAllCasts();
            }
    }

    private void CheckTargetCastingToDistance()
    {
        if (_isCasting)
        {
            if (_targetCastingTo != null)
            {
                float distanceToTarget = Vector2.Distance(this.transform.position, _targetCastingTo.transform.position);
                if (distanceToTarget > _interactionRange)
                {
                    StopAllCasts();
                }
            }
        }
    }

    private void HandleMouseClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(_playerInputActions.UI.MousePosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            ClearTarget();
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Target();
                _currentTarget = enemy;
            }
        }
        else
        {
            ClearTarget();
        }
    }

    public void ClearTarget()
    {
        if (_currentTarget != null)
        { 
            _currentTarget.ResetTarget();
            _currentTarget = null;
        }
    }
    
    private void CastStop()
    {
        _targetCastingTo = null;
        _isCasting = false;
        _castProgress = 0f;
        _castBar.fillAmount = _castProgress;
    }

    private void InterruptCast(InputAction.CallbackContext context)
    {
        if (_isCasting)
        {
            StopAllCasts();
            GCDstop();
        }
    }

    public void ElementalInvocation(int amountOfInvocations)
    {
        List<ShardType> shardsToInvoke = new List<ShardType>();
        switch (amountOfInvocations)
        {
            case 1:
                shardsToInvoke.Add(_lastUsedShard);
                break;
            case 2:
                shardsToInvoke.Add(_lastUsedShard);
                shardsToInvoke.Add(_previosShard);
                break;
        }

        foreach (var shard in shardsToInvoke)
        {
            switch (shard)
            {
                case ShardType.FireShard:
                    for (int i = 0; i < _fireShardsRefreshRoutine.Length; i++)
                    {
                        if (_isfireShardsRefreshRoutineStarted[i])
                        {
                            _fireShards[i].fillAmount = 1f;
                            _fsRefreshProgress[i] = 1f;

                            StopCoroutine(_fireShardsRefreshRoutine[i]);

                            _isfireShardsRefreshRoutineStarted[i] = false;
                            _fireShardsRefreshRoutine[i] = FS_Refreshing(i);
                            GetFireShard();
                            break;
                        }
                    }
                    break;
                
                case ShardType.FrostShard:
                    for (int i = 0; i < _frostShardsRefreshRoutine.Length; i++)
                    {
                        if (_isfrostShardsRefreshRoutineStarted[i])
                        {
                            _frostShards[i].fillAmount = 1f;
                            _frSRefreshProgress[i] = 1f;

                            StopCoroutine(_frostShardsRefreshRoutine[i]);

                            _isfrostShardsRefreshRoutineStarted[i] = false;
                            _frostShardsRefreshRoutine[i] = FrS_Refreshing(i);
                            GetFrostShard();
                            break;
                        }
                    }
                    break;
                
                case ShardType.EarthShard:
                    for (int i = 0; i < _earthShardsRefreshRoutine.Length; i++)
                    {
                        if (_isearthShardsRefreshRoutineStarted[i])
                        {
                            _earthShards[i].fillAmount = 1f;
                            _esRefreshProgress[i] = 1f;

                            StopCoroutine(_earthShardsRefreshRoutine[i]);

                            _isearthShardsRefreshRoutineStarted[i] = false;
                            _earthShardsRefreshRoutine[i] = ES_Refreshing(i);
                            GetEarthShard();
                            break;
                        }
                    }
                    break;
            }
        }
        
    }

    public void SetCritAdjustFire(float mult, float chance)
    {
        _fireCritMultAdjust = mult;
        _fireCritChanceAdjust = chance;
    }
    
    public void SetCritAdjustFrost(float mult, float chance)
    {
        _frostCritMultAdjust = mult;
        _frostCritChanceAdjust = chance;
    }
    
    public void SetCritAdjustEarth(float mult, float chance)
    {
        _earthCritMultAdjust = mult;
        _earthCritChanceAdjust = chance;
    }
    
    public void SetCritAdjustNoelem(float mult, float chance)
    {
        _noelemCritMultAdjust = mult;
        _noelemCritChanceAdjust = chance;
    }

    public void FreezeMovement()
    {
        _isAvailableToMove = false;
        _iceTomb.SetActive(true);
    }
    
    public void UnfreezeMovement()
    {
        _isAvailableToMove = true;
        _iceTomb.SetActive(false);
    }
    
    public void StopAllCasts()
    {
        StopCoroutine("FireballCast");
        StopCoroutine("frost_whirlwindCast");
        StopCoroutine("SpikeCast");
        StopCoroutine("FirewallCast");
        StopCoroutine("FireSpiritCast");
        StopCoroutine("FirelaserCast");
        StopCoroutine("FireMarkCast");
        StopCoroutine("IcecleBarrageCast");
        StopCoroutine("AvalancheCoreCast");
        CastStop();
    }
/*
    public void CastSpell(SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.Fireball:
                if (IsEnoughShards(_fireballCost))
                {
                    if (_currentTarget != null)
                    {
                        _currentCastCastTime = _fireballCastTime;
                        _castBar.color = _fireballCastBarColor;
                        StartCoroutine("FireballCast");
                    }
                }
                break;
            case SpellType.FrostWhirlwind:
                if (IsEnoughShards(_frostWhirlwindCost))
                {
                    if (_currentTarget != null)
                    {
                        _currentCastCastTime = _frostWhirlwindCastTime;
                        _castBar.color = _frostWhirlwindCastBarColor;
                        StartCoroutine("frost_whirlwindCast");
                    }
                }
                break;
            case SpellType.Spike:
                if (IsEnoughShards(_spikeCost)) 
                {
                    if (_currentTarget != null)
                    {
                        _currentCastCastTime = _spikeCastTime;
                        _castBar.color = _spikeCastBarColor;
                        StartCoroutine("SpikeCast");
                    }
                }
                break;
            case SpellType.Zap:
                if (_remainderAmount >= _zapCost) 
                {
                    if (_currentTarget != null)
                    {
                        ZapCast();
                    }
                }
                break;
            case SpellType.Boom:
                if (IsEnoughShards(_boomCost))
                {
                    BoomCast();
                }
                break;
            case SpellType.Firewall:
                if (IsEnoughShards(_firewallCost))
                {
                    _currentCastCastTime = _firewallCastTime;
                    _castBar.color = _firewallCastBarColor;
                    StartCoroutine("FirewallCast");
                }
                break;
            case SpellType.Firespirit:
                if (IsEnoughShards(_firespiritCost))
                {
                    _currentCastCastTime = _firespiritCastTime;
                    _castBar.color = _fireSpiritCastBarColor;
                    StartCoroutine("FireSpiritCast");
                }
                break; 
            case SpellType.Firelaser:
                if (IsEnoughShards(_firelaserCost))
                {
                    if (_currentTarget != null)
                    {
                        _currentCastCastTime = _firelaserCastTime;
                        _castBar.color = _firelaserCastBarColor;
                        StartCoroutine("FirelaserCast");
                    }
                }
                break;
            case SpellType.Fireaura:
                if (IsEnoughShards(_fireauraCost))
                {
                    FireAuraCast();
                }
                break;
            case SpellType.Firemark:
                if (IsEnoughShards(_firemarkCost))
                {
                    if (_currentTarget != null)
                    {
                        _currentCastCastTime = _firemarkCastTime;
                        _castBar.color = _fireMarkCastBarColor;
                        StartCoroutine("FireMarkCast");
                    }
                }
                break;
            case SpellType.FlashFreeze:
                if (IsEnoughShards(_flashFreezeCost))
                {
                    FlashFreezeCast();
                }
                break;
            case SpellType.StasisFreeze:
                if (IsEnoughShards(_stasisFreezeCost))
                {
                    StasisFreezeCast();
                }
                break;
            case SpellType.IcicleBarrage:
                if (IsEnoughShards(_icecleBarrageCost)) 
                {
                    if (_currentTarget != null)
                    {
                        _currentCastCastTime = _icecleBarrageCastTime;
                        _castBar.color = _icecleBarrageCastBarColor;
                        StartCoroutine("IcecleBarrageCast");
                    }
                }
                break;
            case SpellType.CryoLeach:
                if (IsEnoughShards(_cryoLeachCost)) 
                {
                    if (_currentTarget != null)
                    {
                        CryoLeachCast();
                    }
                }
                break;
            case SpellType.FrostAegis:
                if (IsEnoughShards(_frostAegisCost))
                {
                    FrostAegisCast();
                }
                break;
            case SpellType.AvalancheCore:
                if (IsEnoughShards(_avalancheCoreCost))
                {
                    if (_currentTarget != null)
                    {
                        _currentCastCastTime = _avalancheCoreCastTime;
                        _castBar.color = _avalancheCoreCastBarColor;
                        StartCoroutine("AvalancheCoreCast");
                    }
                }
                break;
            case SpellType.EarthShield:
                if (IsEnoughShards(_earthShieldCost))
                {
                    EarthShieldCast();
                }
                break;
            case SpellType.DeathZone:
                if (IsEnoughShards(_deathZoneCost))
                {
                    DeathZoneCast();
                }
                break;
        }
    }

    private void SpellBarButtonCast(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!_isCasting)
            {
                if (_gcDprogress <= 0)
                {
                    if (_isAvailableToMove)
                    {
                        _targetCastingTo = _currentTarget;

                        switch (context.action.name)
                        {
                            case "Castbar1":
                                CastSpell(_spellBarCells[0].GetSpellType());
                                break;
                            case "Castbar2":
                                CastSpell(_spellBarCells[1].GetSpellType());
                                break;
                            case "Castbar3":
                                CastSpell(_spellBarCells[2].GetSpellType());
                                break;
                            case "Castbar4":
                                CastSpell(_spellBarCells[3].GetSpellType());
                                break;
                            case "Castbar5":
                                CastSpell(_spellBarCells[4].GetSpellType());
                                break;
                            case "Castbar6":
                                CastSpell(_spellBarCells[5].GetSpellType());
                                break;
                            case "Castbar7":
                                CastSpell(_spellBarCells[6].GetSpellType());
                                break;
                        }
                    }
                }
            }
        }
    }
*/
    /*private IEnumerator FireballCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        yield return new WaitForSeconds(_fireballCastTime);
        if (_targetCastingTo != null)
        {
            spell = Instantiate(_fireBallPrefab, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(_targetCastingTo);
            spell.AdjustCrit(_fireCritMultAdjust,_fireCritChanceAdjust);
            UseShards(_fireballCost);
        }
        CastStop();
    }

    private IEnumerator frost_whirlwindCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        yield return new WaitForSeconds(_frostWhirlwindCastTime);
        if (_targetCastingTo != null)
        {
            spell = Instantiate(_frostWhirlwindPrefab, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(_targetCastingTo);
            spell.AdjustCrit(_frostCritMultAdjust,_frostCritChanceAdjust);
            UseShards(_frostWhirlwindCost);
        }
        CastStop();
    }
    
    private IEnumerator SpikeCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        yield return new WaitForSeconds(_spikeCastTime);
        if (_targetCastingTo != null)
        {
            spell = Instantiate(_spikePrefab, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(_targetCastingTo);
            spell.AdjustCrit(_earthCritMultAdjust,_earthCritChanceAdjust);
            UseShards(_spikeCost);
        }
        CastStop();
    }

    private void ZapCast()
    {
        Spell spell;
        _isCasting = true;
        if (_targetCastingTo != null)
        {
            spell = Instantiate(_zapPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.GetComponent<LineRenderer>().SetPosition(0,this.transform.position);
            spell.GetComponent<LineRenderer>().SetPosition(1,_targetCastingTo.transform.position);
            spell.SetTarget(_targetCastingTo);
            spell.AdjustCrit(_noelemCritMultAdjust,_noelemCritChanceAdjust);
            _remainderAmount -= _zapCost;
        }
        GCDstart();
        CastStop();
    }

    private void BoomCast()
    {
        Spell spell;
        _isCasting = true;
        spell = Instantiate(_boomPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        spell.AdjustCrit(_fireCritMultAdjust,_fireCritChanceAdjust);
        UseShards(_boomCost);
        GCDstart();
        CastStop();
    }

    private void FlashFreezeCast()
    {
        Spell spell;
        _isCasting = true;
        spell = Instantiate(_flashFreezePrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        spell.AdjustCrit(_frostCritMultAdjust,_frostCritChanceAdjust);
        UseShards(_flashFreezeCost);
        GCDstart();
        CastStop();
    }

    private void StasisFreezeCast()
    {
        _isCasting = true;
        this.GetComponent<Buff>().GetBuff(BuffType.StasisFreeze, this);
        UseShards(_stasisFreezeCost);
        GCDstart();
        CastStop();
    }
    
    private IEnumerator FirewallCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(_firewallCastTime);
        
        spell = Instantiate(_firewallPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        spell.AdjustCrit(_fireCritMultAdjust,_fireCritChanceAdjust);
        UseShards(_firewallCost);
        CastStop();
    }
    
    private IEnumerator FireSpiritCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(_firespiritCastTime);
        
        spell = Instantiate(_firespiritPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        spell.AdjustCrit(_fireCritMultAdjust,_fireCritChanceAdjust);
        UseShards(_firespiritCost);
        CastStop();
    }

    private IEnumerator FirelaserCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(_firelaserCastTime);
        
        if (_targetCastingTo != null)
        {
            spell = Instantiate(_firelaserPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            spell.GetComponent<LineRenderer>().SetPosition(1, _targetCastingTo.transform.position);
            spell.SetTarget(_targetCastingTo);
            spell.AdjustCrit(_fireCritMultAdjust,_fireCritChanceAdjust);
            UseShards(_firelaserCost);
        }
        CastStop();
    }

    private void FireAuraCast()
    {
        _isCasting = true;
        this.GetComponent<Buff>().GetBuff(BuffType.FireAura, this);
        UseShards(_fireauraCost);
        GCDstart();
        CastStop();
    }

    private IEnumerator FireMarkCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(_firemarkCastTime);
        
        if (_targetCastingTo != null)
        {
            spell = Instantiate(_firemarkPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(_targetCastingTo);
            spell.AdjustCrit(_fireCritMultAdjust,_fireCritChanceAdjust);
            UseShards(_firemarkCost);
        }
        CastStop();
    }

    private IEnumerator IcecleBarrageCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        yield return new WaitForSeconds(_icecleBarrageCastTime);
        if (_targetCastingTo != null)
        {
            UseShards(_icecleBarrageCost);
            for (int i = 0; i < _amountOfIcecles; i++)
            {
                Vector2 intstpos = transform.position;
                intstpos.x += Random.Range(-0.3f, 0.3f);
                intstpos.y += Random.Range(-0.3f, 0.3f);
                spell = Instantiate(_icecleBarragePrefub, intstpos, Quaternion.identity).GetComponent<Spell>();
                spell.SetTarget(_targetCastingTo);
                spell.AdjustCrit(_frostCritMultAdjust,_frostCritChanceAdjust);
                yield return new WaitForSeconds(0.05f);
            }
        }
        CastStop();
    }

    private void CryoLeachCast()
    {
        Spell spell;
        _isCasting = true;
        if (_targetCastingTo != null)
        {
            spell = Instantiate(_cryoLeachPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.GetComponent<LineRenderer>().SetPosition(0,this.transform.position);
            spell.GetComponent<LineRenderer>().SetPosition(1,_targetCastingTo.transform.position);
            spell.SetTarget(_targetCastingTo);
            spell.AdjustCrit(_frostCritMultAdjust,_frostCritChanceAdjust);
            UseShards(_cryoLeachCost);
        }
        GCDstart();
        CastStop();
    }

    private void FrostAegisCast()
    {
        _isCasting = true;
        this.GetComponent<Hp>().GetOverHp(SpellType.FrostAegis);
        UseShards(_frostAegisCost);
        GCDstart();
        CastStop();
    }

    private IEnumerator AvalancheCoreCast()
    {
        Spell spell;
        _isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(_avalancheCoreCastTime);
        
        if (_targetCastingTo != null)
        {
            spell = Instantiate(_avalancheCorePrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(_targetCastingTo);
            spell.AdjustCrit(_frostCritMultAdjust,_frostCritChanceAdjust);
            UseShards(_avalancheCoreCost);
        }
        CastStop();
    }

    private void EarthShieldCast()
    {
        _isCasting = true;
        this.GetComponent<Hp>().GetShieldStacks(SpellType.EarthShield);
        UseShards(_earthShieldCost);
        GCDstart();
        CastStop();
    }

    private void DeathZoneCast()
    {
        _isCasting = true;
        
        Instantiate(_deathZonePrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        UseShards(_deathZoneCost);
        GCDstart();
        CastStop();
    }
    
    private void GainRemainder(int amount)
    {
        if (_remainderAmount < MaxRemainderAmount)
        {
            _remainderAmount += amount;
            if (_remainderAmount > MaxRemainderAmount)
            {
                _remainderAmount = MaxRemainderAmount;
            }
        }
    }

    private void UseShards(Vector3Int shards)
    {
        UseFs(shards.x);
        UseFrS(shards.y);
        UseES(shards.z);
    }
    
    private void UseFs(int amount)
    {
        if (amount > 0)
        {
            if(_fsAmount > 0)
            {
                _fsAmount -= amount;
                
                if (_lastUsedShard == ShardType.None)
                {
                    _lastUsedShard = ShardType.FireShard;
                }
                else
                {
                    _previosShard = _lastUsedShard;
                    _lastUsedShard = ShardType.FireShard;
                }
                
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    for (int j = 0; j < _fireShardsRefreshRoutine.Length; j++)
                    {
                        if (_isfireShardsRefreshRoutineStarted[j] == false)
                        {
                            StartCoroutine(_fireShardsRefreshRoutine[j]);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void UseFrS(int amount)
    {
        if (amount > 0)
        {
            if (_frSAmount > 0)
            {
                _frSAmount -= amount;
                
                if (_lastUsedShard == ShardType.None)
                {
                    _lastUsedShard = ShardType.FrostShard;
                }
                else
                {
                    _previosShard = _lastUsedShard;
                    _lastUsedShard = ShardType.FrostShard;
                }
                
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    for (int j = 0; j < _frostShardsRefreshRoutine.Length; j++)
                    {
                        if (_isfrostShardsRefreshRoutineStarted[j] == false)
                        {
                            StartCoroutine(_frostShardsRefreshRoutine[j]);
                            break;
                        }
                    }
                }
            }
        }
    }
    
    private void UseES(int amount)
    {
        if (amount > 0)
        {
            if (_esAmount > 0)
            {
                _esAmount -= amount;
                
                if (_lastUsedShard == ShardType.None)
                {
                    _lastUsedShard = ShardType.EarthShard;
                }
                else
                {
                    _previosShard = _lastUsedShard;
                    _lastUsedShard = ShardType.EarthShard;
                }
                
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    for (int j = 0; j < _earthShardsRefreshRoutine.Length; j++)
                    {
                        if (_isearthShardsRefreshRoutineStarted[j] == false)
                        {
                            StartCoroutine(_earthShardsRefreshRoutine[j]);
                            break;
                        }
                    }
                }
            }
        }
    }*/

    private IEnumerator FS_Refreshing(int id)
    {
        _isfireShardsRefreshRoutineStarted[id] = true;
        //Debug.Log("Started " + ID);
        int k = 0;
        foreach (Image shard in _fireShards)
        {
            if (_fsRefreshProgress[k] >= 1f)
            {
                _fsRefreshProgress[k] = 0f;
                break;
            }
            k++;
        }
        
        yield return new WaitForSeconds(_fsRefreshTime);
        
        GetFireShard();
        _fireShardsRefreshRoutine[id] = FS_Refreshing(id);
        _isfireShardsRefreshRoutineStarted[id] = false;
        //Debug.Log("Finished " + ID);
    }

    private IEnumerator FrS_Refreshing(int id)
    {
        _isfrostShardsRefreshRoutineStarted[id] = true;
        
        int k = 0;
        foreach (Image shard in _frostShards)
        {
            if (_frSRefreshProgress[k] >= 1f)
            {
                _frSRefreshProgress[k] = 0f;
                break;
            }
            k++;
        }
        
        yield return new WaitForSeconds(_frSRefreshTime);
        
        GetFrostShard();
        _frostShardsRefreshRoutine[id] = FrS_Refreshing(id);
        _isfrostShardsRefreshRoutineStarted[id] = false;
    }
    
    private IEnumerator ES_Refreshing(int id)
    {
        _isearthShardsRefreshRoutineStarted[id] = true;
        
        int k = 0;
        foreach (Image shard in _earthShards)
        {
            if (_esRefreshProgress[k] >= 1f)
            {
                _esRefreshProgress[k] = 0f;
                break;
            }
            k++;
        }
        
        yield return new WaitForSeconds(_esRefreshTime);
        
        GetEarthShard();
        _earthShardsRefreshRoutine[id] = ES_Refreshing(id);
        _isearthShardsRefreshRoutineStarted[id] = false;
    }

    private void GetFireShard()
    {
        if(_fsAmount < MaxFsAmount)
        {
            _fsAmount++;
            //Debug.Log("fire: " + FSAmount);
            if (_fsAmount > MaxFsAmount)
            {
                _fsAmount = MaxFsAmount;
            }
        }
    }

    private void GetFrostShard()
    {
        if(_frSAmount < MaxFrSAmount)
        {
            _frSAmount++;
            //Debug.Log("frost: " + FrSAmount);
            if (_frSAmount > MaxFrSAmount)
            {
                _frSAmount = MaxFrSAmount;
            }
        }
    }

    private void GetEarthShard()
    {
        if(_esAmount < MaxESAmount)
        {
            _esAmount++;
            if (_esAmount > MaxESAmount)
            {
                _esAmount = MaxESAmount;
            }
        }
    }

    private void Blink(InputAction.CallbackContext context)
    {
        if (_movement.magnitude != 0f)
        {
            if (!_isBlinked)
            {
                float raycastOffet = 0.4f;
                float distFactor = 0.8f;
                LayerMask mask = LayerMask.GetMask("Walls");
                _blinkRch = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - raycastOffet), this._movement, _blinkDist, mask);
                //Debug.DrawLine(new Vector3(transform.position.x,transform.position.y - raycastOffet, 0),new Vector3(transform.position.x + Movement.x * blinkRCH.distance * distFactor, transform.position.y + this.Movement.y * blinkRCH.distance * distFactor, 0), Color.red, 99f);
                if (_blinkRch.collider != null)
                {
                    this._rb.position += this._movement * _blinkRch.distance * distFactor;
                }
                else
                {
                    this._rb.position += this._movement * _blinkDist;
                }
                _blinkRefreshBar.fillAmount = 0f;
                StartCoroutine("BlinkRefresh");   
            }
        }
    }

    private IEnumerator BlinkRefresh()
    {
        _isBlinked = true;
        yield return new WaitForSeconds(_blinkCd);
        _blinkRefreshProgress = 0f;
        _isBlinked = false;
    }

    private void GCDstart()
    {
        _gcDprogress = 1f;
        foreach (var bar in _gcdBarz)
        {
            bar.fillAmount = _gcDprogress;
        }
    }

    private void GCDstop()
    {
        _gcDprogress = 0f;
        foreach (var bar in _gcdBarz)
        {
            bar.fillAmount = _gcDprogress;
        }
    }

    private bool IsEnoughShards(Vector3Int cost)
    {
        return (_fsAmount >= cost.x) && (_frSAmount >= cost.y) &&
               (_esAmount >= cost.z);
    }
}