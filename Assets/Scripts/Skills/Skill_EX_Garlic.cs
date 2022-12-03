using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_Garlic : MonoBehaviour
{
    PlayerInfo PI;

    Vector3 _originScale;
    Dictionary<GameObject, float> _AttachedEnemies;
    [SerializeField] float _skillDamage;

    void Awake()
    {
        PI = PlayerInfo.instance;
        _originScale = transform.localScale;
        _AttachedEnemies = new Dictionary<GameObject, float>();
        gameObject.SetActive(false);
    }

    public void SetScale()
    {
        // 촛대 추가될때마다 or 스킬 고를때마다 호출
        // 촛대 계수 추가
        transform.localScale = _originScale * 1.35f;
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
                    float damage = PI._damage * _skillDamage * 1.7f;
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
