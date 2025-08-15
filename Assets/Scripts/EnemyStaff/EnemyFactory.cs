using System;
using System.Collections.Generic;
using Data;
using Data.EnemyConfigs;
using UnityEngine;
using Data.EnemyConfigs.Melee;
using Data.Enums;

namespace EnemyStaff
{
    public class EnemyFactory : MonoBehaviour
    {
        #region Singleton

        public static EnemyFactory Instance { get; private set; }

        EnemyFactory()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        #endregion
        
        [Header("Base")] 
        [SerializeField, Min(1)] private int _defaultAmountOfEnemies = 10;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _enemyMelee;
        //[SerializeField] private GameObject _enemyRanged;
        //[SerializeField] private GameObject _enemyCaster;
        
        private readonly Dictionary<EnemyName, Queue<Enemy>> _enemyPools = new Dictionary<EnemyName, Queue<Enemy>>();
        
        private void Awake()
        {
            InitializeEnemyPools();
        }

        private void InitializeEnemyPools()
        {
            _enemyPools.Add(EnemyName.None, new Queue<Enemy>());
            
            InstantiateEnemies(EnemyName.Prisoner, typeof(Prisoner), _enemyMelee, _defaultAmountOfEnemies);
        }
        
        private void InstantiateEnemies(EnemyName enemyName, Type enemyType, GameObject prefab, int amountOfEnemies)
        {
            _enemyPools.Add(enemyName, new Queue<Enemy>());
            
            for (int i = 0; i < amountOfEnemies; i++)
            {
                InstantiateEnemy(enemyName, enemyType, prefab);
            }
        }

        private void InstantiateEnemy(EnemyName enemyName, Type enemyType, GameObject prefab)
        {
            GameObject enemy = Instantiate(prefab, transform);
            var enemyComponent = (Enemy)enemy.gameObject.AddComponent(enemyType);
            _enemyPools[enemyName].Enqueue(enemyComponent);
            enemy.gameObject.SetActive(false);
        }
        
        private T GetEnemy<T>(EnemyName enemyName, GameObject prefab) where T : class
        {
            if (_enemyPools == null) return null;

            T enemy;

            if (_enemyPools.TryGetValue(enemyName, out var enemyQueue) && enemyQueue.Count > 0)
            {
                if (enemyQueue.Count > 1)
                {
                    enemy = enemyQueue.Dequeue() as T;
                }
                else
                {
                    var dequeuedEnemy = enemyQueue.Dequeue();
                    InstantiateEnemy(enemyName, dequeuedEnemy.GetType(), prefab);
                    enemy = dequeuedEnemy as T;
                }
            }
            else
            {
                enemy = null;
                Debug.LogError("NO ENEMIES IN POOL WTF");
            }

            return enemy;
        }
        
        public Enemy PoolEnemy(EnemyName enemyName)
        {
            Enemy enemy;
            EnemyType type = EnemyData.Instance.GetEnemyConfig(enemyName).GetEnemyType();

            switch (type)
            {
                case EnemyType.Melee:
                    enemy = GetEnemy<Enemy>(enemyName, _enemyMelee);
                    break;
                default:
                    return null;
            }
            
            enemy.transform.SetParent(null);
            return enemy;
        }
        
        public void ReturnEnemy(EnemyName enemyName, Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            enemy.transform.SetParent(transform);

            if (_enemyPools != null)
            {
                _enemyPools[enemyName].Enqueue(enemy);
            }
            else
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}
