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
        [SerializeField] private EnemyName _name;
        [SerializeField] private String _title;
        [SerializeField] private Sprite _sprite;
        
        [Header("Resources")]
        [SerializeField, Min(1)] private int _maxHp;
        
        [Header("Behavior")]
        [SerializeField] private EnemyIdleSoBase _enemyIdle;
        [SerializeField] private EnemyEngageSoBase _enemyEngage;
        [SerializeField] private EnemyAttackSoBase _enemyAttack;
        [SerializeField] private EnemyWanderingSoBase _enemyWandering;
        [SerializeField] private EnemyRetreatSoBase _enemyRetreat;
        
        public EnemyName Name => _name;
        public String Title => _title;
        public Sprite Sprite => _sprite;
        public int MaxHp => _maxHp;
        public EnemyIdleSoBase EnemyIdle => _enemyIdle;
        public EnemyEngageSoBase EnemyEngage =>_enemyEngage;
        public EnemyAttackSoBase EnemyAttack => _enemyAttack;
        public EnemyWanderingSoBase EnemyWandering => _enemyWandering;
        public EnemyRetreatSoBase EnemyRetreat =>_enemyRetreat;
    }
}
