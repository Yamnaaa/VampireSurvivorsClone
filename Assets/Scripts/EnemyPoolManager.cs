using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager instance;

    GameManager GM;

    [SerializeField] List<GameObject> _enemies;
    [SerializeField] GameObject _boss;
    [SerializeField] Transform _enemyParent;
    List<List<GameObject>> _enemyPools;
    int _resolutionX, _resolutionY, _offset;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        GM = GameManager.instance;

        _enemyPools = new List<List<GameObject>>();
        for (int i = 0; i < _enemies.Count; i++)
        {
            EnemyGenerate(i, 100);
        }

        _resolutionX = Screen.width;
        _resolutionY = Screen.height;
        _offset = 100;
    }

    public void EnemyActive(int enemy, int amount)
    {
        int actived = 0;
        for (int i = 0; i < _enemyPools[enemy].Count; i++)
        {
            if (!_enemyPools[enemy][i].activeSelf)
            {
                int random = Random.Range(0, 4);
                int randomX = 0;
                int randomY = 0;

                if (random == 0)
                {
                    randomX = -_offset;
                    randomY = Random.Range(-_offset, _offset + _resolutionY);
                }
                else if (random == 1)
                {
                    randomX = Random.Range(-_offset, _offset + _resolutionX);
                    randomY = _offset + _resolutionY;
                }
                else if (random == 2)
                {
                    randomX = _offset + _resolutionX;
                    randomY = Random.Range(-_offset, _offset + _resolutionY);
                }
                else if (random == 3)
                {
                    randomX = Random.Range(-_offset, _offset + _resolutionX);
                    randomY = -_offset;
                }

                _enemyPools[enemy][i].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomY, 0));
                _enemyPools[enemy][i].SetActive(true);
                GM._enemySpawned[enemy]++;
                actived++;
                if (actived >= amount)
                {
                    return;
                }
            }
        }

        if (actived < amount)
        {
            EnemyGenerate(enemy, amount - actived);
            EnemyActive(enemy, amount - actived);
        }
    }

    void EnemyGenerate(int enemy, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject temp = Instantiate(_enemies[enemy], _enemyParent);
            temp.SetActive(false);
            while (_enemyPools.Count <= enemy)
            {
                _enemyPools.Add(new List<GameObject>());
            }
            _enemyPools[enemy].Add(temp);
        }
    }

    public void BossActive()
    {
        int random = Random.Range(0, 4);
        int randomX = 0;
        int randomY = 0;

        if (random == 0)
        {
            randomX = -_offset;
            randomY = Random.Range(-_offset, _offset + _resolutionY);
        }
        else if (random == 1)
        {
            randomX = Random.Range(-_offset, _offset + _resolutionX);
            randomY = _offset + _resolutionY;
        }
        else if (random == 2)
        {
            randomX = _offset + _resolutionX;
            randomY = Random.Range(-_offset, _offset + _resolutionY);
        }
        else if (random == 3)
        {
            randomX = Random.Range(-_offset, _offset + _resolutionX);
            randomY = -_offset;
        }

        GameObject boss = Instantiate(_boss, _enemyParent);
        boss.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomY, 0));
    }
}
