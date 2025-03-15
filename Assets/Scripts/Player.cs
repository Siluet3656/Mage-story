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
    [Header("Spell")]
    [SerializeField] private SpellTypeData data;
    [SerializeField] private Image CastBar = null;
    [SerializeField] private Image RemainderBar = null;
    [SerializeField] private SpellBarButton[] spellBarCells;
    [SerializeField] private Image[] FireShards = null;
    [SerializeField] private Image[] FrostShards = null;
    [SerializeField] private Image[] EarthShards = null;
    [SerializeField] private float FS_RefreshTime = 0f;
    [SerializeField] private float FrS_RefreshTime = 0f;
    [SerializeField] private float ES_RefreshTime = 0f;
    
    private GameObject FireBallPrefab;
    private GameObject ZapPrefub;
    private GameObject frost_whirlwindPrefab;
    private GameObject SpikePrefab;
    
    private float FireballCastTime;
    private float frost_whirlwindCastTime;
    private float SpikeCastTime;

    private Vector3Int fireballCost;
    private Vector3Int frost_whirlwindCost;
    private Vector3Int SpikeCost;
    private float ZapCost;
    
    private KeyCode Cast1Key = KeyCode.Alpha1;
    private KeyCode Cast2Key = KeyCode.Alpha2;
    private KeyCode Cast3Key = KeyCode.Alpha3;
    private KeyCode Cast4Key = KeyCode.Alpha4;
    private KeyCode Cast5Key = KeyCode.Alpha5;
    private KeyCode Cast6Key = KeyCode.Alpha6;
    private KeyCode Cast7Key = KeyCode.Alpha7;
    private KeyCode InterruptCastKey = KeyCode.X;
    
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
    private float RemainderAmount = 0;
    private int FSAmount = 0;
    private int FrSAmount = 0;
    private int ESAmount = 0;

    [Header("Movement")] 
    [SerializeField] private SpeedType speedType;
    [SerializeField] private Image BlinkRefreshBar;
    [SerializeField] private float BlinkDist = 0f;
    [SerializeField] private float blinkCD = 0f;
    private RaycastHit2D blinkRCH;
    private KeyCode BlinkKey = KeyCode.Space;
    private bool isBlinked = false;
    private float BlinkRefreshProgress = 0f;
    private float Speed = 0;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 Movement;

    [Header("Target system")]
    [SerializeField] private float interactionRange  = 0;
    private KeyCode TargetingKey = KeyCode.Tab;
    private Enemy currentTarget = null;
    private Enemy TargetCastingTo = null;
    private List<Enemy> EnemiesInRange = new List<Enemy>();
    private int currentIndex = 0;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        FSAmount = MaxFSAmount;
        FrSAmount = MaxFrSAmount;
        ESAmount = MaxESAmount;
        
        FireBallPrefab = data.GetDataByType(SpellType.Fireball).PrefubOfSpell;
        frost_whirlwindPrefab = data.GetDataByType(SpellType.Frost_whirlwind).PrefubOfSpell;
        SpikePrefab = data.GetDataByType(SpellType.Spike).PrefubOfSpell;
        ZapPrefub = data.GetDataByType(SpellType.Zap).PrefubOfSpell;
        
        FireballCastTime = data.GetDataByType(SpellType.Fireball).CastTime;
        frost_whirlwindCastTime = data.GetDataByType(SpellType.Frost_whirlwind).CastTime;
        SpikeCastTime = data.GetDataByType(SpellType.Spike).CastTime;
        
        fireballCost = data.GetDataByType(SpellType.Fireball).ShardsCost;
        frost_whirlwindCost = data.GetDataByType(SpellType.Frost_whirlwind).ShardsCost;
        SpikeCost = data.GetDataByType(SpellType.Spike).ShardsCost;
        ZapCost = data.GetDataByType(SpellType.Zap).ReminderCost;

        for (var frp = 0; frp < MaxFSAmount; frp++)
        {
            FS_RefreshProgress[frp] = 1f;
            FrS_RefreshProgress[frp] = 1f;
            ES_RefreshProgress[frp] = 1f;
        }

        BlinkRefreshBar.fillAmount = 1f;
    }

    private void Update() {
        this.Movement.x = Input.GetAxisRaw("Horizontal");
        this.Movement.y = Input.GetAxisRaw("Vertical");
        this.Movement = this.Movement.normalized;

        anim.SetFloat("MoveX", Movement.x);
        anim.SetFloat("MoveY",  Movement.y);
        
        if (currentTarget != null)
        {
            CheckTargetDistance();
        }
        else
        {
            if (isCasting)
            {
                CheckTargetCastingToDistance();
            }
        }

        if (Input.GetKeyDown(BlinkKey))
        {
            if (Movement.magnitude != 0f)
            {
                Blink();
            }
        }
        
        if (Input.GetKeyDown(TargetingKey))
        {
            TrySelectTarget();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        if (isCasting)
        {
            if (Input.GetKeyDown(InterruptCastKey))
            {
                StopAllCasts();
                CastStop();
            }
        }
        else
        {
            if (currentTarget != null)
            {
                TargetCastingTo = currentTarget;
                
                if (Input.GetKeyDown(Cast1Key))
                {
                    if (currentTarget != null)
                    {
                        CastSpell(spellBarCells[0].GetSpellType());
                    }
                }

                if (Input.GetKeyDown(Cast2Key))
                {
                    if (currentTarget != null)
                    {
                        CastSpell(spellBarCells[1].GetSpellType());
                    }
                }
        
                if (Input.GetKeyDown(Cast3Key))
                {
                    if (currentTarget != null)
                    {
                        CastSpell(spellBarCells[2].GetSpellType());
                    }
                }
        
                if (Input.GetKeyDown(Cast4Key))
                {
                    if (currentTarget != null)
                    {
                        CastSpell(spellBarCells[3].GetSpellType());
                    }
                }
                
                if (Input.GetKeyDown(Cast5Key))
                {
                    if (currentTarget != null)
                    {
                        CastSpell(spellBarCells[4].GetSpellType());
                    }
                }
                
                if (Input.GetKeyDown(Cast6Key))
                {
                    if (currentTarget != null)
                    {
                        CastSpell(spellBarCells[5].GetSpellType());
                    }
                }
                
                if (Input.GetKeyDown(Cast7Key))
                {
                    if (currentTarget != null)
                    {
                        CastSpell(spellBarCells[6].GetSpellType());
                    }
                }
            }
        }

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
        }
        else
        {
            Speed = SpeedTypeData.GetDataByID(speedType);
        }

        RemainderBar.fillAmount = (float)RemainderAmount/MaxRemainderAmount;

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

    private void TrySelectTarget()
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
                CastStop();
            }
    }

    private void CheckTargetCastingToDistance()
    {
        float distanceToTarget = Vector2.Distance(this.transform.position, TargetCastingTo.transform.position);
        if (distanceToTarget > interactionRange)
        {
            StopAllCasts();
            CastStop();
        }
    }

    private void HandleMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
                if (!isCasting)
                {
                    if (FSAmount > 0)
                    {
                        CurrentCastCastTime = FireballCastTime;
                        CastBar.color = new Color(1,0.2f,0.2f);
                        StartCoroutine("FireballCast");
                    }
                }
                break;
            case SpellType.Zap:
                if (!isCasting)
                {
                    if (RemainderAmount >= ZapCost)
                    {
                        ZapCast();
                    }
                }
                break;
            case SpellType.Frost_whirlwind:
                if (!isCasting)
                {
                    if (FrSAmount > 0)
                    {
                        CurrentCastCastTime = frost_whirlwindCastTime;
                        CastBar.color = new Color(0.2f,0.2f,1);
                        StartCoroutine("frost_whirlwindCast");
                    }
                }
                break;
            case SpellType.Spike:
                if (!isCasting)
                {
                    if (ESAmount > 0)
                    {
                        CurrentCastCastTime = SpikeCastTime;
                        CastBar.color = new Color(0.2f,1,0.2f);
                        StartCoroutine("SpikeCast");
                    }
                }
                break;
        }
    }

    private IEnumerator FireballCast()
    {
        Spell spell;
        isCasting = true;
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
                GainRemainder(20);
                StartCoroutine("FS_Refreshing");
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
                GainRemainder(20);
                StartCoroutine("FrS_Refreshing");
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
                GainRemainder(20);
                StartCoroutine("ES_Refreshing");
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

    private void Blink()
    {
        if (!isBlinked)
        {
            blinkRCH = Physics2D.Raycast(this.transform.position, this.Movement);
            if (blinkRCH.collider != null)
            {
                if (blinkRCH.collider.CompareTag("Wall"))
                {
                    if (blinkRCH.distance < BlinkDist)
                    {
                        this.rb.position += this.Movement * blinkRCH.distance;
                    }
                    else
                    {
                        this.rb.position += this.Movement * BlinkDist;
                    }
                }
                else
                {
                    this.rb.position += this.Movement * BlinkDist;
                }
            }
            else
            {
                this.rb.position += this.Movement * BlinkDist;
            }
            BlinkRefreshBar.fillAmount = 0f;
            StartCoroutine("BlinkRefresh");   
        }
    }

    private IEnumerator BlinkRefresh()
    {
        isBlinked = true;
        yield return new WaitForSeconds(blinkCD);
        BlinkRefreshProgress = 0f;
        isBlinked = false;
    }
}
