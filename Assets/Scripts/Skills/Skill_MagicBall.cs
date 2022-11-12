using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MagicBall : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _skillDamage;

    void OnEnable()
    {
        transform.eulerAngles = Vector3.forward * Random.Range(0f, 360f);
    }

    void Update()
    {
        transform.position += transform.up * (_speed * Time.deltaTime);
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
