using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    float _speed;
    [HideInInspector] public bool _isMoved;

    private void OnEnable()
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
        Transform player = PlayerInfo.instance.transform;
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
