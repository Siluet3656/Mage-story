using System.Collections.Generic;
using UnityEngine;
using Data;
using Data.Enums;

namespace EnemyStaff
{
    public class EnemyFactory : MonoBehaviour
    {
        #region Singleton

        public static EnemyFactory Instance { get; private set; }
        
        #endregion
        
        [Header("Base")] 
        [SerializeField, Min(1)] private int _defaultAmountOfEnemies = 10;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _enemyMelee;
        [SerializeField] private GameObject _enemyRanged;
        [SerializeField] private GameObject _enemyCaster;
        
        private readonly Dictionary<EnemyName, Queue<Enemy>> _enemyPools = new Dictionary<EnemyName, Queue<Enemy>>();
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            
            InitializeEnemyPools();
        }

        private void InitializeEnemyPools()
        {
            _enemyPools.Add(EnemyName.None, new Queue<Enemy>());
            
            InstantiateEnemies(EnemyName.Prisoner, _enemyMelee, _defaultAmountOfEnemies);
            InstantiateEnemies(EnemyName.Hunter, _enemyRanged, _defaultAmountOfEnemies);
            InstantiateEnemies(EnemyName.Paladin, _enemyCaster, _defaultAmountOfEnemies);
        }
        
        private void InstantiateEnemies(EnemyName enemyName, GameObject prefab, int amountOfEnemies)
        {
            _enemyPools.Add(enemyName, new Queue<Enemy>());
            
            for (int i = 0; i < amountOfEnemies; i++)
            {
                InstantiateEnemy(enemyName, prefab);
            }
        }

        private void InstantiateEnemy(EnemyName enemyName, GameObject prefab)
        {
            GameObject enemyGameObject = Instantiate(prefab, transform);
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();
            enemy.Pool(enemyName);
            _enemyPools[enemyName].Enqueue(enemy);
            enemyGameObject.SetActive(false);
        }
        
        private Enemy GetEnemy(EnemyName enemyName, GameObject prefab)
        {
            if (_enemyPools == null) return null;

            Enemy enemy;

            if (_enemyPools.TryGetValue(enemyName, out var enemyQueue) && enemyQueue.Count > 0)
            {
                if (enemyQueue.Count > 1)
                {
                    enemy = enemyQueue.Dequeue();
                }
                else
                {
                    var dequeuedEnemy = enemyQueue.Dequeue();
                    InstantiateEnemy(enemyName, prefab);
                    enemy = dequeuedEnemy;
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
                    enemy = GetEnemy(enemyName, _enemyMelee);
                    break;
                case EnemyType.Range:
                    enemy = GetEnemy(enemyName, _enemyRanged);
                    break;
                case EnemyType.Caster:
                    enemy = GetEnemy(enemyName, _enemyCaster);
                    break;
                default:
                    return null;
            }
            
            enemy.transform.SetParent(null);
            enemy.gameObject.SetActive(true);
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
