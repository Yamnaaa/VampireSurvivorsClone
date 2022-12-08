using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Book : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    [SerializeField] float _skillDamage;

    void Start()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;
    }

    void OnEnable()
    {
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PI._damage * _skillDamage * (0.9f + SM._skillAmounts[2] * 0.1f) * (1 + SM._accAmounts[5] * 0.1f);
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
            }
            else if (collision.TryGetComponent(out BossMove bossMove))
            {
                bossMove._CurHP -= damage;
            }
            if (SM._EXSkillAmounts[2] == 0)
            {
                SM._damages[2] += damage;
            }
            else
            {
                SM._damages[8] += damage;
            }
        }
        else if (collision.CompareTag("Box"))
        {
            if (collision.TryGetComponent(out RandomBox randomBox))
            {
                randomBox.RandomItem();
            }
        }
    }
}
