using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Garlic : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    Vector3 _originScale;
    Dictionary<GameObject, float> _AttachedEnemies;
    [SerializeField] float _skillDamage;

    void Awake()
    {
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;
        _originScale = transform.localScale;
        _AttachedEnemies = new Dictionary<GameObject, float>();
        gameObject.SetActive(false);
    }

    public void SetScale()
    {
        transform.localScale = _originScale * (0.95f + SM._skillAmounts[4] * 0.05f) * (1 + SM._accAmounts[1] * 0.1f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !_AttachedEnemies.ContainsKey(collision.gameObject))
        {
            _AttachedEnemies.Add(collision.gameObject, 1);
        }
        else if (collision.CompareTag("Box"))
        {
            if (collision.TryGetComponent(out RandomBox randomBox))
            {
                randomBox.RandomItem();
            }
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
                    float damage = PI._damage * _skillDamage * (0.9f + SM._skillAmounts[4] * 0.1f) * (1 + SM._accAmounts[5] * 0.1f);
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
