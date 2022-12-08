using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lightning : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    Vector3 _originScale;
    [SerializeField] float _skillDamage;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;

        _originScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = _originScale * (1 + SM._accAmounts[1] * 0.1f);
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
            float damage = PI._damage * _skillDamage * (0.9f + SM._skillAmounts[5] * 0.1f) * (1 + SM._accAmounts[5] * 0.1f);
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
            }
            else if (collision.TryGetComponent(out BossMove bossMove))
            {
                bossMove._CurHP -= damage;
            }
            if (SM._EXSkillAmounts[5] == 0)
            {
                SM._damages[5] += damage;
            }
            else
            {
                SM._damages[11] += damage;
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
