using UnityEngine;
using UnityEngine.UI;
using Data.Enums;
using EnemyStaff;
using Statuses;

namespace View
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyTargeting))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(StatusController))]
    public class EnemyView : MonoBehaviour
    {
        [Header("Visual References")]
        [SerializeField] private SpriteRenderer[] _targetedMarks;
        [SerializeField] private Color _targetedColor;
        [SerializeField] private GameObject _iceTomb;
        [SerializeField] private GameObject _roots;
        [SerializeField] private GameObject _fireMark;
        [SerializeField] private Image _attackSwingBar;
        [SerializeField] private Text _enemyTitle;
        [SerializeField] private SpriteRenderer _enemySpriteRenderer;
        [SerializeField] private Material _outlineMaterial;
        [SerializeField] private Material _originalMaterial;
        
        private EnemyTargeting _enemyTargeting;
        private EnemyMovement _enemyMovement;
        private StatusController _statusController;
        
        private Color _originalColor;
        
        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _enemyTargeting = GetComponent<EnemyTargeting>();
            _statusController = GetComponent<StatusController>();

            InitializeColors();
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }
        
        private void InitializeColors()
        {
            _originalColor = Color.white;
        }

        private void SubscribeToEvents()
        {
            _enemyTargeting.OnTargetStatusChanged += HandleTargetStatusChanged;
            _enemyMovement.OnMovementAvailabilityChanged += HandleMovementAvailabilityChanged;
            _statusController.OnStatusGained += ShowEffect;
            _statusController.OnStatusLost += HideEffect;
        }

        private void UnsubscribeFromEvents()
        {
            _enemyTargeting.OnTargetStatusChanged -= HandleTargetStatusChanged;
            _enemyMovement.OnMovementAvailabilityChanged -= HandleMovementAvailabilityChanged;
            _statusController.OnStatusGained -= ShowEffect;
            _statusController.OnStatusLost -= HideEffect;
        }
        
        private void HandleTargetStatusChanged(bool isTargeted)
        {
            foreach (var mark in _targetedMarks)
            {
                mark.color = isTargeted ? _targetedColor : _originalColor;
            }

            _enemySpriteRenderer.material = isTargeted?  _outlineMaterial: _originalMaterial;
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

        private void ShowEffect(StatusType effect)
        {
            switch (effect)
            {
                case StatusType.FireMark:
                    _fireMark.SetActive(true);
                    break;
            }
        }

        private void HideEffect(StatusType effect)
        {
            switch (effect)
            {
                case StatusType.FireMark:
                    _fireMark.SetActive(false);
                    break;
            }
        }
        
        public void UpdateAttackSwingBar(float swingProgress)
        {
            _attackSwingBar.fillAmount = swingProgress;
        }

        public void SetSprite(Sprite sprite)
        {
            _enemySpriteRenderer.sprite = sprite;
        }

        public void SetTitle(string title)
        {
            _enemyTitle.text = title;
        }
    }
}