using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_Lightning : MonoBehaviour
{
    PlayerInfo PI;

    [SerializeField] float _skillDamage;

    void Awake()
    {
        PI = PlayerInfo.instance;
    }

    void OnEnable()
    {
        StartCoroutine(Delay_Deactivate());
    }

    IEnumerator Delay_Deactivate()
    {
        print("actived");
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PI._damage * _skillDamage * 1.7f;
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
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
