using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    
    [Header("Spell")]
    [SerializeField] private SpellTypeData data;
    [SerializeField] private Image CastBar = null;
    [SerializeField] private Image RemainderBar = null;
    [SerializeField] private SpellBarButton[] spellBarCells;
    [SerializeField] private Image[] FireShards = null;
    [SerializeField] private Image[] FrostShards = null;
    [SerializeField] private Image[] EarthShards = null;
    [SerializeField] private Image[] GCD_barz;
    [Space]
    [SerializeField] private float FS_RefreshTime = 0f;
    [SerializeField] private float FrS_RefreshTime = 0f;
    [SerializeField] private float ES_RefreshTime = 0f;
    [SerializeField] private float GCD;
    [Space] 
    [SerializeField] private GameObject iceTomb;
    [SerializeField] private int amountOfIcecles;

    private IEnumerator[] fireShardsRefreshRoutine = new IEnumerator[3];
    private IEnumerator[] frostShardsRefreshRoutine = new IEnumerator[3];
    private IEnumerator[] earthShardsRefreshRoutine = new IEnumerator[3];

    private bool[] isfireShardsRefreshRoutineStarted = new bool[3];
    private bool[] isfrostShardsRefreshRoutineStarted = new bool[3];
    private bool[] isearthShardsRefreshRoutineStarted = new bool[3];

    private GameObject FireBallPrefab;
    private GameObject ZapPrefub;
    private GameObject frost_whirlwindPrefab;
    private GameObject SpikePrefab;
    private GameObject BoomPrefub;
    private GameObject FirewallPrefub;
    private GameObject FirespiritPrefub;
    private GameObject FirelaserPrefub;
    private GameObject FiremarkPrefub;
    private GameObject FlashFreezePrefub;
    private GameObject IcecleBarragePrefub;
    private GameObject CryoLeachPrefub;
    private GameObject AvalancheCorePrefub;
    private GameObject DeathZonePrefub;
    //private GameObject FlowerPrefub;
    

    private float FireballCastTime;
    private float frost_whirlwindCastTime;
    private float SpikeCastTime;
    private float FirewallCastTime;
    private float FirespiritCastTime;
    private float FirelaserCastTime;
    private float FiremarkCastTime;
    private float IcecleBarrageCastTime;
    private float AvalancheCoreCastTime;

    private Vector3Int fireballCost;
    private Vector3Int frost_whirlwindCost;
    private Vector3Int SpikeCost;
    private Vector3Int BoomCost;
    private Vector3Int FirewallCost;
    private Vector3Int FirespiritCost;
    private Vector3Int FirelaserCost;
    private Vector3Int FireauraCost;
    private Vector3Int FiremarkCost;
    private Vector3Int flashFreezeCost;
    private Vector3Int stasisFreezeCost;
    private Vector3Int IcecleBarrageCost;
    private Vector3Int CryoLeachCost;
    private Vector3Int frostAegisCost;
    private Vector3Int avalancheCoreCost;
    private Vector3Int earthShieldCost;
    private Vector3Int deathZoneCost;
    private float ZapCost;
    
    
    private const int MaxRemainderAmount = 100;
    private const int MaxFSAmount = 3;
    private const int MaxFrSAmount = 3;
    private const int MaxESAmount = 3;
    
    private bool isCasting = false;
    private float CastProgress = 0f;
    private float CurrentCastCastTime = 0f;
    private float[] FS_RefreshProgress = new float[MaxFSAmount];
    private float[] FrS_RefreshProgress = new float[MaxFrSAmount];
    private float[] ES_RefreshProgress = new float[MaxFrSAmount];
    private float GCDprogress = 0f;
    private float RemainderAmount = 0;
    private int FSAmount = 0;
    private int FrSAmount = 0;
    private int ESAmount = 0;
    private ShardType lastUsedShard = ShardType.None;
    private ShardType previosShard = ShardType.None;
    
    private float fireCritMultAdjust = 1;
    private float fireCritChanceAdjust = 1;
    private float frostCritMultAdjust = 1;
    private float frostCritChanceAdjust = 1;
    private float earthCritMultAdjust = 1;
    private float earthCritChanceAdjust = 1;
    private float noelemCritMultAdjust = 1;
    private float noelemCritChanceAdjust = 1;
    
    
    private Color FireballCastBarColor = new Color(0.8f,0.1f,0.1f);
    private Color FirewallCastBarColor = new Color(1,0.3f,0.1f);
    private Color FireSpiritCastBarColor = new Color(1,0.7f,0.4f);
    private Color FirelaserCastBarColor = new Color(1,0f,0.2f);
    private Color FireMarkCastBarColor = new Color(1,0.6f,0.1f);
    private Color FrostWhirlwindCastBarColor = new Color(0.1f,0.3f,1f);
    private Color IcecleBarrageCastBarColor = new Color(0f, 0.5f, 1f);
    private Color AvalancheCoreCastBarColor = new Color(0.2f, 0.2f, 1f);
    private Color SpikeCastBarColor = new Color(0.3f,1f,0.1f);

    private Buff playerBuffs;

    [Header("Movement")] 
    [SerializeField] private SpeedType speedType;
    [SerializeField] private Image BlinkRefreshBar;
    [Space]
    [SerializeField] private float BlinkDist = 0f;
    [SerializeField] private float blinkCD = 0f;
    private RaycastHit2D blinkRCH;
    private bool isBlinked = false;
    private float BlinkRefreshProgress = 0f;
    private float Speed = 0;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 Movement;
    private bool _isAvailableToMove = true;

    [Header("Target system")]
    [SerializeField] private float interactionRange  = 0;
    private Enemy currentTarget = null;
    private Enemy TargetCastingTo = null;
    private List<Enemy> EnemiesInRange = new List<Enemy>();
    private int currentIndex = 0;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerBuffs = this.GetComponent<Buff>();
        playerInputActions.Player.Blink.started += Blink;
        playerInputActions.Player.FastTarget.started += TrySelectTarget;
        playerInputActions.Player.CastInterrupt.started += InterruptCast;
        playerInputActions.Player.Castbar1.started += SpellBarButtonCast;
        playerInputActions.Player.Castbar2.started += SpellBarButtonCast;
        playerInputActions.Player.Castbar3.started += SpellBarButtonCast;
        playerInputActions.Player.Castbar4.started += SpellBarButtonCast;
        playerInputActions.Player.Castbar5.started += SpellBarButtonCast;
        playerInputActions.Player.Castbar6.started += SpellBarButtonCast;
        playerInputActions.Player.Castbar7.started += SpellBarButtonCast;
        playerInputActions.UI.LBM.started += HandleMouseClick;

        playerBuffs.OnPlayerFreeze += FreezeMovement;
        playerBuffs.OnPlayerUnFreeze += UnfreezeMovement;
        //Application.targetFrameRate = -1;
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.UI.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
        playerInputActions.UI.Disable();
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        FSAmount = MaxFSAmount;
        FrSAmount = MaxFrSAmount;
        ESAmount = MaxESAmount;
        
        isfireShardsRefreshRoutineStarted[0] = false;
        isfireShardsRefreshRoutineStarted[1] = false;
        isfireShardsRefreshRoutineStarted[2] = false;
        
        isfrostShardsRefreshRoutineStarted[0] = false;
        isfrostShardsRefreshRoutineStarted[1] = false;
        isfrostShardsRefreshRoutineStarted[2] = false;

        isearthShardsRefreshRoutineStarted[0] = false;
        isearthShardsRefreshRoutineStarted[1] = false;
        isearthShardsRefreshRoutineStarted[2] = false;
        
        fireShardsRefreshRoutine[0] = FS_Refreshing(0);
        fireShardsRefreshRoutine[1] = FS_Refreshing(1);
        fireShardsRefreshRoutine[2] = FS_Refreshing(2);
        
        frostShardsRefreshRoutine[0] = FrS_Refreshing(0);
        frostShardsRefreshRoutine[1] = FrS_Refreshing(1);
        frostShardsRefreshRoutine[2] = FrS_Refreshing(2);
        
        earthShardsRefreshRoutine[0] = ES_Refreshing(0);
        earthShardsRefreshRoutine[1] = ES_Refreshing(1);
        earthShardsRefreshRoutine[2] = ES_Refreshing(2);
        
        for (var frp = 0; frp < MaxFSAmount; frp++)
        {
            FS_RefreshProgress[frp] = 1f;
            FrS_RefreshProgress[frp] = 1f;
            ES_RefreshProgress[frp] = 1f;
        }
        
        BlinkRefreshBar.fillAmount = 1f;

        GCDprogress = 0f;
        foreach (var bar in GCD_barz)
        {
            bar.fillAmount = GCDprogress;
        }

        lastUsedShard = ShardType.None;

        //SpellData data = this.data.GetDataByType(SpellType.Fireball);
        
        FireBallPrefab = data.GetDataByType(SpellType.Fireball).PrefubOfSpell;
        frost_whirlwindPrefab = data.GetDataByType(SpellType.Frost_whirlwind).PrefubOfSpell;
        SpikePrefab = data.GetDataByType(SpellType.Spike).PrefubOfSpell;
        ZapPrefub = data.GetDataByType(SpellType.Zap).PrefubOfSpell;
        BoomPrefub = data.GetDataByType(SpellType.Boom).PrefubOfSpell;
        FirewallPrefub = data.GetDataByType(SpellType.Firewall).PrefubOfSpell;
        FirespiritPrefub = data.GetDataByType(SpellType.Firespirit).PrefubOfSpell;
        FirelaserPrefub = data.GetDataByType(SpellType.Firelaser).PrefubOfSpell;
        FiremarkPrefub = data.GetDataByType(SpellType.Firemark).PrefubOfSpell;
        FlashFreezePrefub = data.GetDataByType(SpellType.FlashFreeze).PrefubOfSpell;
        IcecleBarragePrefub = data.GetDataByType(SpellType.IcicleBarrage).PrefubOfSpell;
        CryoLeachPrefub = data.GetDataByType(SpellType.CryoLeach).PrefubOfSpell;
        AvalancheCorePrefub = data.GetDataByType(SpellType.AvalancheCore).PrefubOfSpell;
        DeathZonePrefub = data.GetDataByType(SpellType.DeathZone).PrefubOfSpell;
        
        FireballCastTime = data.GetDataByType(SpellType.Fireball).CastTime;
        frost_whirlwindCastTime = data.GetDataByType(SpellType.Frost_whirlwind).CastTime;
        SpikeCastTime = data.GetDataByType(SpellType.Spike).CastTime;
        FirewallCastTime = data.GetDataByType(SpellType.Firewall).CastTime;
        FirespiritCastTime = data.GetDataByType(SpellType.Firespirit).CastTime;
        FirelaserCastTime = data.GetDataByType(SpellType.Firelaser).CastTime;
        FiremarkCastTime = data.GetDataByType(SpellType.Firemark).CastTime;
        IcecleBarrageCastTime = data.GetDataByType(SpellType.IcicleBarrage).CastTime;
        AvalancheCoreCastTime = data.GetDataByType(SpellType.AvalancheCore).CastTime;
        
        fireballCost = data.GetDataByType(SpellType.Fireball).ShardsCost;
        frost_whirlwindCost = data.GetDataByType(SpellType.Frost_whirlwind).ShardsCost;
        SpikeCost = data.GetDataByType(SpellType.Spike).ShardsCost;
        ZapCost = data.GetDataByType(SpellType.Zap).ReminderCost;
        BoomCost = data.GetDataByType(SpellType.Boom).ShardsCost;
        FirewallCost = data.GetDataByType(SpellType.Firewall).ShardsCost;
        FirespiritCost = data.GetDataByType(SpellType.Firespirit).ShardsCost;
        FirelaserCost = data.GetDataByType(SpellType.Firelaser).ShardsCost;
        FireauraCost = data.GetDataByType(SpellType.Fireaura).ShardsCost;
        FiremarkCost = data.GetDataByType(SpellType.Firemark).ShardsCost;
        flashFreezeCost = data.GetDataByType(SpellType.FlashFreeze).ShardsCost;
        stasisFreezeCost = data.GetDataByType(SpellType.StasisFreeze).ShardsCost;
        IcecleBarrageCost = data.GetDataByType(SpellType.IcicleBarrage).ShardsCost;
        CryoLeachCost = data.GetDataByType(SpellType.CryoLeach).ShardsCost;
        frostAegisCost = data.GetDataByType(SpellType.FrostAegis).ShardsCost;
        avalancheCoreCost = data.GetDataByType(SpellType.AvalancheCore).ShardsCost;
        earthShieldCost = data.GetDataByType(SpellType.EarthShield).ShardsCost;
        deathZoneCost = data.GetDataByType(SpellType.DeathZone).ShardsCost;
    }

    private void Update()
    {
        if (_isAvailableToMove)
        {
            Movement = playerInputActions.Player.Movement.ReadValue<Vector2>();
        }
        else
        {
            Movement = Vector2.zero;
        }
        
        anim.SetFloat("MoveX", Movement.x);
        anim.SetFloat("MoveY",  Movement.y);
        
        RemainderBar.fillAmount = RemainderAmount/MaxRemainderAmount;

        if (isCasting)
        {
            Speed = SpeedTypeData.GetDataByType(speedType - 1);
            if (CastProgress <= 1f)
            {
                CastProgress += 1f / CurrentCastCastTime * Time.deltaTime;
                CastBar.fillAmount = CastProgress;
            }
            else
            {
                CastBar.fillAmount = 1f;
            }
        }
        else
        {
            Speed = SpeedTypeData.GetDataByType(speedType);
        }

        int j = 0;
        foreach (Image shard in FireShards)
        {
            if (FS_RefreshProgress[j] < 1f)
            {
                FS_RefreshProgress[j] += 1f / FS_RefreshTime * Time.deltaTime;
                shard.fillAmount = FS_RefreshProgress[j];
            }
            j++;
        }
        j = 0;
        foreach (Image shard in FrostShards)
        {
            if (FrS_RefreshProgress[j] < 1f)
            {
                FrS_RefreshProgress[j] += 1f / FrS_RefreshTime * Time.deltaTime;
                shard.fillAmount = FrS_RefreshProgress[j];
            }
            j++;
        }
        j = 0;
        foreach (Image shard in EarthShards)
        {
            if (ES_RefreshProgress[j] < 1f)
            {
                ES_RefreshProgress[j] += 1f / ES_RefreshTime * Time.deltaTime;
                shard.fillAmount = ES_RefreshProgress[j];
            }
            j++;
        }
        
        if (GCDprogress >= 0)
        {
            GCDprogress -= 1f / GCD * Time.deltaTime;
        }
        else
        {
            GCDprogress = 0;
        }
        foreach (Image bar in GCD_barz)
        {
            bar.fillAmount = GCDprogress;
        }
        
        if (currentTarget != null)
        {
            CheckTargetDistance();
        }
        else
        {
            CheckTargetCastingToDistance();
        }
        
        if (isBlinked)
        {
            if (BlinkRefreshProgress <= 1f)
            {
                BlinkRefreshProgress += 1f / blinkCD * Time.deltaTime;
                BlinkRefreshBar.fillAmount = BlinkRefreshProgress;
            }
            else
            {
                BlinkRefreshBar.fillAmount = 1f;
            }
        }
    }

    private void FixedUpdate()
    {
        this.rb.MovePosition(this.rb.position + this.Movement * (Speed * Time.fixedDeltaTime));
    }

    private void TrySelectTarget(InputAction.CallbackContext context)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        EnemiesInRange.Clear();
        foreach (Enemy enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy <= interactionRange)
            {
                EnemiesInRange.Add(enemy);
            }
        }

        if (EnemiesInRange.Count > 0)
        {
            EnemiesInRange = EnemiesInRange.OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position)).ToList();

            if (currentTarget != null)
            {
                currentIndex = EnemiesInRange.IndexOf(currentTarget);
                ClearTarget();
                
                if (currentIndex == -1 || currentIndex >= EnemiesInRange.Count - 1)
                {
                    currentTarget = EnemiesInRange[0];
                    EnemiesInRange[0].Target();
                }
                else
                {
                    currentTarget = EnemiesInRange[currentIndex + 1];
                    EnemiesInRange[currentIndex + 1].Target();
                }
            }
            else
            {
                currentTarget = EnemiesInRange[0];
                EnemiesInRange[0].Target();
            }
        }
    }

    private void CheckTargetDistance()
    {
        float distanceToTarget = Vector2.Distance(this.transform.position, currentTarget.transform.position);
            if (distanceToTarget > interactionRange)
            {
                ClearTarget();
                StopAllCasts();
            }
    }

    private void CheckTargetCastingToDistance()
    {
        if (isCasting)
        {
            if (TargetCastingTo != null)
            {
                float distanceToTarget = Vector2.Distance(this.transform.position, TargetCastingTo.transform.position);
                if (distanceToTarget > interactionRange)
                {
                    StopAllCasts();
                }
            }
        }
    }

    private void HandleMouseClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(playerInputActions.UI.MousePosition.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            ClearTarget();
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Target();
                currentTarget = enemy;
            }
        }
        else
        {
            ClearTarget();
        }
    }

    public void ClearTarget()
    {
        if (currentTarget != null)
        { 
            currentTarget.ResetTarget();
            currentTarget = null;
        }
    }
    
    private void CastStop()
    {
        TargetCastingTo = null;
        isCasting = false;
        CastProgress = 0f;
        CastBar.fillAmount = CastProgress;
    }

    private void InterruptCast(InputAction.CallbackContext context)
    {
        if (isCasting)
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
                shardsToInvoke.Add(lastUsedShard);
                break;
            case 2:
                shardsToInvoke.Add(lastUsedShard);
                shardsToInvoke.Add(previosShard);
                break;
        }

        foreach (var shard in shardsToInvoke)
        {
            switch (shard)
            {
                case ShardType.FireShard:
                    for (int i = 0; i < fireShardsRefreshRoutine.Length; i++)
                    {
                        if (isfireShardsRefreshRoutineStarted[i])
                        {
                            FireShards[i].fillAmount = 1f;
                            FS_RefreshProgress[i] = 1f;

                            StopCoroutine(fireShardsRefreshRoutine[i]);

                            isfireShardsRefreshRoutineStarted[i] = false;
                            fireShardsRefreshRoutine[i] = FS_Refreshing(i);
                            GetFireShard();
                            break;
                        }
                    }
                    break;
                
                case ShardType.FrostShard:
                    for (int i = 0; i < frostShardsRefreshRoutine.Length; i++)
                    {
                        if (isfrostShardsRefreshRoutineStarted[i])
                        {
                            FrostShards[i].fillAmount = 1f;
                            FrS_RefreshProgress[i] = 1f;

                            StopCoroutine(frostShardsRefreshRoutine[i]);

                            isfrostShardsRefreshRoutineStarted[i] = false;
                            frostShardsRefreshRoutine[i] = FrS_Refreshing(i);
                            GetFrostShard();
                            break;
                        }
                    }
                    break;
                
                case ShardType.EarthShard:
                    for (int i = 0; i < earthShardsRefreshRoutine.Length; i++)
                    {
                        if (isearthShardsRefreshRoutineStarted[i])
                        {
                            EarthShards[i].fillAmount = 1f;
                            ES_RefreshProgress[i] = 1f;

                            StopCoroutine(earthShardsRefreshRoutine[i]);

                            isearthShardsRefreshRoutineStarted[i] = false;
                            earthShardsRefreshRoutine[i] = ES_Refreshing(i);
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
        fireCritMultAdjust = mult;
        fireCritChanceAdjust = chance;
    }
    
    public void SetCritAdjustFrost(float mult, float chance)
    {
        frostCritMultAdjust = mult;
        frostCritChanceAdjust = chance;
    }
    
    public void SetCritAdjustEarth(float mult, float chance)
    {
        earthCritMultAdjust = mult;
        earthCritChanceAdjust = chance;
    }
    
    public void SetCritAdjustNoelem(float mult, float chance)
    {
        noelemCritMultAdjust = mult;
        noelemCritChanceAdjust = chance;
    }

    public void FreezeMovement()
    {
        _isAvailableToMove = false;
        iceTomb.SetActive(true);
    }
    
    public void UnfreezeMovement()
    {
        _isAvailableToMove = true;
        iceTomb.SetActive(false);
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

    public void CastSpell(SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.Fireball:
                if (isEnoughShards(fireballCost))
                {
                    if (currentTarget != null)
                    {
                        CurrentCastCastTime = FireballCastTime;
                        CastBar.color = FireballCastBarColor;
                        StartCoroutine("FireballCast");
                    }
                }
                break;
            case SpellType.Frost_whirlwind:
                if (isEnoughShards(frost_whirlwindCost))
                {
                    if (currentTarget != null)
                    {
                        CurrentCastCastTime = frost_whirlwindCastTime;
                        CastBar.color = FrostWhirlwindCastBarColor;
                        StartCoroutine("frost_whirlwindCast");
                    }
                }
                break;
            case SpellType.Spike:
                if (isEnoughShards(SpikeCost)) 
                {
                    if (currentTarget != null)
                    {
                        CurrentCastCastTime = SpikeCastTime;
                        CastBar.color = SpikeCastBarColor;
                        StartCoroutine("SpikeCast");
                    }
                }
                break;
            case SpellType.Zap:
                if (RemainderAmount >= ZapCost) 
                {
                    if (currentTarget != null)
                    {
                        ZapCast();
                    }
                }
                break;
            case SpellType.Boom:
                if (isEnoughShards(BoomCost))
                {
                    BoomCast();
                }
                break;
            case SpellType.Firewall:
                if (isEnoughShards(FirewallCost))
                {
                    CurrentCastCastTime = FirewallCastTime;
                    CastBar.color = FirewallCastBarColor;
                    StartCoroutine("FirewallCast");
                }
                break;
            case SpellType.Firespirit:
                if (isEnoughShards(FirespiritCost))
                {
                    CurrentCastCastTime = FirespiritCastTime;
                    CastBar.color = FireSpiritCastBarColor;
                    StartCoroutine("FireSpiritCast");
                }
                break; 
            case SpellType.Firelaser:
                if (isEnoughShards(FirelaserCost))
                {
                    if (currentTarget != null)
                    {
                        CurrentCastCastTime = FirelaserCastTime;
                        CastBar.color = FirelaserCastBarColor;
                        StartCoroutine("FirelaserCast");
                    }
                }
                break;
            case SpellType.Fireaura:
                if (isEnoughShards(FireauraCost))
                {
                    FireAuraCast();
                }
                break;
            case SpellType.Firemark:
                if (isEnoughShards(FiremarkCost))
                {
                    if (currentTarget != null)
                    {
                        CurrentCastCastTime = FiremarkCastTime;
                        CastBar.color = FireMarkCastBarColor;
                        StartCoroutine("FireMarkCast");
                    }
                }
                break;
            case SpellType.FlashFreeze:
                if (isEnoughShards(flashFreezeCost))
                {
                    FlashFreezeCast();
                }
                break;
            case SpellType.StasisFreeze:
                if (isEnoughShards(stasisFreezeCost))
                {
                    StasisFreezeCast();
                }
                break;
            case SpellType.IcicleBarrage:
                if (isEnoughShards(IcecleBarrageCost)) 
                {
                    if (currentTarget != null)
                    {
                        CurrentCastCastTime = IcecleBarrageCastTime;
                        CastBar.color = IcecleBarrageCastBarColor;
                        StartCoroutine("IcecleBarrageCast");
                    }
                }
                break;
            case SpellType.CryoLeach:
                if (isEnoughShards(CryoLeachCost)) 
                {
                    if (currentTarget != null)
                    {
                        CryoLeachCast();
                    }
                }
                break;
            case SpellType.FrostAegis:
                if (isEnoughShards(frostAegisCost))
                {
                    frostAegisCast();
                }
                break;
            case SpellType.AvalancheCore:
                if (isEnoughShards(avalancheCoreCost))
                {
                    if (currentTarget != null)
                    {
                        CurrentCastCastTime = AvalancheCoreCastTime;
                        CastBar.color = AvalancheCoreCastBarColor;
                        StartCoroutine("AvalancheCoreCast");
                    }
                }
                break;
            case SpellType.EarthShield:
                if (isEnoughShards(earthShieldCost))
                {
                    earthShieldCast();
                }
                break;
            case SpellType.DeathZone:
                if (isEnoughShards(deathZoneCost))
                {
                    deathZoneCast();
                }
                break;
        }
    }

    private void SpellBarButtonCast(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!isCasting)
            {
                if (GCDprogress <= 0)
                {
                    if (_isAvailableToMove)
                    {
                        TargetCastingTo = currentTarget;

                        switch (context.action.name)
                        {
                            case "Castbar1":
                                CastSpell(spellBarCells[0].GetSpellType());
                                break;
                            case "Castbar2":
                                CastSpell(spellBarCells[1].GetSpellType());
                                break;
                            case "Castbar3":
                                CastSpell(spellBarCells[2].GetSpellType());
                                break;
                            case "Castbar4":
                                CastSpell(spellBarCells[3].GetSpellType());
                                break;
                            case "Castbar5":
                                CastSpell(spellBarCells[4].GetSpellType());
                                break;
                            case "Castbar6":
                                CastSpell(spellBarCells[5].GetSpellType());
                                break;
                            case "Castbar7":
                                CastSpell(spellBarCells[6].GetSpellType());
                                break;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator FireballCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        yield return new WaitForSeconds(FireballCastTime);
        if (TargetCastingTo != null)
        {
            spell = Instantiate(FireBallPrefab, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(TargetCastingTo);
            spell.AdjustCrit(fireCritMultAdjust,fireCritChanceAdjust);
            UseShards(fireballCost);
        }
        CastStop();
    }

    private IEnumerator frost_whirlwindCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        yield return new WaitForSeconds(frost_whirlwindCastTime);
        if (TargetCastingTo != null)
        {
            spell = Instantiate(frost_whirlwindPrefab, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(TargetCastingTo);
            spell.AdjustCrit(frostCritMultAdjust,frostCritChanceAdjust);
            UseShards(frost_whirlwindCost);
        }
        CastStop();
    }
    
    private IEnumerator SpikeCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        yield return new WaitForSeconds(SpikeCastTime);
        if (TargetCastingTo != null)
        {
            spell = Instantiate(SpikePrefab, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(TargetCastingTo);
            spell.AdjustCrit(earthCritMultAdjust,earthCritChanceAdjust);
            UseShards(SpikeCost);
        }
        CastStop();
    }

    private void ZapCast()
    {
        Spell spell;
        isCasting = true;
        if (TargetCastingTo != null)
        {
            spell = Instantiate(ZapPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.GetComponent<LineRenderer>().SetPosition(0,this.transform.position);
            spell.GetComponent<LineRenderer>().SetPosition(1,TargetCastingTo.transform.position);
            spell.SetTarget(TargetCastingTo);
            spell.AdjustCrit(noelemCritMultAdjust,noelemCritChanceAdjust);
            RemainderAmount -= ZapCost;
        }
        GCDstart();
        CastStop();
    }

    private void BoomCast()
    {
        Spell spell;
        isCasting = true;
        spell = Instantiate(BoomPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        spell.AdjustCrit(fireCritMultAdjust,fireCritChanceAdjust);
        UseShards(BoomCost);
        GCDstart();
        CastStop();
    }

    private void FlashFreezeCast()
    {
        Spell spell;
        isCasting = true;
        spell = Instantiate(FlashFreezePrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        spell.AdjustCrit(frostCritMultAdjust,frostCritChanceAdjust);
        UseShards(flashFreezeCost);
        GCDstart();
        CastStop();
    }

    private void StasisFreezeCast()
    {
        isCasting = true;
        this.GetComponent<Buff>().GetBuff(BuffType.StasisFreeze, this);
        UseShards(stasisFreezeCost);
        GCDstart();
        CastStop();
    }
    
    private IEnumerator FirewallCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(FirewallCastTime);
        
        spell = Instantiate(FirewallPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        spell.AdjustCrit(fireCritMultAdjust,fireCritChanceAdjust);
        UseShards(FirewallCost);
        CastStop();
    }
    
    private IEnumerator FireSpiritCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(FirespiritCastTime);
        
        spell = Instantiate(FirespiritPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        spell.AdjustCrit(fireCritMultAdjust,fireCritChanceAdjust);
        UseShards(FirespiritCost);
        CastStop();
    }

    private IEnumerator FirelaserCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(FirelaserCastTime);
        
        if (TargetCastingTo != null)
        {
            spell = Instantiate(FirelaserPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            spell.GetComponent<LineRenderer>().SetPosition(1, TargetCastingTo.transform.position);
            spell.SetTarget(TargetCastingTo);
            spell.AdjustCrit(fireCritMultAdjust,fireCritChanceAdjust);
            UseShards(FirelaserCost);
        }
        CastStop();
    }

    private void FireAuraCast()
    {
        isCasting = true;
        this.GetComponent<Buff>().GetBuff(BuffType.FireAura, this);
        UseShards(FireauraCost);
        GCDstart();
        CastStop();
    }

    private IEnumerator FireMarkCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(FiremarkCastTime);
        
        if (TargetCastingTo != null)
        {
            spell = Instantiate(FiremarkPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(TargetCastingTo);
            spell.AdjustCrit(fireCritMultAdjust,fireCritChanceAdjust);
            UseShards(FiremarkCost);
        }
        CastStop();
    }

    private IEnumerator IcecleBarrageCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        yield return new WaitForSeconds(IcecleBarrageCastTime);
        if (TargetCastingTo != null)
        {
            UseShards(IcecleBarrageCost);
            for (int i = 0; i < amountOfIcecles; i++)
            {
                Vector2 intstpos = transform.position;
                intstpos.x += Random.Range(-0.3f, 0.3f);
                intstpos.y += Random.Range(-0.3f, 0.3f);
                spell = Instantiate(IcecleBarragePrefub, intstpos, Quaternion.identity).GetComponent<Spell>();
                spell.SetTarget(TargetCastingTo);
                spell.AdjustCrit(frostCritMultAdjust,frostCritChanceAdjust);
                yield return new WaitForSeconds(0.05f);
            }
        }
        CastStop();
    }

    private void CryoLeachCast()
    {
        Spell spell;
        isCasting = true;
        if (TargetCastingTo != null)
        {
            spell = Instantiate(CryoLeachPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.GetComponent<LineRenderer>().SetPosition(0,this.transform.position);
            spell.GetComponent<LineRenderer>().SetPosition(1,TargetCastingTo.transform.position);
            spell.SetTarget(TargetCastingTo);
            spell.AdjustCrit(frostCritMultAdjust,frostCritChanceAdjust);
            UseShards(CryoLeachCost);
        }
        GCDstart();
        CastStop();
    }

    private void frostAegisCast()
    {
        isCasting = true;
        this.GetComponent<HP>().GetOverHP(SpellType.FrostAegis);
        UseShards(frostAegisCost);
        GCDstart();
        CastStop();
    }

    private IEnumerator AvalancheCoreCast()
    {
        Spell spell;
        isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(AvalancheCoreCastTime);
        
        if (TargetCastingTo != null)
        {
            spell = Instantiate(AvalancheCorePrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
            spell.SetTarget(TargetCastingTo);
            spell.AdjustCrit(frostCritMultAdjust,frostCritChanceAdjust);
            UseShards(avalancheCoreCost);
        }
        CastStop();
    }

    private void earthShieldCast()
    {
        isCasting = true;
        this.GetComponent<HP>().GetShieldStacks(SpellType.EarthShield);
        UseShards(earthShieldCost);
        GCDstart();
        CastStop();
    }

    private void deathZoneCast()
    {
        isCasting = true;
        
        Instantiate(DeathZonePrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        UseShards(deathZoneCost);
        GCDstart();
        CastStop();
    }
    
    private void GainRemainder(int amount)
    {
        if (RemainderAmount < MaxRemainderAmount)
        {
            RemainderAmount += amount;
            if (RemainderAmount > MaxRemainderAmount)
            {
                RemainderAmount = MaxRemainderAmount;
            }
        }
    }

    private void UseShards(Vector3Int shards)
    {
        UseFS(shards.x);
        UseFrS(shards.y);
        UseES(shards.z);
    }
    
    private void UseFS(int amount)
    {
        if (amount > 0)
        {
            if(FSAmount > 0)
            {
                FSAmount -= amount;
                
                if (lastUsedShard == ShardType.None)
                {
                    lastUsedShard = ShardType.FireShard;
                }
                else
                {
                    previosShard = lastUsedShard;
                    lastUsedShard = ShardType.FireShard;
                }
                
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    for (int j = 0; j < fireShardsRefreshRoutine.Length; j++)
                    {
                        if (isfireShardsRefreshRoutineStarted[j] == false)
                        {
                            StartCoroutine(fireShardsRefreshRoutine[j]);
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
            if (FrSAmount > 0)
            {
                FrSAmount -= amount;
                
                if (lastUsedShard == ShardType.None)
                {
                    lastUsedShard = ShardType.FrostShard;
                }
                else
                {
                    previosShard = lastUsedShard;
                    lastUsedShard = ShardType.FrostShard;
                }
                
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    for (int j = 0; j < frostShardsRefreshRoutine.Length; j++)
                    {
                        if (isfrostShardsRefreshRoutineStarted[j] == false)
                        {
                            StartCoroutine(frostShardsRefreshRoutine[j]);
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
            if (ESAmount > 0)
            {
                ESAmount -= amount;
                
                if (lastUsedShard == ShardType.None)
                {
                    lastUsedShard = ShardType.EarthShard;
                }
                else
                {
                    previosShard = lastUsedShard;
                    lastUsedShard = ShardType.EarthShard;
                }
                
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    for (int j = 0; j < earthShardsRefreshRoutine.Length; j++)
                    {
                        if (isearthShardsRefreshRoutineStarted[j] == false)
                        {
                            StartCoroutine(earthShardsRefreshRoutine[j]);
                            break;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator FS_Refreshing(int ID)
    {
        isfireShardsRefreshRoutineStarted[ID] = true;
        //Debug.Log("Started " + ID);
        int k = 0;
        foreach (Image shard in FireShards)
        {
            if (FS_RefreshProgress[k] >= 1f)
            {
                FS_RefreshProgress[k] = 0f;
                break;
            }
            k++;
        }
        
        yield return new WaitForSeconds(FS_RefreshTime);
        
        GetFireShard();
        fireShardsRefreshRoutine[ID] = FS_Refreshing(ID);
        isfireShardsRefreshRoutineStarted[ID] = false;
        //Debug.Log("Finished " + ID);
    }

    private IEnumerator FrS_Refreshing(int ID)
    {
        isfrostShardsRefreshRoutineStarted[ID] = true;
        
        int k = 0;
        foreach (Image shard in FrostShards)
        {
            if (FrS_RefreshProgress[k] >= 1f)
            {
                FrS_RefreshProgress[k] = 0f;
                break;
            }
            k++;
        }
        
        yield return new WaitForSeconds(FrS_RefreshTime);
        
        GetFrostShard();
        frostShardsRefreshRoutine[ID] = FrS_Refreshing(ID);
        isfrostShardsRefreshRoutineStarted[ID] = false;
    }
    
    private IEnumerator ES_Refreshing(int ID)
    {
        isearthShardsRefreshRoutineStarted[ID] = true;
        
        int k = 0;
        foreach (Image shard in EarthShards)
        {
            if (ES_RefreshProgress[k] >= 1f)
            {
                ES_RefreshProgress[k] = 0f;
                break;
            }
            k++;
        }
        
        yield return new WaitForSeconds(ES_RefreshTime);
        
        GetEarthShard();
        earthShardsRefreshRoutine[ID] = ES_Refreshing(ID);
        isearthShardsRefreshRoutineStarted[ID] = false;
    }

    private void GetFireShard()
    {
        if(FSAmount < MaxFSAmount)
        {
            FSAmount++;
            //Debug.Log("fire: " + FSAmount);
            if (FSAmount > MaxFSAmount)
            {
                FSAmount = MaxFSAmount;
            }
        }
    }

    private void GetFrostShard()
    {
        if(FrSAmount < MaxFrSAmount)
        {
            FrSAmount++;
            //Debug.Log("frost: " + FrSAmount);
            if (FrSAmount > MaxFrSAmount)
            {
                FrSAmount = MaxFrSAmount;
            }
        }
    }

    private void GetEarthShard()
    {
        if(ESAmount < MaxESAmount)
        {
            ESAmount++;
            if (ESAmount > MaxESAmount)
            {
                ESAmount = MaxESAmount;
            }
        }
    }

    private void Blink(InputAction.CallbackContext context)
    {
        if (Movement.magnitude != 0f)
        {
            if (!isBlinked)
            {
                float raycastOffet = 0.4f;
                float distFactor = 0.8f;
                LayerMask mask = LayerMask.GetMask("Walls");
                blinkRCH = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y - raycastOffet), this.Movement, BlinkDist, mask);
                //Debug.DrawLine(new Vector3(transform.position.x,transform.position.y - raycastOffet, 0),new Vector3(transform.position.x + Movement.x * blinkRCH.distance * distFactor, transform.position.y + this.Movement.y * blinkRCH.distance * distFactor, 0), Color.red, 99f);
                if (blinkRCH.collider != null)
                {
                    this.rb.position += this.Movement * blinkRCH.distance * distFactor;
                }
                else
                {
                    this.rb.position += this.Movement * BlinkDist;
                }
                BlinkRefreshBar.fillAmount = 0f;
                StartCoroutine("BlinkRefresh");   
            }
        }
    }

    private IEnumerator BlinkRefresh()
    {
        isBlinked = true;
        yield return new WaitForSeconds(blinkCD);
        BlinkRefreshProgress = 0f;
        isBlinked = false;
    }

    private void GCDstart()
    {
        GCDprogress = 1f;
        foreach (var bar in GCD_barz)
        {
            bar.fillAmount = GCDprogress;
        }
    }

    private void GCDstop()
    {
        GCDprogress = 0f;
        foreach (var bar in GCD_barz)
        {
            bar.fillAmount = GCDprogress;
        }
    }

    private bool isEnoughShards(Vector3Int cost)
    {
        return (FSAmount >= cost.x) && (FrSAmount >= cost.y) &&
               (ESAmount >= cost.z);
    }
}
