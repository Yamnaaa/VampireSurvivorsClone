using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    GameManager GM;

    [SerializeField] GameObject _chicken;
    [SerializeField] GameObject _magnet;
    Transform _itemParent;

    void Awake()
    {
        GM = GameManager.instance;
        _itemParent = GM._itemParent;
    }

    public void RandomItem()
    {
        if (Random.Range(0, 100f) < 80)
        {
            Instantiate(_chicken, transform.position, Quaternion.identity, _itemParent);
        }
        else
        {
            Instantiate(_magnet, transform.position, Quaternion.identity, _itemParent);
        }

        Destroy(gameObject);
    }
}
