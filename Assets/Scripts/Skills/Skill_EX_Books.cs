using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_Books : MonoBehaviour
{
    PlayerInfo PI;

    float _speed = 120;

    void Awake()
    {
        PI = PlayerInfo.instance;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    void Update()
    {
        transform.eulerAngles += Vector3.back * (Time.deltaTime * _speed);
    }

    void LateUpdate()
    {
        transform.position = PI.transform.position;
    }
}
