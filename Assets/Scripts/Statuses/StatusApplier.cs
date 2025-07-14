using UnityEngine;
using Data.Enums;
using Data.StatusConfigs;


namespace Statuses
{
    public class StatusApplier : MonoBehaviour
    {
        [SerializeField] private StatusEffectData[] _statusEffectsData;
    
        public void ApplyStatusToTarget(StatusType type, GameObject target)
        {
            var data = GetStatusData(type);
            if (data != null && target != null)
            {
                var statusController = target.GetComponent<StatusController>();
                if (statusController == null)
                {
                    statusController = target.AddComponent<StatusController>();
                }
            
                statusController.ApplyStatus(data);
            }
        }
    
        private StatusEffectData GetStatusData(StatusType type)
        {
            foreach (var data in _statusEffectsData)
            {
                if (data.Type == type)
                {
                    return data;
                }
            }
            return null;
        }
    }
}