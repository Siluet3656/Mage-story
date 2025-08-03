using System;
using UnityEngine;
using EntityStaff;
using View;

namespace EnemyStaff
{
    [RequireComponent(typeof(EnemyView))]
    public class EnemyTargeting : MonoBehaviour, ITargetable
    {
        private bool _isTargeted;
        
        public bool IsTargeted => _isTargeted;
        public bool IsTargetable { get; private set; }

        public event Action<bool> OnTargetStatusChanged;

        private void Awake()
        {
            IsTargetable = true;
        }

        private void OnDestroy()
        {
            OnTargetDestroy?.Invoke();
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

        public void SetTargetStatus(bool isTargeted)
        {
            _isTargeted = isTargeted;
            OnTargetStatusChanged?.Invoke(isTargeted);
        }
    }
}