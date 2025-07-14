using System.Collections.Generic;
using UnityEngine;
using Data;
using Data.Enums;
using Data.StatusConfigs;

namespace View
{
    public abstract class StatusPanel : MonoBehaviour
    {
        private readonly Dictionary<StatusType, GameObject> _activeStatusIcons = new Dictionary<StatusType, GameObject>();
        private readonly Dictionary<GameObject, StatusDisplaySettings> _targetSettings = new Dictionary<GameObject, StatusDisplaySettings>();
        
        protected Dictionary<StatusType, GameObject> ActiveStatusIcons => _activeStatusIcons;
        protected Dictionary<GameObject, StatusDisplaySettings> TargetSettings => _targetSettings;

        private void RefreshStatusPositions(GameObject target)
        {
            if (!_targetSettings.TryGetValue(target, out var settings))
                return;

            int index = 0;
            foreach (var icon in _activeStatusIcons.Values)
            {
                icon.transform.localPosition = new Vector3(
                    settings.DefaultX + settings.Offset * index,
                    settings.DefaultY,
                    0
                );
                index++;
            }
        }

        public abstract void AddStatusEffect(StatusEffectData data, GameObject target);

        public void RemoveStatusEffect(StatusType type, GameObject target)
        {
            if (_activeStatusIcons.TryGetValue(type, out var icon))
            {
                Destroy(icon);
                _activeStatusIcons.Remove(type);
                RefreshStatusPositions(target);
            }
        }

        public void ClearAllStatuses()
        {
            foreach (var icon in _activeStatusIcons.Values)
            {
                Destroy(icon);
            }
            _activeStatusIcons.Clear();
            _targetSettings.Clear();
        }
    }
}