using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region юс╫ц
    public EnemyController enemyPrefab;
    public Transform[] wayPoints;
    #endregion

    #region Data
    private float _spawnTime = 2.0f;
    #endregion

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while(true)
        {
            yield return new WaitForSeconds(_spawnTime);
            EnemyController enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.Init(wayPoints);
        }
    }
}
