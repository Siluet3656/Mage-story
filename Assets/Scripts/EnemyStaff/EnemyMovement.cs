using System;
using UnityEngine;
using Data;
using Data.Enums;
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
        }

        public event Action<bool, MovementDisableSource> OnMovementAvailabilityChanged;
        
        public SpeedType CurrentSpeed => _currentSpeed;
        public SpeedType DefaultSpeed => _defaultSpeed;
        
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
