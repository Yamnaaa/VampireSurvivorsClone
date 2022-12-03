using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    [SerializeField] List<BoxCollider2D> _walls;
    [SerializeField] float _speed;
    float _inputX, _inputY;
    [HideInInspector] public int _resolutionX, _resolutionY, _offset;
    GameManager GM;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        GM = GameManager.instance;

        _resolutionX = 0;
        _resolutionY = 0;
        _inputX = 0;
        _inputY = 0;

        SetWall();
    }

    void Update()
    {
        if (_resolutionX != Screen.width || _resolutionY != Screen.height)
        {
            SetWall();
        }

        if (GM._IsLevelUp || GM._IsPause) return;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _inputX = -1;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _inputX = 1;
        }
        else
        {
            _inputX = 0;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _inputY = 1;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _inputY = -1;
        }
        else
        {
            _inputY = 0;
        }

        Vector2 dir = new Vector2(_inputX, _inputY).normalized;
        float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;

        if (_inputX != 0 || _inputY != 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        transform.position = new Vector2(transform.position.x + _inputX * _speed * Time.deltaTime, transform.position.y + _inputY * _speed * Time.deltaTime);

        Camera.main.transform.position = transform.position - Vector3.forward * 10;
        _walls[0].transform.parent.position = transform.position;
    }

    void SetWall()
    {
        _resolutionX = Screen.width;
        _resolutionY = Screen.height;
        _offset = 500;

        _walls[0].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(-_offset, _resolutionY * 0.5f, 0));
        _walls[1].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_resolutionX * 0.5f, _offset + _resolutionY, 0));
        _walls[2].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_offset + _resolutionX, _resolutionY * 0.5f, 0));
        _walls[3].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_resolutionX * 0.5f, -_offset, 0));

        _walls[0].size = new Vector2(1, _resolutionY * 0.5f);
        _walls[1].size = new Vector2(_resolutionX * 0.5f, 1);
        _walls[2].size = new Vector2(1, _resolutionY * 0.5f);
        _walls[3].size = new Vector2(_resolutionX * 0.5f, 1);
    }
}
