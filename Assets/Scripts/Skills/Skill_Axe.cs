using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Axe : MonoBehaviour
{
    [SerializeField] float _skillDamage;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rb.velocity = Vector2.up * 3;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float damage = PlayerInfo.instance._damage * _skillDamage;
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                enemyMove._CurHP -= damage;
                gameObject.SetActive(false);
            }
        }
    }
}
