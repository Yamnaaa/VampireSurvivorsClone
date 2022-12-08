using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    ResultValueManager RVM;

    [SerializeField] Text _time;
    [SerializeField] Text _level;
    [SerializeField] Text _enemy;
    [SerializeField] List<Sprite> _skillImages;
    [SerializeField] List<Image> _images;
    [SerializeField] List<Text> _skillNames;
    [SerializeField] List<Text> _skillLevels;
    [SerializeField] List<Text> _skillDamages;
    [SerializeField] List<Text> _skillTimes;
    [SerializeField] List<Text> _skillDPSs;

    void Awake()
    {
        RVM = ResultValueManager.instance;

        _time.text = (int)RVM._time / 60 + ":" + ((int)RVM._time % 60).ToString().PadLeft(2, '0');
        _level.text = RVM._level.ToString();
        _enemy.text = RVM._enemies.ToString();

        for (int i = 0; i < RVM._skillOrders.Count; i++)
        {
            _images[i].sprite = _skillImages[RVM._skillOrders[i]];
            _skillNames[i].text = _skillImages[RVM._skillOrders[i]].name;
            _skillLevels[i].text = RVM._skillAmounts[RVM._skillOrders[i]].ToString();
            if (RVM._damages[RVM._skillOrders[i]] < 1000)
            {
                _skillDamages[i].text = string.Format("{0:0.#}", RVM._damages[RVM._skillOrders[i]]);
            }
            else if (RVM._damages[RVM._skillOrders[i]] < 1000000)
            {
                _skillDamages[i].text = string.Format("{0:0.#}", RVM._damages[RVM._skillOrders[i]] / 1000) + "k";
            }
            else if (RVM._damages[RVM._skillOrders[i]] < 1000000000)
            {
                _skillDamages[i].text = string.Format("{0:0.#}", RVM._damages[RVM._skillOrders[i]] / 1000000) + "M";
            }
            else
            {
                _skillDamages[i].text = string.Format("{0:0.#}", RVM._damages[RVM._skillOrders[i]] / 1000000000) + "B";
            }
            _skillTimes[i].text = (int)(RVM._time - RVM._skillTimes[RVM._skillOrders[i]]) / 60 + ":" + ((int)(RVM._time - RVM._skillTimes[RVM._skillOrders[i]]) % 60).ToString().PadLeft(2, '0');
            if (RVM._damages[RVM._skillOrders[i]] / (RVM._time - RVM._skillTimes[RVM._skillOrders[i]]) < 1000)
            {
                _skillDPSs[i].text = string.Format("{0:0.#}", RVM._damages[RVM._skillOrders[i]] / (RVM._time - RVM._skillTimes[RVM._skillOrders[i]]));
            }
            else if (RVM._damages[RVM._skillOrders[i]] / (RVM._time - RVM._skillTimes[RVM._skillOrders[i]]) < 1000000)
            {
                _skillDPSs[i].text = string.Format("{0:0.#}", RVM._damages[RVM._skillOrders[i]] / (RVM._time - RVM._skillTimes[RVM._skillOrders[i]]) / 1000) + "k";
            }
            else if (RVM._damages[RVM._skillOrders[i]] / (RVM._time - RVM._skillTimes[RVM._skillOrders[i]]) < 1000000000)
            {
                _skillDPSs[i].text = string.Format("{0:0.#}", RVM._damages[RVM._skillOrders[i]] / (RVM._time - RVM._skillTimes[RVM._skillOrders[i]]) / 1000000) + "M";
            }
            else
            {
                _skillDPSs[i].text = string.Format("{0:0.#}", RVM._damages[RVM._skillOrders[i]] / (RVM._time - RVM._skillTimes[RVM._skillOrders[i]]) / 1000000000) + "B";
            }
        }
    }

    public void Btn_Home()
    {
        SceneManager.LoadScene(0);
    }
}
