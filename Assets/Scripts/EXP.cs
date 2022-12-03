using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    PlayerInfo PI;

    float _speed;
    [HideInInspector] public bool _isMoved;

    void Awake()
    {
        PI = PlayerInfo.instance;
    }

    void OnEnable()
    {
        _isMoved = false;
        _speed = 0;
    }

    public void MoveToPlayer()
    {
        if (_isMoved) return;
        _isMoved = true;
        StartCoroutine(Delay_MoveToPlayer());
    }

    IEnumerator Delay_MoveToPlayer()
    {
        Transform player = PI.transform;
        Vector3 dir = (player.position - transform.position).normalized;
        while (_speed > -10f)
        {
            _speed -= Time.deltaTime * 100;
            transform.position += dir * _speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (gameObject.activeSelf)
        {
            if (_speed < 10f)
            {
                _speed += Time.deltaTime * 100;
            }
            dir = (player.position - transform.position).normalized;
            transform.position += dir * _speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
