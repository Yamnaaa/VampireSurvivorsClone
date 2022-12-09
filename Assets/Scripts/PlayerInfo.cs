using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;

    GameManager GM;
    EXPPoolManager EXPPM;
    SkillManager SM;

    [HideInInspector] public Dictionary<GameObject, float> _AttachedEnemies;
    [SerializeField] Slider _HPBar;
    [SerializeField] Slider _EXPBar;
    [SerializeField] Text _LevelText;
    [HideInInspector] public float _damage = 10;
    [HideInInspector] public float _MaxHP;
    [HideInInspector] public float _HP;
    [HideInInspector] public int _level;
    float _MaxEXP;
    float _EXP;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        GM = GameManager.instance;

        _MaxHP = 100;
        _HP = _MaxHP;
        _MaxEXP = 10;
        _EXP = 0;
        _level = 1;
        _LevelText.text = "레벨 " + _level;

        _AttachedEnemies = new Dictionary<GameObject, float>();
    }

    void Start()
    {
        EXPPM = EXPPoolManager.instance;
        SM = SkillManager.instance;
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
            else if (collision.gameObject.TryGetComponent(out BossMove bossMove))
            {
                _AttachedEnemies[collision.gameObject] += Time.deltaTime;

                if (_AttachedEnemies[collision.gameObject] >= 1)
                {
                    _HP -= bossMove._damage;
                    _AttachedEnemies[collision.gameObject] = 0;
                    _HPBar.value = _HP / _MaxHP;
                }
            }

            if (_HP <= 0)
            {
                ResultValueManager.instance.GetResultValues();
                SceneManager.LoadScene(2);
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

            _EXP += 10 * (1 + SM._accAmounts[2] * 0.08f);
            collision.gameObject.SetActive(false);

            if (_EXP >= _MaxEXP)
            {
                _EXP -= _MaxEXP;
                _MaxEXP += 50;
                LevelUp();
            }
        }
        else if (collision.gameObject.CompareTag("Chicken"))
        {
            _HP = Mathf.Clamp(_HP + 30, 0, _MaxHP);
            _HPBar.value = _HP / _MaxHP;
            Destroy(collision.gameObject);
            GM._itemExist--;
        }
        else if (collision.gameObject.CompareTag("Magnet"))
        {
            for (int i = 0; i < EXPPM._EXPParent.childCount; i++)
            {
                GameObject temp = EXPPM._EXPParent.GetChild(i).gameObject;
                if (temp.activeSelf)
                {
                    if (temp.TryGetComponent(out EXP exp))
                    {
                        exp.MoveToPlayer();
                    }
                }
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("BossBox"))
        {
            GM.LevelUp(true);

            Destroy(collision.gameObject);
        }
    }

    void LevelUp()
    {
        _level++;
        _LevelText.text = "레벨 " + _level;

        GM.LevelUp(false);
    }

    public void UpdateHP()
    {
        _HPBar.value = _HP / _MaxHP;
    }
}
