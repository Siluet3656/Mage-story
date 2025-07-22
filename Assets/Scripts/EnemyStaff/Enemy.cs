using System;
using System.Collections;
using Data;
using Data.Enums;
using UnityEngine;
using View;
using Random = UnityEngine.Random;

namespace EnemyStaff
{
    [RequireComponent(typeof(EnemyView))]
    public class Enemy : MonoBehaviour, ITargetble
    {
        [Header("Movement Settings")]
        [SerializeField] private SpeedType _defaultSpeed;
        [SerializeField] private float _minWaitingTime;
        [SerializeField] private float _maxWaitingTime;
        [SerializeField] private float _pointMinDist;

        [Header("Patrol Points")]
        [SerializeField] private Transform[] _patrolPoints;

        private Rigidbody2D _rb;
        private SpeedType _currentSpeed;
        private Vector2 _movementDirection;
        private int _currentPointIndex;
        private float _waitingTime;
        private bool _isTargeted;
        private bool _isAvailableToMove;
        private bool _isPatrolling;

        public SpeedType CurrentSpeed => _currentSpeed;
        public SpeedType DefaultSpeed => _defaultSpeed;
        public bool IsTargeted => _isTargeted;
        public bool IsTargetable { get; }

        public event Action<bool> OnTargetStatusChanged;
        public event Action<bool, MovementDisableSource> OnMovementAvailabilityChanged;
        public event Action<Vector2> OnMovementDirectionChanged;

        private void Awake()
        {
            InitializeMovement();
        }

        private void Update()
        {
            if (_isAvailableToMove && _isPatrolling)
            {
                UpdatePatrolMovement();
            }
        }

        private void FixedUpdate()
        {
            if (_isPatrolling)
            {
                MoveCharacter();
            }
        }
        
        private void InitializeMovement()
        {
            _rb = GetComponent<Rigidbody2D>();
            _isAvailableToMove = true;
            _currentSpeed = _defaultSpeed;
        
            if (_patrolPoints.Length > 0)
            {
                StartPatrol();
            }
        }

        private void OnDestroy()
        {
            OnTargetDestroy?.Invoke();
        }

        public void OnTargeted()
        {
            SetTargetStatus(true);
        }

        public void OnUntargeted()
        {
            SetTargetStatus(false);
        }

        public GameObject GameObject => gameObject;
        public event Action OnTargetDestroy;

        private void StartPatrol()
        {
            _isPatrolling = true;
            PickNextPatrolPoint();
        }

        private void UpdatePatrolMovement()
        {
            float distance = Vector2.Distance(transform.position, _patrolPoints[_currentPointIndex].position);
            if (distance < _pointMinDist)
            {
                PickNextPatrolPoint();
            }

            _movementDirection = (_patrolPoints[_currentPointIndex].position - transform.position).normalized;
            OnMovementDirectionChanged?.Invoke(_movementDirection);
        }

        private void MoveCharacter()
        {
            float speed = SpeedData.GetDataByType(_currentSpeed);
            
            _rb.MovePosition(_rb.position + _movementDirection * (speed * Time.fixedDeltaTime));
        }

        private void PickNextPatrolPoint()
        {
            _isPatrolling = false;
            _waitingTime = Random.Range(_minWaitingTime, _maxWaitingTime);
            StartCoroutine(WaitAtPoint());
        }

        private IEnumerator WaitAtPoint()
        {
            yield return new WaitForSeconds(_waitingTime);
        
            _currentPointIndex = (_currentPointIndex + 1) % _patrolPoints.Length;
            _isPatrolling = true;
        }
        
        public void SetMovementAvailability(bool isAvailable, MovementDisableSource source)
        {
            _isAvailableToMove = isAvailable;
            OnMovementAvailabilityChanged?.Invoke(isAvailable, source);
        
            if (!isAvailable)
            {
                _movementDirection = Vector2.zero;
                OnMovementDirectionChanged?.Invoke(_movementDirection);
            }
        }

        public void SetTargetStatus(bool isTargeted)
        {
            _isTargeted = isTargeted;
            OnTargetStatusChanged?.Invoke(isTargeted);
        }

        public void SetSpeed(SpeedType speed)
        {
            _currentSpeed = speed;
        }

        public void ShufflePatrolPoints()
        {
            for (int i = _patrolPoints.Length - 1; i >= 1; i--)
            {
                int j = Random.Range(0, i);
                (_patrolPoints[j], _patrolPoints[i]) = (_patrolPoints[i], _patrolPoints[j]);
            }
        }
    }
}