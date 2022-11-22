using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public float _curTime;
    float _roundTime1;
    float _roundTime2;
    float _roundTime3;
    float _enemySpawnCool;
    [SerializeField] List<float> _skillCools;
    [HideInInspector] public List<float> _skillTimes;
    [HideInInspector] public List<int> _enemySpawned;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _roundTime1 = 0;
        _roundTime2 = 600;
        _roundTime3 = 1200;
        _enemySpawnCool = 1f;
        _skillTimes = new List<float>();
        for (int i = 0; i < _skillCools.Count; i++)
        {
            _skillTimes.Add(0);
        }
        _enemySpawned = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            _enemySpawned.Add(0);
        }
    }

    void Update()
    {
        TimeCheck();
    }

    void TimeCheck()
    {
        _curTime += Time.deltaTime;
        for (int i = 0; i < _skillTimes.Count; i++)
        {
            _skillTimes[i] += Time.deltaTime;
        }
        if (_curTime > _roundTime1)
        {
            while (_enemySpawned[0] * _enemySpawnCool < _curTime)
            {
                EnemyPoolManager.instance.EnemyActive(0, 1);
            }
        }
        
        if (_curTime > _roundTime2)
        {
            while (_enemySpawned[1] * _enemySpawnCool < _curTime)
            {
                EnemyPoolManager.instance.EnemyActive(1, 1);
            }
        }
        
        if (_curTime > _roundTime3)
        {
            while (_enemySpawned[2] * _enemySpawnCool < _curTime)
            {
                EnemyPoolManager.instance.EnemyActive(2, 1);
            }
        }
        
        for (int i = 0; i < _skillCools.Count; i++)
        {
            if (i != 4)
            {
                if (_skillTimes[i] >= _skillCools[i])
                {
                    StartCoroutine(SkillManager.instance.Delay_SkillActive(i, 1));

                    _skillTimes[i] = 0;
                }
            }
        }
    }
}
