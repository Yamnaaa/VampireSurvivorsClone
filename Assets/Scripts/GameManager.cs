using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    float _curTime;
    float _roundTime1;
    float _roundTime2;
    float _roundTime3;
    float _enemySpawnCool;
    [HideInInspector] public int _enemySpawned;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _roundTime1 = 0;
        _roundTime2 = 600;
        _roundTime3 = 1200;
        _enemySpawnCool = 0.5f;
        _enemySpawned = 0;
    }

    void Update()
    {
        TimeCheck();
    }

    void TimeCheck()
    {
        _curTime += Time.deltaTime;
        if (_curTime > _roundTime1)
        {
            while (_enemySpawned * _enemySpawnCool < _curTime)
            {
                EnemyPoolManager.instance.EnemyActive(0, 1);
            }
        }
        
        if (_curTime > _roundTime2)
        {
            while (_enemySpawned * _enemySpawnCool < _curTime)
            {
                EnemyPoolManager.instance.EnemyActive(1, 1);
            }
        }
        
        if (_curTime > _roundTime3)
        {
            while (_enemySpawned * _enemySpawnCool < _curTime)
            {
                EnemyPoolManager.instance.EnemyActive(2, 1);
            }
        }
    }
}
