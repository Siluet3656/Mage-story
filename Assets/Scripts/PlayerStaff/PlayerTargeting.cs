using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PlayerStaff
{
    public class PlayerTargeting : MonoBehaviour
    {
        [SerializeField] private float _interactionRange = 10f;

        private Enemy _currentTarget;
        private List<Enemy> _enemiesInRange = new List<Enemy>();
        private PlayerInputActions _inputActions;

        public Enemy CurrentTarget => _currentTarget;
        public bool HasTarget => _currentTarget != null;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Player.FastTarget.started += OnFastTarget;
            _inputActions.UI.LBM.started += OnMouseClick;
        }

        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();

        private void OnFastTarget(InputAction.CallbackContext context)
        {
            UpdateEnemiesInRange();

            if (_enemiesInRange.Count == 0)
            {
                ClearTarget();
                return;
            }

            if (_currentTarget != null)
            {
                int currentIndex = _enemiesInRange.IndexOf(_currentTarget);
                ClearTarget();

                _currentTarget = currentIndex == -1 || currentIndex >= _enemiesInRange.Count - 1
                    ? _enemiesInRange[0]
                    : _enemiesInRange[currentIndex + 1];
            }
            else
            {
                _currentTarget = _enemiesInRange[0];
            }

            _currentTarget.Target();
        }

        private void OnMouseClick(InputAction.CallbackContext context)
        {
            var ray = Camera.main.ScreenPointToRay(_inputActions.UI.MousePosition.ReadValue<Vector2>());
            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            ClearTarget();

            if (hit.collider != null && hit.collider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Target();
                _currentTarget = enemy;
            }
        }

        private void UpdateEnemiesInRange()
        {
            _enemiesInRange = FindObjectsOfType<Enemy>()
                .Where(enemy => Vector2.Distance(transform.position, enemy.transform.position) <= _interactionRange)
                .OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position))
                .ToList();
        }

        public void ClearTarget()
        {
            if (_currentTarget != null)
            {
                _currentTarget.ResetTarget();
                _currentTarget = null;
            }
        }

        public bool IsTargetInRange()
        {
            return _currentTarget != null &&
                   Vector2.Distance(transform.position, _currentTarget.transform.position) <= _interactionRange;
        }
    }
}