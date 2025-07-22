using UnityEngine;
using Data.Enums;
using Data.StatusConfigs;

namespace View
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] private StatusPanel _statusPanel;
    
        public void AddStatusEffect(StatusEffectData data, GameObject target)
        {
            _statusPanel.AddStatusEffect(data, target);
        }
    
        public void RemoveStatusEffect(StatusType type, GameObject target)
        {
            _statusPanel.RemoveStatusEffect(type, target);
        }
    
        public void ClearAllStatuses()
        {
            _statusPanel.ClearAllStatuses();
        }
    }
}