using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Book : MonoBehaviour
{
    [SerializeField] float _skillDamage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PlayerInfo.instance._damage * _skillDamage;
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
            }
        }
    }
}
