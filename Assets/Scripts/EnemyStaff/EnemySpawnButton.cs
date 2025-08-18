using UnityEngine;
using Random = UnityEngine.Random;
using Data.Enums;

namespace EnemyStaff
{
    public class EnemySpawnButton : MonoBehaviour
    {
        private Vector3 _pos;
        private Enemy _enemy;
        private EnemyMovement _enemyMovement;
        public void SpawnAnEnemy(GameObject enemy)
        {
            _pos = new Vector3(Random.Range(-10f,10f),Random.Range(-10f,10f),0f);
            _enemy = EnemyFactory.Instance.PoolEnemy(EnemyName.Prisoner);
            _enemy.transform.position = _pos;
            _enemyMovement = _enemy.GetComponent<EnemyMovement>();
            _enemyMovement.SetSpeed((SpeedType)Random.Range(1,2));
            _enemyMovement.SetMovementAvailability(true, MovementDisableSource.None);
        }
    }
}
