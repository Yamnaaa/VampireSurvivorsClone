using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] List<BoxCollider2D> _walls;
    [SerializeField] float _speed;
    float _inputX, _inputY;
    int _resolutionX, _resolutionY, _offset;

    void Awake()
    {
        _inputX = 0;
        _inputY = 0;

        SetWall();
    }

    void Update()
    {
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

        transform.position = new Vector2(transform.position.x + _inputX * _speed * Time.deltaTime, transform.position.y + _inputY * _speed * Time.deltaTime);
    }

    void SetWall()
    {
        _resolutionX = Screen.width;
        _resolutionY = Screen.height;
        _offset = 200;

        _walls[0].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(-_offset, _resolutionY * 0.5f, 0));
        _walls[1].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_resolutionX * 0.5f, _offset + _resolutionY, 0));
        _walls[2].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_offset + _resolutionX, _resolutionY * 0.5f, 0));
        _walls[3].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(_resolutionX * 0.5f, -_offset, 0));

        _walls[0].size = new Vector2(1, _resolutionY * 0.02f);
        _walls[1].size = new Vector2(_resolutionX * 0.02f, 1);
        _walls[2].size = new Vector2(1, _resolutionY * 0.02f);
        _walls[3].size = new Vector2(_resolutionX * 0.02f, 1);
    }
}