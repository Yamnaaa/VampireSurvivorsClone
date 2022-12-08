using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    GameManager GM;
    PlayerInfo PI;
    EXPPoolManager EXPPM;

    [SerializeField] GameObject _bossBox;
    Transform _itemParent;

    [SerializeField] float _speed;
    Transform _player;
    Vector2 _dir;
    float _angle;
    public float _MaxHP;
    [HideInInspector] public float _CurHP;
    [HideInInspector] public float _damage;

    void Awake()
    {
        GM = GameManager.instance;
        PI = PlayerInfo.instance;
        EXPPM = EXPPoolManager.instance;
        _itemParent = GM._itemParent;
        _player = PI.transform;
        _damage = 5;
    }

    void OnEnable()
    {
        _CurHP = _MaxHP * (1 + GM._curTime / 300);
    }

    void Update()
    {
        _dir = (_player.transform.position - transform.position).normalized;
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * _angle);
        transform.position = new Vector2(transform.position.x + _dir.x * (_speed * Time.deltaTime), transform.position.y + _dir.y * (_speed * Time.deltaTime));

        if (_CurHP <= 0)
        {
            GM._killText.text = (int.Parse(GM._killText.text) + 1).ToString();
            EXPPM.EXPActive(transform, 1);
            if (PI._AttachedEnemies.ContainsKey(gameObject))
            {
                PI._AttachedEnemies.Remove(gameObject);
            }

            Instantiate(_bossBox, transform.position, Quaternion.identity, _itemParent);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            transform.position += (PI.transform.position - transform.position) * 2;
        }
    }
}
