using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region юс╫ц
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
            EnemyController enemy = Managers.Resource.Instantiate("RedSlime", transform.position).GetComponent<EnemyController>();
            enemy.Init(wayPoints);
        }
    }
}
