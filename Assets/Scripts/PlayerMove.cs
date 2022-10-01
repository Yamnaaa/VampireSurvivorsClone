using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float _inputX, _inputY;
    float _speed = 5;

    void Awake()
    {
        _inputX = 0;
        _inputY = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _inputX = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _inputX = 1;
        }
        else
        {
            _inputX = 0;
        }

        if (Input.GetKey(KeyCode.W))
        {
            _inputY = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _inputY = -1;
        }
        else
        {
            _inputY = 0;
        }

        transform.position = new Vector2(transform.position.x + _inputX * _speed * Time.deltaTime, transform.position.y + _inputY * _speed * Time.deltaTime);
    }
}
