using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private Transform[] PatrolPoints;
    [SerializeField]private SpriteRenderer TargetedSR = null;
    [SerializeField]private SpeedType speedType;
    [SerializeField]private float MinWaitingTime = 0;
    [SerializeField]private float MaxWaitingTime = 10;
    private Rigidbody2D rb;
    private Color OriginalColor;
    private Color TargetedColor;
    private bool isTargeted = false;
    private bool isPatrolling = false;
    private float Speed = 0;
    private Vector2 Movement;
    private int CurrentPointToMove = -1;
    private float PointMinDist = 0.2f;
    private float distanceToTarget = 0;
    private float WaitingTime = 0;
    
    private void Start()
    {
        OriginalColor = TargetedSR.color;
        TargetedColor = new Color(1, 0, 0, OriginalColor[3]);
        if (PatrolPoints.Length != 0)
        {
            rb = GetComponent<Rigidbody2D>();
            Speed = SpeedTypeData.GetDataByID(speedType);
            PickNextPoint();
        }
    }

    private void Update() 
    {
        if (isPatrolling)
        {
            distanceToTarget = Vector2.Distance(this.transform.position, PatrolPoints[CurrentPointToMove].position);
            if (distanceToTarget < PointMinDist)
            {
                PickNextPoint();
            }
            this.Movement = PatrolPoints[CurrentPointToMove].position - this.transform.position;
            this.Movement = this.Movement.normalized;
        }
    }

    private void FixedUpdate() 
    {
        if (isPatrolling)
        {
            this.rb.MovePosition(this.rb.position + this.Movement * (Speed * Time.fixedDeltaTime));
        }
    }

    private void OnDestroy() 
    {
        var SpellsToClear = FindObjectsOfType<Spell>();
        Enemy nullenemy = null;
        for (var i = 0; i < SpellsToClear.Length; i++)
        {
            if(SpellsToClear[i].GetTarget().name == this.name)
            {
                SpellsToClear[i].SetTarget(nullenemy);
            }
        }
    }
    
    private void PickNextPoint()
    {
        isPatrolling = false;
        WaitingTime = Random.Range(MinWaitingTime,MaxWaitingTime);
        StartCoroutine("WaitAtPoint");
    }

    private IEnumerator WaitAtPoint()
    {
        yield return new WaitForSeconds(WaitingTime);
        CurrentPointToMove++;
        if (CurrentPointToMove >= PatrolPoints.Length)
        {
            CurrentPointToMove = 0;
        }
        isPatrolling = true;
    }

    public void Target()
    {
        isTargeted = true;
        TargetedSR.color = TargetedColor;
    }

    public void ResetTarget()
    {
        isTargeted = false;
        TargetedSR.color = OriginalColor;
    }

    public bool CheckTargetStatus()
    {
        return this.isTargeted;
    }

    public void SetSpeed(float speed)
    {
        this.Speed = speed;
    }

    public SpeedType GetSpeed()
    {
        return this.speedType;
    }
}
