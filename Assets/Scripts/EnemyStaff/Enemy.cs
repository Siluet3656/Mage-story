using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Data;
using Data.Enums;
using EntityStaff;
using Pathfinding;
using View;

namespace EnemyStaff
{
    [RequireComponent(typeof(EnemyView))]
    public class Enemy : MonoBehaviour, ITargetable
    {
        [Header("Movement Settings")]
        [SerializeField] private SpeedType _defaultSpeed;
        [SerializeField] private float _minWaitingTime;
        [SerializeField] private float _maxWaitingTime;
        [SerializeField] private float _pointMinDist;
        [SerializeField] private bool _isAvailableToMove;

        private List<Node> _pathToMove = new List<Node>();
        
        private Rigidbody2D _rb;
        private SpeedType _currentSpeed;
        private Vector2 _movementDirection;
        private int _currentPointIndex;
        private float _waitingTime;
        private bool _isTargeted;

        public SpeedType CurrentSpeed => _currentSpeed;
        public SpeedType DefaultSpeed => _defaultSpeed;
        public bool IsTargeted => _isTargeted;
        public bool IsTargetable { get; private set; }

        public event Action<bool> OnTargetStatusChanged;
        public event Action<bool, MovementDisableSource> OnMovementAvailabilityChanged;

        private void Awake()
        {
            IsTargetable = true;
            
            InitializeMovement();
        }
        
        private void FixedUpdate()
        {
            if (_isAvailableToMove)
            {
                if (_pathToMove.Count > 0)
                {
                    MoveCharacter(_pathToMove.First());
                }
                else
                {
                    CreatePath();
                }
            }
        }
        
        private void InitializeMovement()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentSpeed = _defaultSpeed;
        }

        private void CreatePath()
        {
            if (AStar.Instance.NodesOnScene.Count > 0)
            {
                Node start = AStar.Instance.NodesOnScene[Random.Range(0, AStar.Instance.NodesOnScene.Count)];
                Node end = AStar.Instance.NodesOnScene[Random.Range(0, AStar.Instance.NodesOnScene.Count)];

                if (start == end) return;
                
                _pathToMove = AStar.Instance.GeneratePath(start, end);

                if (_pathToMove == null) return;
            }
        }

        private void OnDestroy()
        {
            OnTargetDestroy?.Invoke();
        }

        private void MoveCharacter(Node nodeMovingTo)
        {
            float speed = SpeedData.GetDataByType(_currentSpeed);
            _movementDirection = nodeMovingTo.transform.position - transform.position;
            _movementDirection = _movementDirection.normalized;
            
            _rb.MovePosition(_rb.position + _movementDirection * (speed * Time.fixedDeltaTime));
            
            if (Vector2.Distance(transform.position, nodeMovingTo.transform.position) < _pointMinDist)
            {
                _pathToMove.Remove(_pathToMove.First());
            }
        }
        
        public GameObject GameObject => gameObject;
        public event Action OnTargetDestroy;
        
        public void OnTargeted()
        {
            SetTargetStatus(true);
        }

        public void OnUntargeted()
        {
            SetTargetStatus(false);
        }
        
        public void SetMovementAvailability(bool isAvailable, MovementDisableSource source)
        {
            _isAvailableToMove = isAvailable;
            OnMovementAvailabilityChanged?.Invoke(isAvailable, source);
        
            if (isAvailable == false)
            {
                _movementDirection = Vector2.zero;
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
    }
}