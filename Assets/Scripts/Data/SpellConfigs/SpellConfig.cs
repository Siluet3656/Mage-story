using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    public class SpellConfig : ScriptableObject
    {
        public SpellType Type { get; } = SpellType.Projectile;

        [Header("Basic")]
        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;
        
        [SerializeField] private bool _requiresTarget = true;
        public bool RequiresTarget => _requiresTarget;
        
        [Header("Resources")]
        [SerializeField, Min(0)] private Vector3Int _shardCost;
        public Vector3Int ShardCost => _shardCost;
        
        [SerializeField, Min(0)] private float _reminderCost;
        public float ReminderCost => _reminderCost;
    }
}