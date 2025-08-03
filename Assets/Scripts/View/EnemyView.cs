using Data.Enums;
using EnemyStaff;
using UnityEngine;

namespace View
{
    public class EnemyView : MonoBehaviour
    {
        [Header("Visual References")]
        [SerializeField] private SpriteRenderer _targetedMark;
        [SerializeField] private GameObject _iceTomb;
        [SerializeField] private GameObject _roots;

        private EnemyTargeting _enemyTargeting;
        private EnemyMovement _enemyMovement;
        private Color _originalColor;
        private Color _targetedColor;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyTargeting = GetComponent<EnemyTargeting>();

            InitializeColors();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }
        
        private void InitializeColors()
        {
            _originalColor = _targetedMark.color;
            _targetedColor = new Color(1, 0, 0, _originalColor.a);
        }

        private void SubscribeToEvents()
        {
            _enemyTargeting.OnTargetStatusChanged += HandleTargetStatusChanged;
            _enemyMovement.OnMovementAvailabilityChanged += HandleMovementAvailabilityChanged;
        }

        private void UnsubscribeFromEvents()
        {
            if (_enemyMovement == null) return;
        
            _enemyTargeting.OnTargetStatusChanged -= HandleTargetStatusChanged;
            _enemyMovement.OnMovementAvailabilityChanged -= HandleMovementAvailabilityChanged;
        }
        
        private void HandleTargetStatusChanged(bool isTargeted)
        {
            _targetedMark.color = isTargeted ? _targetedColor : _originalColor;
        }

        private void HandleMovementAvailabilityChanged(bool isAvailable, MovementDisableSource source)
        {
            switch (source)
            {
                case MovementDisableSource.IceTomb:
                    _iceTomb.SetActive(!isAvailable);
                    break;
                case MovementDisableSource.Roots:
                    _roots.SetActive(!isAvailable);
                    break;
            }
        }
    }
}