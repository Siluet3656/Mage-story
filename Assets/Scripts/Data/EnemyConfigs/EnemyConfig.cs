using System;
using UnityEngine;
using Data.Enums;
using EnemyStaff.StateSO;

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
        
        [Header("Behavior")]
        [SerializeField] private EnemyIdleSoBase _enemyIdle;
        [SerializeField] private EnemyEngageSoBase _enemyEngage;
        [SerializeField] private EnemyAttackSoBase _enemyAttack;
        [SerializeField] private EnemyWanderingSoBase _enemyWandering;
        [SerializeField] private EnemyRetreatSoBase _enemyRetreat;
        
        public String Name => _name;
        public Sprite Sprite => _sprite;
        public float MaxHp => _maxHp;
        public EnemyIdleSoBase EnemyIdle => _enemyIdle;
        public EnemyEngageSoBase EnemyEngage =>_enemyEngage;
        public EnemyAttackSoBase EnemyAttack => _enemyAttack;
        public EnemyWanderingSoBase EnemyWandering => _enemyWandering;
        public EnemyRetreatSoBase EnemyRetreat =>_enemyRetreat;
    }
}
