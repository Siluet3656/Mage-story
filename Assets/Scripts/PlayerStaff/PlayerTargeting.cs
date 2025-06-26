using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PlayerStaff
{
    [RequireComponent(typeof(PlayerSpellCasting))]
    public class PlayerTargeting : MonoBehaviour
    {
        [SerializeField] private float _interactionRange = 10f;

        private PlayerSpellCasting _spellCasting;
        
        private Enemy _currentTarget;
        
        private readonly List<Enemy> _nearbyEnemies = new List<Enemy>();
        private List<Enemy> _enemiesInRange = new List<Enemy>();
        
        public bool HasTarget => _currentTarget != null;
        public Enemy GetTarget => _currentTarget;

        private void Awake()
        {
            _spellCasting = GetComponent<PlayerSpellCasting>();
        }

        private void Update()
        {
            if (IsTargetInRange() == false)
            {
                InterruptCast();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && _nearbyEnemies.Contains(enemy) == false)
            {
                _nearbyEnemies.Add(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && _nearbyEnemies.Contains(enemy))
            {
                _nearbyEnemies.Remove(enemy);
            }
        }
        
        private void OrderEnemiesInRange()
        {
            _enemiesInRange = _nearbyEnemies
                .Where(enemy => Vector2.Distance(transform.position, enemy.transform.position) <= _interactionRange)
                .OrderBy(enemy => Vector2.Distance(transform.position, enemy.transform.position))
                .ToList();
        }
        
        private void ClearTarget()
        {
            if (HasTarget)
            {
                _currentTarget.ResetTarget();
                _currentTarget = null;
            }
        }

        private bool IsTargetInRange()
        {
            return HasTarget &&
                   Vector2.Distance(transform.position, _currentTarget.transform.position) <= _interactionRange;
        }

        private void InterruptCast()
        {
            if (_spellCasting.Casting) _spellCasting.StopCast();
            ClearTarget();
        }
        
        public void OnFastTarget()
        {
            OrderEnemiesInRange();

            if (_enemiesInRange.Count == 0)
            {
                ClearTarget();
                return;
            }

            if (HasTarget)
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

        public void OnMouseTargetSelect(RaycastHit2D hit)
        {
            ClearTarget();

            if (hit.collider != null && hit.collider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Target();
                _currentTarget = enemy;
            }
        }
    }
}