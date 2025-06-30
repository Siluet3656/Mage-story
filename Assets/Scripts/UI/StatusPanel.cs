using System.Collections.Generic;
using Data.Enums;
using UnityEngine;

namespace UI
{
    public class StatusPanel : MonoBehaviour
    {
        [Header("Icon Prefabs")]
        [SerializeField] private StatusIconPrefabs _iconPrefabs;

        [Header("Player Display Settings")]
        [SerializeField] private StatusDisplaySettings _playerSettings;

        [Header("Enemy Display Settings")]
        [SerializeField] private StatusDisplaySettings _enemySettings;

        private Dictionary<StatusType, GameObject> _activeStatusIcons = new Dictionary<StatusType, GameObject>();
        private Dictionary<GameObject, StatusDisplaySettings> _targetSettings = new Dictionary<GameObject, StatusDisplaySettings>();

        public void AddStatusEffect(StatusType type, GameObject target)
        {
            // Don't add duplicate status icons
            if (_activeStatusIcons.ContainsKey(type))
                return;

            // Get the appropriate prefab
            GameObject prefab = GetPrefabForStatus(type);
            if (prefab == null)
            {
                Debug.LogWarning($"No prefab assigned for status type: {type}");
                return;
            }

            // Get display settings based on target
            if (!_targetSettings.TryGetValue(target, out var settings))
            {
                settings = target.CompareTag("Player") ? _playerSettings : _enemySettings;
                _targetSettings[target] = settings;
            }

            // Create and position the icon
            GameObject icon = Instantiate(prefab, transform);
            int statusCount = _activeStatusIcons.Count;
            icon.transform.localPosition = new Vector3(
                settings.DefaultX + settings.Offset * statusCount,
                settings.DefaultY,
                0
            );

            _activeStatusIcons.Add(type, icon);
        }

        public void RemoveStatusEffect(StatusType type, GameObject target)
        {
            if (_activeStatusIcons.TryGetValue(type, out var icon))
            {
                Destroy(icon);
                _activeStatusIcons.Remove(type);
                RefreshStatusPositions(target);
            }
        }

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

        private GameObject GetPrefabForStatus(StatusType type)
        {
            switch (type)
            {
                case StatusType.Slow: return _iconPrefabs.Slow;
                case StatusType.Poison: return _iconPrefabs.Poison;
                case StatusType.FireMark: return _iconPrefabs.FireMark;
                case StatusType.FireAura: return _iconPrefabs.FireAura;
                case StatusType.StasisFreeze: return _iconPrefabs.IceTomb;
                default: return null;
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