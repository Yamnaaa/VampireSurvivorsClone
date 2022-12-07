using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_Books : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    Vector3 _originScale;
    float _speed = 120;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;

        _originScale = transform.localScale;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    void Update()
    {
        transform.eulerAngles += Vector3.back * (Time.deltaTime * _speed * (1 + SM._accAmounts[0] * 0.1f));
    }

    void LateUpdate()
    {
        transform.position = PI.transform.position;
    }

    public void SetScale()
    {
        transform.localScale = _originScale * (1 + SM._accAmounts[1] * 0.1f);
    }
}
