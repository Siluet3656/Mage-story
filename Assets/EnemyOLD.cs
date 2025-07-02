using System.Collections;
using Data.Enums;
using UnityEngine;
using Random = UnityEngine.Random;
using Data;
using Statuses;
using UnityEngine.Serialization;

public class EnemyOld : MonoBehaviour
{
    [FormerlySerializedAs("PatrolPoints")] [SerializeField]private Transform[] _patrolPoints;
    [FormerlySerializedAs("TargetedSR")] [SerializeField]private SpriteRenderer _targetedSr = null;
    [FormerlySerializedAs("speedType")] [SerializeField]private SpeedType _speedType;
    [FormerlySerializedAs("MinWaitingTime")] [SerializeField]private float _minWaitingTime = 0;
    [FormerlySerializedAs("MaxWaitingTime")] [SerializeField]private float _maxWaitingTime = 10;
    [FormerlySerializedAs("iceTomb")] [SerializeField]private GameObject _iceTomb;
    private Rigidbody2D _rb;
    private Color _originalColor;
    private Color _targetedColor;
    private bool _isTargeted = false;
    private bool _isPatrolling = false;
    private float _speed = 0;
    private Vector2 _movement;
    private int _currentPointToMove = -1;
    private float _pointMinDist = 0.2f;
    private float _distanceToTarget = 0;
    private float _waitingTime = 0;
    
    //private Buff _enemyBuffs;
    private bool _isAvailableToMove = true;

    private void Awake()
    {
        /*_enemyBuffs = this.GetComponent<Buff>();
        
        _enemyBuffs.OnEnemyFreeze += FreezeMovement;
        _enemyBuffs.OnEnemyUnFreeze += UnfreezeMovement;*/
    }

    private void Start()
    {
        _originalColor = _targetedSr.color;
        _targetedColor = new Color(1, 0, 0, _originalColor[3]);
        if (_patrolPoints.Length != 0)
        {
            _rb = GetComponent<Rigidbody2D>();
            _speed = SpeedData.GetDataByType(_speedType);
            PickNextPoint();
        }
    }

    private void Update() 
    {
        if (_isAvailableToMove)
        {
            if (_isPatrolling)
            {
                _distanceToTarget = Vector2.Distance(this.transform.position, _patrolPoints[_currentPointToMove].position);
                if (_distanceToTarget < _pointMinDist)
                {
                    PickNextPoint();
                }
                this._movement = _patrolPoints[_currentPointToMove].position - this.transform.position;
                this._movement = this._movement.normalized;
            }
        }
        else
        {
            this._movement = Vector2.zero;
        }
    }

    private void FixedUpdate() 
    {
        if (_isPatrolling)
        {
            this._rb.MovePosition(this._rb.position + this._movement * (_speed * Time.fixedDeltaTime));
        }
    }

    private void OnDestroy() 
    {
        /*var spellsToClear = FindObjectsOfType<Spell>();
        var palyerToClear = FindObjectOfType<Player>();
        Enemy nullenemy = null;
        for (var i = 0; i < spellsToClear.Length; i++)
        {
            if(spellsToClear[i].GetTarget().name == this.name)
            {
                spellsToClear[i].SetTarget(nullenemy);
            }
        }
        if(palyerToClear)
        {
            palyerToClear.ClearTarget();
            palyerToClear.StopAllCasts();
        }*/
    }
    
    private void PickNextPoint()
    {
        _isPatrolling = false;
        _waitingTime = Random.Range(_minWaitingTime,_maxWaitingTime);
        StartCoroutine("WaitAtPoint");
    }

    private IEnumerator WaitAtPoint()
    {
        yield return new WaitForSeconds(_waitingTime);
        _currentPointToMove++;
        if (_currentPointToMove >= _patrolPoints.Length)
        {
            _currentPointToMove = 0;
        }
        _isPatrolling = true;
    }

    private void FreezeMovement()
    {
        _isAvailableToMove = false;
        _iceTomb.SetActive(true);
    }
    
    private void UnfreezeMovement()
    {
        _isAvailableToMove = true;
        _iceTomb.SetActive(false);
    }

    public void Target()
    {
        _isTargeted = true;
        _targetedSr.color = _targetedColor;
    }

    public void ResetTarget()
    {
        _isTargeted = false;
        _targetedSr.color = _originalColor;
    }

    public bool CheckTargetStatus()
    {
        return this._isTargeted;
    }

    public void SetSpeed(float speed)
    {
        this._speed = speed;
    }

    public SpeedType GetSpeed()
    {
        return this._speedType;
    }

    public void ShufflePoints()
    {
        for (int i = _patrolPoints.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0,i - 1);
            // обменять значения data[j] и data[i]
            var temp = _patrolPoints[j];
            _patrolPoints[j] = _patrolPoints[i];
            _patrolPoints[i] = temp;
        }
    }
}
