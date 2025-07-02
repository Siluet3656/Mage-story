using Data.Enums;
using UnityEngine;

namespace View
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] private StatusPanel _statusPanel;
    
        public void AddStatusEffect(StatusType type, GameObject target)
        {
            _statusPanel.AddStatusEffect(type, target);
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