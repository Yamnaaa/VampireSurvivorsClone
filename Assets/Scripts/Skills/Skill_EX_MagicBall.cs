using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_MagicBall : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    Vector3 _originScale;
    [SerializeField] float _speed;
    [SerializeField] float _skillDamage;
    int _through;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;

        _originScale = transform.localScale;
        _through = 3;
    }


    void OnEnable()
    {
        transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
        transform.localScale = _originScale * (1 + SM._accAmounts[1] * 0.1f);
    }

    void Update()
    {
        transform.position += transform.up * (_speed * (1 + SM._accAmounts[0] * 0.1f) * Time.deltaTime);
        transform.GetChild(0).eulerAngles += Vector3.back * (Time.deltaTime * 720);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PI._damage * _skillDamage * 1.7f * (1 + SM._accAmounts[5] * 0.1f);
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
                _through--;
                if (_through <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
            else if (collision.TryGetComponent(out BossMove bossMove))
            {
                bossMove._CurHP -= damage;
                _through--;
                if (_through <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
            SM._damages[9] += damage;
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
