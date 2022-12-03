using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    EnemyPoolManager EPM;
    SkillManager SM;

    [SerializeField] List<Sprite> _skillImages;
    [SerializeField] GameObject _boxBtn;
    [SerializeField] Button _skillBtn_1;
    [SerializeField] Button _skillBtn_2;
    [SerializeField] Button _skillBtn_3;
    [SerializeField] List<Image> _skillSlots;
    [SerializeField] List<Image> _accSlots;
    [SerializeField] GameObject _pause;
    public Text _killText;

    [HideInInspector] public float _curTime;
    float _roundTime1;
    float _roundTime2;
    float _roundTime3;
    [SerializeField] List<float> _skillCools;
    [HideInInspector] public List<float> _skillTimes;
    [HideInInspector] public List<int> _enemySpawned;
    List<int> _skillOrders;
    List<int> _accOrders;

    [HideInInspector] public bool _IsLevelUp;
    [HideInInspector] public bool _IsPause;
    bool _IsSkip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _killText.text = "0";
        _roundTime1 = 0;
        _roundTime2 = 600;
        _roundTime3 = 1200;
        _skillTimes = new List<float>();
        _skillOrders = new List<int>();
        _accOrders = new List<int>();
        for (int i = 0; i < _skillCools.Count; i++)
        {
            _skillTimes.Add(0);
            _skillOrders.Add(0);
            _accOrders.Add(0);
        }
        _skillSlots[0].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        _enemySpawned = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            _enemySpawned.Add(0);
        }

        _boxBtn.transform.parent.gameObject.SetActive(false);
        _boxBtn.SetActive(false);
        _skillBtn_1.gameObject.SetActive(false);
        _pause.SetActive(false);

        _IsLevelUp = false;
        _IsSkip = false;
        _IsPause = false;
    }

    void Start()
    {
        EPM = EnemyPoolManager.instance;
        SM = SkillManager.instance;
    }

    void Update()
    {
        TimeCheck();

        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Escape) && _IsLevelUp))
        {
            _IsSkip = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !_IsLevelUp)
        {
            if (_IsPause)
            {
                _pause.SetActive(false);
                Time.timeScale = 1f;
                _IsPause = false;
            }
            else
            {
                _pause.SetActive(true);
                Time.timeScale = 0f;
                _IsPause = true;
            }
        }
    }

    void TimeCheck()
    {
        _curTime += Time.deltaTime;
        for (int i = 0; i < _skillTimes.Count; i++)
        {
            _skillTimes[i] += Time.deltaTime;
        }
        if (_curTime > _roundTime1)
        {
            while (_enemySpawned[0] * (0.2f + 0.8f * _roundTime2 / (_roundTime2 + _curTime * 3)) < _curTime)
            {
                EPM.EnemyActive(0, 1);
            }
        }
        
        if (_curTime > _roundTime2)
        {
            while (_enemySpawned[1] * (0.4f + 1.6f * _roundTime3 / (_roundTime3 + (_curTime - _roundTime2) * 3)) < _curTime - _roundTime2)
            {
                EPM.EnemyActive(1, 1);
            }
        }
        
        if (_curTime > _roundTime3)
        {
            while (_enemySpawned[2] * (0.8f + 3.2f * 1800 / (1800 + (_curTime - _roundTime3) * 3)) < _curTime - _roundTime3)
            {
                EPM.EnemyActive(2, 1);
            }
        }
        
        for (int i = 0; i < _skillCools.Count; i++)
        {
            if (i != 4)
            {
                if (_skillTimes[i] >= _skillCools[i])
                {
                    StartCoroutine(SM.Delay_SkillActive(i, 1));

                    _skillTimes[i] = 0;
                }
            }
        }
    }

    public void LevelUp()
    {
        Time.timeScale = 0f;

        _IsLevelUp = true;

        _boxBtn.transform.parent.gameObject.SetActive(true);
        _boxBtn.SetActive(true);
        _skillBtn_1.gameObject.SetActive(true);
    }

    public void Btn_Box()
    {
        int random = Random.Range(0, _skillImages.Count);

        while (SM._skillAmounts[random] >= 8)
        {
            random = Random.Range(0, _skillImages.Count);
        }

        StartCoroutine(Delay_Box(random));

        _boxBtn.SetActive(false);
    }

    IEnumerator Delay_Box(int index)
    {
        float deltaTime = 0;
        _IsSkip = false;

        while (deltaTime < 1.5f && !_IsSkip)
        {
            _skillBtn_1.image.sprite = _skillImages[Random.Range(0, _skillImages.Count)];
            _skillBtn_2.image.sprite = _skillImages[Random.Range(0, _skillImages.Count)];
            _skillBtn_3.image.sprite = _skillImages[Random.Range(0, _skillImages.Count)];
            deltaTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (SM._skillAmounts[index] == 0)
        {
            bool IsDone = false;
            for (int i = 0; i < _skillSlots.Count; i++)
            {
                print(_skillSlots[i].sprite);
                if (_skillSlots[i].sprite == null && !IsDone)
                {
                    _skillSlots[i].sprite = _skillImages[index];
                    _skillOrders[index] = i;
                    IsDone = true;
                }
            }
        }

        _skillSlots[_skillOrders[index]].transform.GetChild(SM._skillAmounts[index]).GetComponent<Image>().color = Color.green;

        _skillBtn_1.image.sprite = _skillImages[index];
        SM._skillAmounts[index]++;

        if (index == 4)
        {
            SM._garlic.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(1);
        _boxBtn.transform.parent.gameObject.SetActive(false);
        _skillBtn_1.gameObject.SetActive(false);
        _IsLevelUp = false;

        Time.timeScale = 1f;
    }
}
