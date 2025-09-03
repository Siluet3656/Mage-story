using Data.Enums;
using UnityEngine;

namespace EnemyStaff
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [SerializeField] private bool _isRandom;
        [SerializeField] private EnemyName _enemyName;
        private void Start()
        {
            if (_isRandom) _enemyName = (EnemyName)Random.Range(0, 3);

            Enemy enemy = EnemyFactory.Instance.PoolEnemy(_enemyName);
            if (enemy != null) enemy.transform.position = transform.position;
        }
    }
}
