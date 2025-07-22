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

        private Enemy _enemyModel;
        private Color _originalColor;
        private Color _targetedColor;

        private void Awake()
        {
            _enemyModel = GetComponent<Enemy>();

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
            _enemyModel.OnTargetStatusChanged += HandleTargetStatusChanged;
            _enemyModel.OnMovementAvailabilityChanged += HandleMovementAvailabilityChanged;
        }

        private void UnsubscribeFromEvents()
        {
            if (_enemyModel == null) return;
        
            _enemyModel.OnTargetStatusChanged -= HandleTargetStatusChanged;
            _enemyModel.OnMovementAvailabilityChanged -= HandleMovementAvailabilityChanged;
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