using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    Transform _player;
    float _HPOffsetY = -40;

    void Start()
    {
        _player = PlayerInfo.instance.transform;
    }

    void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(_player.position) + Vector3.up * _HPOffsetY;
    }
}
