using EnemyStaff;
using UnityEngine;

namespace View
{
    public class EnemyView : MonoBehaviour
    {
        [Header("Visual References")]
        [SerializeField] private SpriteRenderer _targetedMark;
        [SerializeField] private GameObject _iceTomb;

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

        private void HandleMovementAvailabilityChanged(bool isAvailable)
        {
            _iceTomb.SetActive(!isAvailable);
        }
    }
}