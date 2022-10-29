using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;

    [HideInInspector] public Dictionary<GameObject, float> _AttachedEnemies;
    [SerializeField] Slider _HPBar;
    [HideInInspector] public float _damage = 10;
    float _MaxHP = 100;
    float _HP;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _HP = _MaxHP;

        _AttachedEnemies = new Dictionary<GameObject, float>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_AttachedEnemies.ContainsKey(collision.gameObject))
        {
            _AttachedEnemies.Add(collision.gameObject, 1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent(out EnemyMove enemyMove))
            {
                _AttachedEnemies[collision.gameObject] += Time.deltaTime;

                if (_AttachedEnemies[collision.gameObject] >= 1)
                {
                    _HP -= enemyMove._damage;
                    _AttachedEnemies[collision.gameObject] = 0;
                    _HPBar.value = _HP / _MaxHP;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _AttachedEnemies.ContainsKey(collision.gameObject))
        {
            _AttachedEnemies.Remove(collision.gameObject);
        }
    }
}
