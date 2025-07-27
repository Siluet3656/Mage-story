using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EnemyStaff;

namespace PlayerStaff
{
    [RequireComponent(typeof(PlayerSpellCasting))]
    public class PlayerTargeting : MonoBehaviour
    {
        [SerializeField] private TargetingCircle _targetingCircle;
        [SerializeField] private float _interactionRange = 10f;

        private PlayerSpellCasting _spellCasting;
        
        private ITargetable _currentTarget;
        
        private List<ITargetable> _targetsInRange = new List<ITargetable>();
        
        public bool HasTarget => _currentTarget != null;
        public ITargetable GetCurrentTarget => _currentTarget;

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
        
        private void OrderEnemiesInRange()
        {
            _targetsInRange = _targetingCircle.NearbyTargets
                .Where(target => Vector2.Distance(transform.position, target.GameObject.transform.position) <= _interactionRange)
                .OrderBy(enemy => Vector2.Distance(transform.position, enemy.GameObject.transform.position))
                .ToList();
        }

        private void SetTarget(ITargetable target)
        {
            target.OnTargeted();
            target.OnTargetDestroy += ClearTarget;
            _currentTarget = target;
        }
        
        private void ClearTarget()
        {
            if (HasTarget)
            {
                _currentTarget.OnUntargeted();
                _currentTarget.OnTargetDestroy -= ClearTarget;
                _currentTarget = null;
            }
        }

        private bool IsTargetInRange()
        {
            return HasTarget &&
                   Vector2.Distance(transform.position, _currentTarget.GameObject.transform.position) <= _interactionRange;
        }

        private void InterruptCast()
        {
            if (_spellCasting.Casting && _spellCasting.RequireTarget) _spellCasting.StopCast();
            ClearTarget();
        }
        
        public void OnFastTarget()
        {
            ITargetable target;
            
            OrderEnemiesInRange();

            if (_targetsInRange.Count == 0)
            {
                ClearTarget();
                return;
            }

            if (HasTarget)
            {
                int currentIndex = _targetsInRange.IndexOf(_currentTarget);
                ClearTarget();

                target = currentIndex == -1 || currentIndex >= _targetsInRange.Count - 1
                    ? _targetsInRange[0]
                    : _targetsInRange[currentIndex + 1];
            }
            else
            {
                target = _targetsInRange[0];
            }
            
            SetTarget(target);
        }

        public void OnMouseTargetSelect(RaycastHit2D hit)
        {
            ClearTarget();

            if (hit.collider != null && hit.collider.TryGetComponent<ITargetable>(out var target))
            {
                SetTarget(target);
            }
        }
    }
}