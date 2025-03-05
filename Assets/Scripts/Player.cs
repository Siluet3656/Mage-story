using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Spell")]
    [SerializeField] private Image CastBar = null;
    [SerializeField] private Image RemainderBar = null;
    [SerializeField] private Image[] FireShards = null;
    [SerializeField] private Image[] FrostShards = null;
    [SerializeField] private Image[] EarthShards = null;
    [SerializeField] private float FS_RefreshTime = 0f;
    [SerializeField] private float FrS_RefreshTime = 0f;
    [SerializeField] private float ES_RefreshTime = 0f;
    [Space]
    [SerializeField] private float FireballCastTime = 0f;
    [SerializeField] private GameObject FireBallPrefab = null;
    [Space]
    [SerializeField] private float frost_whirlwindCastTime = 0f;
    [SerializeField] private GameObject frost_whirlwindPrefab = null;
    [Space]
    [SerializeField] private float SpikeCastTime = 0f;
    [SerializeField] private GameObject SpikePrefab = null;
    [Space] 
    [SerializeField] private int ZapCost = 0;
    [SerializeField] private GameObject ZapPrefub = null;
    
    private KeyCode Cast1Key = KeyCode.Alpha1;
    private KeyCode Cast2Key = KeyCode.Alpha2;
    private KeyCode Cast3Key = KeyCode.Alpha3;
    private KeyCode Cast4Key = KeyCode.Alpha4;
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
    private int RemainderAmount = 0;
    private int FSAmount = 0;
    private int FrSAmount = 0;
    private int ESAmount = 0;

    [Header("Movement")]
    [SerializeField] private SpeedType speedType;
    private float Speed = 0;
    private Rigidbody2D rb;
    private Vector2 Movement;
    private Animator anim;

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

        for (var frp = 0; frp < MaxFSAmount; frp++)
        {
            FS_RefreshProgress[frp] = 1f;
            FrS_RefreshProgress[frp] = 1f;
            ES_RefreshProgress[frp] = 1f;
        }
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
        
        if (Input.GetKeyDown(TargetingKey))
        {
            TrySelectTarget();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        if (Input.GetKeyDown(InterruptCastKey))
        {
            StopAllCasts();
            CastStop();
        }

        if (Input.GetKeyDown(Cast1Key))
        {
            if (currentTarget != null)
            {
                TargetCastingTo = currentTarget;
                CastSpell(SpellType.Fireball);
            }
        }

        if (Input.GetKeyDown(Cast2Key))
        {
            if (currentTarget != null)
            {
                TargetCastingTo = currentTarget;
                CastSpell(SpellType.frost_whirlwind);
            }
        }
        
        if (Input.GetKeyDown(Cast3Key))
        {
            if (currentTarget != null)
            {
                TargetCastingTo = currentTarget;
                CastSpell(SpellType.Spike);
            }
        }
        
        if (Input.GetKeyDown(Cast4Key))
        {
            if (currentTarget != null)
            {
                TargetCastingTo = currentTarget;
                CastSpell(SpellType.Zap);
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

    private void ClearTarget()
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
    
    private void StopAllCasts()
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
            case SpellType.frost_whirlwind:
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
            UseFS();
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
            UseFrS();
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
            UseES();
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

    private void UseFS()
    {
        if(FSAmount > 0)
        {
            FSAmount--;
            GainRemainder(20);
            StartCoroutine("FS_Refreshing");
        }
    }

    private void UseFrS()
    {
        if(FrSAmount > 0)
        {
            FrSAmount--;
            GainRemainder(20);
            StartCoroutine("FrS_Refreshing");
        }
    }
    
    private void UseES()
    {
        if(ESAmount > 0)
        {
            ESAmount--;
            GainRemainder(20);
            StartCoroutine("ES_Refreshing");
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
}
