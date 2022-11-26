using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;

    [HideInInspector] public Dictionary<GameObject, float> _AttachedEnemies;
    [SerializeField] Slider _HPBar;
    [SerializeField] Slider _EXPBar;
    [SerializeField] Text _LevelText;
    [HideInInspector] public float _damage = 10;
    float _MaxHP;
    float _HP;
    float _MaxEXP;
    float _EXP;
    float _level;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _MaxHP = 100;
        _HP = _MaxHP;
        _MaxEXP = 10;
        _EXP = 0;
        _level = 1;
        _LevelText.text = "레벨 " + _level;

        _AttachedEnemies = new Dictionary<GameObject, float>();
    }

    void Update()
    {
        _EXPBar.value = Mathf.Lerp(_EXPBar.value, _EXP / _MaxEXP, 0.1f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EXP"))
        {
            if (collision.TryGetComponent(out EXP exp) && !exp._isMoved) return;

            _EXP += 10;
            collision.gameObject.SetActive(false);

            if (_EXP >= _MaxEXP)
            {
                _EXP = 0;
                _MaxEXP += 100;
                LevelUp();
            }
        }
    }

    void LevelUp()
    {
        _level++;
        _LevelText.text = "레벨 " + _level;

        GameManager.instance.LevelUp();
    }
}
