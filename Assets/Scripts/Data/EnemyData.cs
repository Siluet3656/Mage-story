using System.Collections.Generic;
using Data.EnemyConfigs;
using UnityEngine;
using Data.Enums;

namespace Data
{
    public class EnemyData : MonoBehaviour
    {
        #region Singleton

        public EnemyData()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        public static EnemyData Instance { get; private set; }

        #endregion
        
        [Header("Prison")]
        [SerializeField] private EnemyConfig _prisoner;
        [SerializeField] private EnemyConfig _hunter;
        [SerializeField] private EnemyConfig _paladin;
        
        private Dictionary<EnemyName, EnemyConfig> _enemyValues;
        
        private void Awake()
        {
            _enemyValues = new Dictionary<EnemyName, EnemyConfig>
            {
                { EnemyName.Prisoner, _prisoner },
                { EnemyName.Hunter, _hunter },
                { EnemyName.Paladin, _paladin }
            };
        }
        
        public EnemyConfig GetEnemyConfig(EnemyName enemyName)
        {
            if (_enemyValues.TryGetValue(enemyName, out EnemyConfig config))
            {
                return config;
            }
    
            return null;
        }
    }
}