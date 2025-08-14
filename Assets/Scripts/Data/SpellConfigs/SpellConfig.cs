using UnityEngine;
using Data.Enums;

namespace Data.SpellConfigs
{
    public abstract class SpellConfig : ScriptableObject
    {
        public abstract SpellType GetSPellType();
        
        [Header("Basic")]
        [SerializeField] private SpellName _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private bool _requiresTarget;
        [SerializeField] private SpellElementType _spellElementType;
        
        [Header("Resources")]
        [SerializeField, Min(0)] private Vector3Int _shardCost;
        [SerializeField, Min(0)] private float _reminderCost;
        
        public Sprite Icon => _icon;
        public SpellName SpellName => _name;
        public bool RequiresTarget => _requiresTarget;
        public SpellElementType SpellElementType => _spellElementType;
        
        public Vector3Int ShardCost => _shardCost;
        public float ReminderCost => _reminderCost;
    }
}