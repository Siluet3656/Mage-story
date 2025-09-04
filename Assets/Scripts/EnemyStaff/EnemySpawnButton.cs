using UnityEngine;
using Random = UnityEngine.Random;
using Data;
using Data.Enums;

namespace EnemyStaff
{
    public class EnemySpawnButton : MonoBehaviour
    {
        private Vector3 _pos;
        private Enemy _enemy;
        private EnemyMovement _enemyMovement;
        private void SpawnEnemy(EnemyName enemyName)
        {
            _pos = new Vector3(G.Player.transform.position.x + Random.Range(-5f,5f),G.Player.transform.position.y + Random.Range(-5f,5f),0f);
            _enemy = EnemyFactory.Instance.PoolEnemy(enemyName);
            _enemy.transform.position = _pos;
            _enemyMovement = _enemy.GetComponent<EnemyMovement>();
            _enemyMovement.SetSpeed((SpeedType)Random.Range(1,2));
            _enemyMovement.SetMovementAvailability(true, MovementDisableSource.None);
        }

        public void SpawnPrisoner()
        {
            SpawnEnemy(EnemyName.Prisoner);
        }
        
        public void SpawnHunter()
        {
            SpawnEnemy(EnemyName.Hunter);
        }
        
        public void SpawnPaladin()
        {
            SpawnEnemy(EnemyName.Paladin);
        }
    }
}