using Data.Enums;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyStaff
{
    public class EnemySpawnButton : MonoBehaviour
    {
        private Vector3 _pos;
        private Enemy _enemy;
        public void SpawnAnEnemy(GameObject enemy)
        {
            _pos = new Vector3(Random.Range(-10f,10f),Random.Range(-10f,10f),0f); 
            _enemy = Instantiate(enemy, _pos, quaternion.identity).GetComponent<Enemy>();
            _enemy.SetSpeed((SpeedType)Random.Range(1,2));
            _enemy.ShufflePatrolPoints();
        }
    }
}
