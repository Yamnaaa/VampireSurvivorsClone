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
            float damage = PI._damage * _skillDamage * (0.9f + SM._skillAmounts[2] * 0.1f);
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
            }
        }
    }
}
