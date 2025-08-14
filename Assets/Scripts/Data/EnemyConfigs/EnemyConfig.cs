using System;
using Data.Enums;
using UnityEngine;

namespace Data.EnemyConfigs
{
    public abstract class EnemyConfig : ScriptableObject
    {
        public abstract EnemyType GetEnemyType();
        
        [Header("Basic")]
        [SerializeField] private String _name;
        [SerializeField] private Sprite _sprite;
        
        [Header("Resources")]
        [SerializeField, Min(1)] private float _maxHp;
        
        public String Name => _name;
        public Sprite Sprite => _sprite;
        public float MaxHp => _maxHp;
    }
}
