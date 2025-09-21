using UnityEngine;
using Data.Enums;
using Statuses;

namespace Effects
{
    [RequireComponent(typeof(StatusController))]
    public class PlayerEffects : MonoBehaviour
    {
        private StatusController _playerStatusController;
        private Transform _fireAuraEffects;

        private void Awake()
        {
            _playerStatusController = GetComponent<StatusController>();
            
            Transform[] allChildren = GetComponentsInChildren<Transform>(includeInactive: true);

            foreach (Transform child in allChildren)
            {
                if (child == transform) continue;
                
                if (child.CompareTag("FireAuraEffects"))
                {
                    _fireAuraEffects = child;
                }
            }
            
            //Debug.Log(_playerStatusController);
            //Debug.Log(_fireAuraEffects);
        }

        private void OnEnable()
        {
            _playerStatusController.OnBuffGained += ShowEffect;
            _playerStatusController.OnBuffLost += HideEffect;
        }

        private void OnDisable()
        {
            _playerStatusController.OnBuffGained -= ShowEffect;
            _playerStatusController.OnBuffLost -= HideEffect;
        }

        private void ShowEffect(StatusType type)
        {
            switch (type)
            {
                case StatusType.FireAura:
                    _fireAuraEffects.gameObject.SetActive(true);
                    break;
            }
        }

        private void HideEffect(StatusType type)
        {
            switch (type)
            {
                case StatusType.FireAura:
                    _fireAuraEffects.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
