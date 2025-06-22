using Data;
using Data.Enums;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private Vector3 _pos;
    private Enemy _enemyset;
    public void SpawnAnEnemy(GameObject enemy)
    {
        _pos = new Vector3(Random.Range(-10f,10f),Random.Range(-10f,10f),0f); 
        _enemyset = Instantiate(enemy, _pos, quaternion.identity).GetComponent<Enemy>();
        _enemyset.SetSpeed(SpeedData.GetDataByType((SpeedType)Random.Range(0,2)));
        _enemyset.ShufflePoints();
    }
}
