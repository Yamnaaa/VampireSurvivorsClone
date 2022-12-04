using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_MagicBall : MonoBehaviour
{
    PlayerInfo PI;

    [SerializeField] float _speed;
    [SerializeField] float _skillDamage;
    int _through;

    void Awake()
    {
        PI = PlayerInfo.instance;
        _through = 3;
    }


    void OnEnable()
    {
        transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
    }

    void Update()
    {
        transform.position += transform.up * (_speed * Time.deltaTime);
        transform.GetChild(0).eulerAngles += Vector3.back * (Time.deltaTime * 720);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PI._damage * _skillDamage * 1.7f;
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
                _through--;
                if (_through <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else if (collision.CompareTag("Box"))
        {
            if (collision.TryGetComponent(out RandomBox randomBox))
            {
                _through--;
                randomBox.RandomItem();
            }
        }
    }
}
