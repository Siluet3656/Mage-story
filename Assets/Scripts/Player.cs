using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public enum SpellType
    {
        Fireball,
        Zap,
        frost_whirlwind,
        Spike
    }

    [SerializeField] private float Speed = 0;
    private Rigidbody2D rb;
    private Vector2 Movement;
    private Animator anim;

    private KeyCode TargetingKey = KeyCode.Tab;
    private KeyCode Cast1Key = KeyCode.Alpha1;
    [SerializeField] private float interactionRange  = 0;
    private Enemy currentTarget = null;
    private List<Enemy> EnemiesInRange = new List<Enemy>();
    private int currentIndex = 0;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

        if (currentTarget != null)
        {
            CheckTargetDistance();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        if (Input.GetKeyDown(Cast1Key))
        {
            if (currentTarget != null)
            {
                currentTarget.gameObject.GetComponent<HP>().TakeDamage(100);
            }
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
                //HP.TakeDamage(10);
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
}
