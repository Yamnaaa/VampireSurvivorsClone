using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lightning : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    [SerializeField] float _skillDamage;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;
    }

    void OnEnable()
    {
        StartCoroutine(Delay_Deactivate());
    }

    IEnumerator Delay_Deactivate()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PI._damage * _skillDamage * (0.9f + SM._skillAmounts[5] * 0.1f);
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
            }
        }
    }
}
