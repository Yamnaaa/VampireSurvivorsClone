using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lightning : MonoBehaviour
{
    [SerializeField] float _skillDamage;

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
            float damage = PlayerInfo.instance._damage * _skillDamage;
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
            }
        }
    }
}
