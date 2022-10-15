using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Knife : MonoBehaviour
{
    [SerializeField] float _speed;

    void OnEnable()
    {
        transform.rotation = PlayerInfo.instance.transform.rotation;
    }

    void Update()
    {
        transform.position += transform.up * (_speed * Time.deltaTime);
    }
}
