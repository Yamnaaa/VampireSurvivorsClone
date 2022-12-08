using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_Knife : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    [SerializeField] float _speed;
    [SerializeField] float _skillDamage;
    Vector3 _originScale;
    int _through;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;

        _originScale = transform.localScale;
    }

    void Start()
    {
        _through = 3;
    }

    void OnEnable()
    {
        transform.rotation = PI.transform.rotation;
        _through = 3;
        transform.localScale = _originScale * (1 + SM._accAmounts[1] * 0.1f);
    }

    void Update()
    {
        transform.position += transform.up * (_speed * (1 + SM._accAmounts[0] * 0.1f) * Time.deltaTime);
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
            SM._damages[6] += damage;
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
