using System.Collections.Generic;
using UnityEngine;
using Data.Enums;

namespace EnemyStaff
{
    public class EnemyProjectilesFactory : MonoBehaviour
    {
        #region Singleton

        public static EnemyProjectilesFactory Instance { get; private set; }

        #endregion
        
        [Header("Base")] 
        [SerializeField, Min(1)] private int _defaultAmountOfProjectiles = 10;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _arrow;
        
        private readonly Dictionary<EnemyProjectileName, Queue<Projectile>> _projectilePools = new Dictionary<EnemyProjectileName, Queue<Projectile>>();
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            
            InitializeProjectilePools();
        }

        private void InitializeProjectilePools()
        {
            _projectilePools.Add(EnemyProjectileName.None, new Queue<Projectile>());
            
            InstantiateProjectiles(EnemyProjectileName.Arrow, _arrow, _defaultAmountOfProjectiles);
        }
        
        private void InstantiateProjectiles(EnemyProjectileName projectileName, GameObject prefab, int amountOfProjectiles)
        {
            _projectilePools.Add(projectileName, new Queue<Projectile>());
            
            for (int i = 0; i < amountOfProjectiles; i++)
            {
                InstantiateProjectile(projectileName, prefab);
            }
        }
        
        private void InstantiateProjectile(EnemyProjectileName projectileName, GameObject prefab)
        {
            GameObject projectileGameObject = Instantiate(prefab, transform);
            Projectile projectile = projectileGameObject.GetComponent<Projectile>();
            _projectilePools[projectileName].Enqueue(projectile);
            projectileGameObject.SetActive(false);
        }
        
        private Projectile GetProjectile(EnemyProjectileName projectileName, GameObject prefab)
        {
            if (_projectilePools == null) return null;

            Projectile projectile;

            if (_projectilePools.TryGetValue(projectileName, out var projectileQueue) && projectileQueue.Count > 0)
            {
                if (projectileQueue.Count > 1)
                {
                    projectile = projectileQueue.Dequeue();
                }
                else
                {
                    var dequeuedProjectile = projectileQueue.Dequeue();
                    InstantiateProjectile(projectileName, prefab);
                    projectile = dequeuedProjectile;
                }
            }
            else
            {
                projectile = null;
                Debug.LogError($"No projectiles available in pool for {projectileName}. Consider increasing pool size.");
            }

            return projectile;
        }
        
        public Projectile PoolProjectile(EnemyProjectileName projectileName)
        {
            Projectile projectile;
            
            switch (projectileName)
            {
                case EnemyProjectileName.Arrow:
                    projectile = GetProjectile(projectileName, _arrow);
                    break;
                default:
                    return null;
            }
            
            projectile.transform.SetParent(null);
            projectile.gameObject.SetActive(true);
            return projectile;
        }
        
        public void ReturnProjectile(EnemyProjectileName enemyProjectileName, Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            projectile.transform.SetParent(transform);

            if (_projectilePools != null)
            {
                _projectilePools[enemyProjectileName].Enqueue(projectile);
            }
            else
            {
                Destroy(projectile.gameObject);
            }
        }
    }
}
