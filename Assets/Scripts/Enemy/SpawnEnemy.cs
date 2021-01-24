using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    private float _timeCounter;
    private float _spawnTime;

    public float MinTime;
    public float MaxTime;

    public GameObject EnemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _spawnTime = Random.Range(MinTime, MaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        _timeCounter += Time.deltaTime;
        if (_timeCounter >= _spawnTime)
        {
            Vector3 enemyPosition = new Vector3(transform.position.x, transform.position.y, 0);
            Instantiate(EnemyPrefab, enemyPosition, transform.rotation);
            _spawnTime = Random.Range(MinTime, MaxTime);
            _timeCounter = 0f;
        }
    }
}
