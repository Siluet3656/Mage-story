using System;
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

    private GameObject FireBallPrefab;
    private GameObject ZapPrefub;
    private GameObject frost_whirlwindPrefab;
    private GameObject SpikePrefab;
    private GameObject BoomPrefub;
    private GameObject FirewallPrefub;

    private float FireballCastTime;
    private float frost_whirlwindCastTime;
    private float SpikeCastTime;
    private float FirewallCastTime;

    private Vector3Int fireballCost;
    private Vector3Int frost_whirlwindCost;
    private Vector3Int SpikeCost;
    private Vector3Int BoomCost;
    private Vector3Int FirewallCost;
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
    
    private Color FireballCastBarColor = new Color(0.9f,0.1f,0.1f);
    private Color FirewallCastBarColor = new Color(1f,0.3f,0.1f);
    private Color FrostWhirlwindCastBarColor = new Color(0.1f,0.3f,1f);
    private Color SpikeCastBarColor = new Color(0.3f,1f,0.1f);

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

    [Header("Target system")]
    [SerializeField] private float interactionRange  = 0;
    private Enemy currentTarget = null;
    private Enemy TargetCastingTo = null;
    private List<Enemy> EnemiesInRange = new List<Enemy>();
    private int currentIndex = 0;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
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

        FireBallPrefab = data.GetDataByType(SpellType.Fireball).PrefubOfSpell;
        frost_whirlwindPrefab = data.GetDataByType(SpellType.Frost_whirlwind).PrefubOfSpell;
        SpikePrefab = data.GetDataByType(SpellType.Spike).PrefubOfSpell;
        ZapPrefub = data.GetDataByType(SpellType.Zap).PrefubOfSpell;
        BoomPrefub = data.GetDataByType(SpellType.Boom).PrefubOfSpell;
        FirewallPrefub = data.GetDataByType(SpellType.Firewall).PrefubOfSpell;
        
        FireballCastTime = data.GetDataByType(SpellType.Fireball).CastTime;
        frost_whirlwindCastTime = data.GetDataByType(SpellType.Frost_whirlwind).CastTime;
        SpikeCastTime = data.GetDataByType(SpellType.Spike).CastTime;
        FirewallCastTime = data.GetDataByType(SpellType.Firewall).CastTime;
        
        fireballCost = data.GetDataByType(SpellType.Fireball).ShardsCost;
        frost_whirlwindCost = data.GetDataByType(SpellType.Frost_whirlwind).ShardsCost;
        SpikeCost = data.GetDataByType(SpellType.Spike).ShardsCost;
        ZapCost = data.GetDataByType(SpellType.Zap).ReminderCost;
        BoomCost = data.GetDataByType(SpellType.Boom).ShardsCost;
        FirewallCost = data.GetDataByType(SpellType.Firewall).ShardsCost;
    }

    private void Update()
    {
        Movement = playerInputActions.Player.Movement.ReadValue<Vector2>();

        anim.SetFloat("MoveX", Movement.x);
        anim.SetFloat("MoveY",  Movement.y);
        
        RemainderBar.fillAmount = RemainderAmount/MaxRemainderAmount;

        if (isCasting)
        {
            Speed = SpeedTypeData.GetDataByID(speedType - 1);
            if (CastProgress <= 1f)
            {
                CastProgress += 1f / CurrentCastCastTime * Time.deltaTime;
                CastBar.fillAmount = CastProgress;
            }
            else
            {
                CastBar.fillAmount = 1f;
            }
            Debug.Log(CastProgress);
        }
        else
        {
            Speed = SpeedTypeData.GetDataByID(speedType);
        }
        

        int j = 0;
        foreach (Image shard in FireShards)
        {
            if (FS_RefreshProgress[j] <= 1f)
            {
                FS_RefreshProgress[j] += 1f / FS_RefreshTime * Time.deltaTime;
                shard.fillAmount = FS_RefreshProgress[j];
            }
            j++;
        }
        j = 0;
        foreach (Image shard in FrostShards)
        {
            if (FrS_RefreshProgress[j] <= 1f)
            {
                FrS_RefreshProgress[j] += 1f / FrS_RefreshTime * Time.deltaTime;
                shard.fillAmount = FrS_RefreshProgress[j];
            }
            j++;
        }
        j = 0;
        foreach (Image shard in EarthShards)
        {
            if (ES_RefreshProgress[j] <= 1f)
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
    
    
    public void StopAllCasts()
    {
        StopCoroutine("FireballCast");
        StopCoroutine("frost_whirlwindCast");
        StopCoroutine("SpikeCast");
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
            RemainderAmount -= ZapCost;
        }
        GCDstart();
        CastStop();
    }

    private void BoomCast()
    {
        isCasting = true;
        Instantiate(BoomPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        UseShards(BoomCost);
        GCDstart();
        CastStop();
    }
    
    private IEnumerator FirewallCast()
    {
        isCasting = true;
        GCDstart();
        
        yield return new WaitForSeconds(FirewallCastTime);
        
        Instantiate(FirewallPrefub, transform.position, Quaternion.identity).GetComponent<Spell>();
        UseShards(FirewallCost);
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
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    StartCoroutine("FS_Refreshing");
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
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    StartCoroutine("FrS_Refreshing");
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
                GainRemainder(20 * amount);
                for (int i = 0; i < amount; i++)
                {
                    StartCoroutine("ES_Refreshing");
                }
            }
        }
    }

    private IEnumerator FS_Refreshing()
    {
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
        if(FSAmount < MaxFSAmount)
        {
            FSAmount++;
            if (FSAmount > MaxFSAmount)
            {
                FSAmount = MaxFSAmount;
            }
        }
    }

    private IEnumerator FrS_Refreshing()
    {
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
        if(FrSAmount < MaxFrSAmount)
        {
            FrSAmount++;
            if (FrSAmount > MaxFrSAmount)
            {
                FrSAmount = MaxFrSAmount;
            }
        }        
    }
    
    private IEnumerator ES_Refreshing()
    {
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
