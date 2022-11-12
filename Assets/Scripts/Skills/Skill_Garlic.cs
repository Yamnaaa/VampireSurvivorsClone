using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Garlic : MonoBehaviour
{
    Dictionary<GameObject, float> _AttachedEnemies;
    [SerializeField] float _skillDamage;

    void Awake()
    {
        _AttachedEnemies = new Dictionary<GameObject, float>();
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !_AttachedEnemies.ContainsKey(collision.gameObject))
        {
            _AttachedEnemies.Add(collision.gameObject, 1);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyMove enemyMove))
            {
                _AttachedEnemies[collision.gameObject] += Time.deltaTime;

                if (_AttachedEnemies[collision.gameObject] >= 1)
                {
                    float damage = PlayerInfo.instance._damage * _skillDamage;
                    enemyMove._CurHP -= damage;
                    _AttachedEnemies[collision.gameObject] = 0;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _AttachedEnemies.ContainsKey(collision.gameObject))
        {
            _AttachedEnemies.Remove(collision.gameObject);
        }
    }
}
