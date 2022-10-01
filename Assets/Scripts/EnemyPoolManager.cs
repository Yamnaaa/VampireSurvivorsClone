using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] Transform _enemyParent;
    List<GameObject> _enemyPool;

    void Awake()
    {
        _enemyPool = new List<GameObject>();
        EnemyGenerate(100);
    }

    void Update()
    {

    }

    public void EnemyActive(int amount)
    {
        int actived = 0;
        for (int i = 0; i < _enemyPool.Count; i++)
        {
            if (!_enemyPool[i].activeSelf)
            {
                _enemyPool[i].SetActive(true);
                actived++;
                if (actived >= amount)
                {
                    return;
                }
            }
        }

        EnemyGenerate(amount - actived);
        EnemyActive(amount - actived);
    }

    void EnemyGenerate(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject temp = Instantiate(_enemy, _enemyParent);
            temp.SetActive(false);
            _enemyPool.Add(temp);
        }
    }
}
