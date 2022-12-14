using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Axe : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    [SerializeField] float _skillDamage;
    Rigidbody2D rb;
    Vector3 _originScale;
    float _random;
    int _through;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;
        rb = GetComponent<Rigidbody2D>();

        _originScale = transform.localScale;
    }

    void Start()
    {
        _through = 1;
    }

    void OnEnable()
    {
        _random = Random.Range(-5f, 5f);
        rb.velocity = (Vector2.up * 8 + Vector2.right * _random) * (1 + SM._accAmounts[0] * 0.1f);
        _through = Mathf.CeilToInt(SM._skillAmounts[1] * 0.26f);
        transform.localScale = _originScale * (1 + SM._accAmounts[1] * 0.1f);
    }

    void Update()
    {
        transform.eulerAngles += Vector3.forward * -_random * 100 * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PI._damage * _skillDamage * (0.9f + SM._skillAmounts[1] * 0.1f) * (1 + SM._accAmounts[5] * 0.1f);
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
            SM._damages[1] += damage;
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
