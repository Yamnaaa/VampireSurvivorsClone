using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_Axe : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    [SerializeField] float _skillDamage;
    int _through;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;
    }

    void Start()
    {
        _through = 3;
    }

    void Update()
    {
        transform.parent.position += transform.parent.up * 5 * Time.deltaTime;
        transform.eulerAngles += Vector3.forward * 500 * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PI._damage * _skillDamage * 1.7f;
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
    }
}
