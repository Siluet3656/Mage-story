using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private Vector3 pos;
    private Enemy enemyset;
    public void SpawnAnEnemy(GameObject enemy)
    {
        pos = new Vector3(Random.Range(-10f,10f),Random.Range(-10f,10f),0f); 
        enemyset = Instantiate(enemy, pos, quaternion.identity).GetComponent<Enemy>();
        enemyset.SetSpeed(SpeedTypeData.GetDataByType((SpeedType)Random.Range(0,2)));
        enemyset.ShufflePoints();
    }
}
