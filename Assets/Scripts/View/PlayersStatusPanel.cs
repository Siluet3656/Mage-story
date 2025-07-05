using UnityEngine;
using Data;

namespace View
{
    public class PlayersStatusPanel : StatusPanel
    {
        [Header("Player Display Settings")]
        [SerializeField] private StatusDisplaySettings _playerSettings;
        
        public override void AddStatusEffect(StatusEffectData data, GameObject target)
        {
            // Don't add duplicate status icons
            if (ActiveStatusIcons.ContainsKey(data.Type))
                return;

            // Get the appropriate prefab
            GameObject prefab = data.VisualEffectPrefab;
            if (prefab == null)
            {
                Debug.LogWarning($"No prefab assigned for status type: {data.Type}");
                return;
            }

            // Get display settings based on target
            if (!TargetSettings.TryGetValue(target, out var settings))
            {
                settings = _playerSettings;
                TargetSettings[target] = settings;
            }

            // Create and position the icon
            GameObject icon = Instantiate(prefab, transform);
            int statusCount = ActiveStatusIcons.Count;
            icon.transform.localPosition = new Vector3(
                settings.DefaultX + settings.Offset * statusCount,
                settings.DefaultY,
                0
            );

            ActiveStatusIcons.Add(data.Type, icon);
        }
    }
}
