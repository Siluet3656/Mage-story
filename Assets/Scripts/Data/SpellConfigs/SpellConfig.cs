using UnityEngine;
using Data.Enums;
using Spells;

namespace Data.SpellConfigs
{
    public class SpellConfig : ScriptableObject
    {
        public SpellType Type { get; } = SpellType.Projectile;

        [Header("Basic")]
        [SerializeField] private SpellName _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private bool _requiresTarget;
        
        public Sprite Icon => _icon;
        public SpellName SpellName => _name;
        public bool RequiresTarget => _requiresTarget;
        
        [Header("Resources")]
        [SerializeField, Min(0)] private Vector3Int _shardCost;
        [SerializeField, Min(0)] private float _reminderCost;
        
        public Vector3Int ShardCost => _shardCost;
        public float ReminderCost => _reminderCost;
    }
}