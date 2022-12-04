using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Knife : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    [SerializeField] float _speed;
    [SerializeField] float _skillDamage;
    int _through;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;
    }

    void Start()
    {
        _through = 1;
    }

    void OnEnable()
    {
        transform.rotation = PI.transform.rotation;
        _through = Mathf.CeilToInt(SM._skillAmounts[0] * 0.26f);
    }

    void Update()
    {
        transform.position += transform.up * (_speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PI._damage * _skillDamage * (0.9f + SM._skillAmounts[0] * 0.1f);
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
