using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPPoolManager : MonoBehaviour
{
    public static EXPPoolManager instance;

    [SerializeField] GameObject _EXP;
    [HideInInspector] public List<GameObject> _EXPPool;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _EXPPool = new List<GameObject>();

        EXPGenerate(100);
    }

    public void EXPActive(Transform transform, int amount)
    {
        int actived = 0;
        for (int i = 0; i < _EXPPool.Count; i++)
        {
            if (!_EXPPool[i].activeSelf)
            {
                float randomX = Random.Range(-0.2f, 0.2f);
                float randomY = Random.Range(-0.2f, 0.2f);
                _EXPPool[i].transform.position = transform.position + transform.right * randomX + transform.up * randomY;
                _EXPPool[i].SetActive(true);
                actived++;
                if (actived >= amount)
                {
                    return;
                }
            }
        }

        if (actived < amount)
        {
            EXPGenerate(amount - actived);
            EXPActive(transform, amount - actived);
        }
    }

    void EXPGenerate(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject temp = Instantiate(_EXP);
            temp.SetActive(false);
            _EXPPool.Add(temp);
        }
    }
}
