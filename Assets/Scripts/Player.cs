using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum SpellType
    {
        Fireball,
        Zap,
        frost_whirlwind,
        Spike
    }
    [Header("Spell")]
    [SerializeField] private Image CastBar = null;
    [SerializeField] private Image RemainderBar = null;
    [SerializeField] private Image[] FireShards = null;
    [Space]
    [SerializeField] private float FS_RefreshTime = 0f;
    [SerializeField] private float FireballCastTime = 0f;
    [SerializeField] private GameObject FireBallPrefab = null;
    private const int MaxRemainderAmount = 100;
    private const int MaxFSAmount = 3;
    private bool isCasting = false;
    private float CastProgress = 0f;
    private float[] FS_RefreshProgress = new float[MaxFSAmount];
    private KeyCode InterruptCastKey = KeyCode.X;
    private int RemainderAmount = 0;
    private int FSAmount = 0;

    [Header("Movement")]
    [SerializeField] private float MaxSpeed = 0;
    [SerializeField] private float SlowFactor = 0;
    private float Speed = 0;
    private Rigidbody2D rb;
    private Vector2 Movement;
    private Animator anim;

    [Header("Target system")]
    [SerializeField] private float interactionRange  = 0;
    private KeyCode TargetingKey = KeyCode.Tab;
    private KeyCode Cast1Key = KeyCode.Alpha1;
    private Enemy currentTarget = null;
    private Enemy TargetCastingTo = null;
    private List<Enemy> EnemiesInRange = new List<Enemy>();
    private int currentIndex = 0;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        FSAmount = MaxFSAmount;

        for (var frp = 0; frp < MaxFSAmount; frp++)
        {
            FS_RefreshProgress[frp] = 1f;
        }
    }

    private void Update() {
        this.Movement.x = Input.GetAxisRaw("Horizontal");
        this.Movement.y = Input.GetAxisRaw("Vertical");
        this.Movement = this.Movement.normalized;

        anim.SetFloat("MoveX", Movement.x);
        anim.SetFloat("MoveY",  Movement.y);

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
            StopCoroutine("FireballCast");
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

        if (currentTarget != null)
        {
            CheckTargetDistance();
        }

        if (isCasting)
        {
            Speed = MaxSpeed/SlowFactor;
            if (CastProgress <= 1f)
            {
                CastProgress += 1f / FireballCastTime * Time.deltaTime;
                CastBar.fillAmount = CastProgress;
            }
            else
            {
                CastBar.fillAmount = 1f;
            }
        }
        else
        {
            Speed = MaxSpeed;
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
    }

    private void FixedUpdate()
    {
        this.rb.MovePosition(this.rb.position + this.Movement * Speed * Time.fixedDeltaTime);
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
        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.transform.position);
            if (distanceToTarget > interactionRange)
            {
                ClearTarget();
                StopCoroutine("FireballCast");
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

    public void CastSpell(SpellType spellType)
    {
        switch (spellType)
        {
            case SpellType.Fireball:
                if (!isCasting)
                {
                    if (FSAmount > 0)
                    {
                        StartCoroutine("FireballCast");
                    }
                }
                break;
            case SpellType.Zap:
                //HP.TakeDamage(5);
                //enemy.Slow(2.0f);
                break;
            case SpellType.frost_whirlwind:
                //enemy.Heal(15);
                break;
            case SpellType.Spike:
                //enemy.ApplyPoison(5, 3);
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

    private void CastStop()
    {
        TargetCastingTo = null;
        isCasting = false;
        CastProgress = 0f;
        CastBar.fillAmount = CastProgress;
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
}
