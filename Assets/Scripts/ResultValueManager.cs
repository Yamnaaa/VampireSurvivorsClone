using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultValueManager : MonoBehaviour
{
    public static ResultValueManager instance;

    GameManager GM;
    SkillManager SM;
    PlayerInfo PI;

    [HideInInspector] public List<float> _damages;
    [HideInInspector] public List<float> _skillTimes;
    [HideInInspector] public float _time;
    [HideInInspector] public List<int> _skillOrders;
    [HideInInspector] public List<int> _accOrders;
    [HideInInspector] public List<int> _skillAmounts;
    [HideInInspector] public List<int> _EXSkillAmounts;
    [HideInInspector] public int _level;
    [HideInInspector] public int _enemies;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        GM = GameManager.instance;
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;

        _damages = new List<float>();
        _skillTimes = new List<float>();
        for (int i = 0; i < SM._skills.Count + SM._EXSkills.Count; i++)
        {
            _skillTimes.Add(0);
        }
        _skillOrders = new List<int>();
        _accOrders = new List<int>();
        _skillAmounts = new List<int>();
        _EXSkillAmounts = new List<int>();
    }

    public void GetResultValues()
    {
        _damages = SM._damages;
        _time = GM._curTime;
        _skillOrders = GM._skillOrdersResult;
        _accOrders = GM._accOrdersResult;
        _level = PI._level;
        _enemies = int.Parse(GM._killText.text);
        _skillAmounts = SM._skillAmounts;
        _EXSkillAmounts = SM._EXSkillAmounts;
    }
}
