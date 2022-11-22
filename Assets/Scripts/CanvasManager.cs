using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] Text _timeText;
    float _time;

    void Awake()
    {
        _time = 0;
    }

    void Update()
    {
        _time += Time.deltaTime;
        _timeText.text = (int)_time / 60 + ":" + ((int)_time % 60).ToString().PadLeft(2, '0');
    }
}
