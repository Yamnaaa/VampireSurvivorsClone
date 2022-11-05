using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Axe : MonoBehaviour
{
    [SerializeField] float _skillDamage;
    Rigidbody2D rb;
    float _random;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _random = Random.Range(-5f, 5f);
        rb.velocity = Vector2.up * 8 + Vector2.right * _random;
    }

    void Update()
    {
        transform.eulerAngles += Vector3.forward * -_random * 100 * Time.deltaTime;
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
