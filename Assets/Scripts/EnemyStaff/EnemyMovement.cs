using System;
using Data;
using UnityEngine;
using Data.Enums;
using EnemyStaff.ConcreteState;
using View;

namespace EnemyStaff
{
    [RequireComponent(typeof(EnemyView))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private SpeedType _defaultSpeed;
        [SerializeField] private bool _isAvailableToMove;
        
        private  Rigidbody2D _rigidbody;
        
        private SpeedType _currentSpeed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            StateMachine = new EnemyStateMachine();

            IdleState = new IdleState(this, StateMachine);
            EngageState = new EngageState(this, StateMachine);
            AttackState = new AttackState(this, StateMachine);
        }

        private void Start()
        {
            StateMachine.Initialize(EngageState);
        }

        private void Update()
        {
            StateMachine.CurrentEnemyState.FrameUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentEnemyState.PhysicsUpdate();
        }

        public event Action<bool, MovementDisableSource> OnMovementAvailabilityChanged;
        
        public SpeedType CurrentSpeed => _currentSpeed;
        public SpeedType DefaultSpeed => _defaultSpeed;
        
        public EnemyStateMachine StateMachine { get; set; }
        public IdleState IdleState { get; set; }
        public EngageState EngageState { get; set; }
        public AttackState AttackState { get; set; }

        public void Move(Vector2 movementDirection)
        {
            if (_isAvailableToMove)
            {
                _rigidbody.MovePosition(_rigidbody.position + movementDirection * (SpeedData.GetDataByType(_currentSpeed) * Time.fixedDeltaTime));
            }
        }
        
        public void SetSpeed(SpeedType speed)
        {
            _currentSpeed = speed;
        }
        
        public void SetMovementAvailability(bool isAvailable, MovementDisableSource source)
        {
            _isAvailableToMove = isAvailable;
            OnMovementAvailabilityChanged?.Invoke(isAvailable, source);
        }
    }
}
