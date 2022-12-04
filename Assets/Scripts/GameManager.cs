using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    EnemyPoolManager EPM;
    SkillManager SM;
    PlayerMove PM;

    [SerializeField] List<Sprite> _skillImages;
    [SerializeField] List<Sprite> _accImages;
    [SerializeField] List<Sprite> _EXSkillImages;
    [SerializeField] GameObject _boxBtn;
    [SerializeField] Button _skillBtn_1;
    [SerializeField] Button _skillBtn_2;
    [SerializeField] Button _skillBtn_3;
    [SerializeField] List<Image> _skillSlots;
    [SerializeField] List<Image> _accSlots;
    [SerializeField] GameObject _pause;
    [SerializeField] GameObject _blockPanel;
    [SerializeField] GameObject _randomBox;
    public Transform _itemParent;
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
    List<int> _btnOrders;
    [HideInInspector] public int _itemExist;
    int _itemCnt;

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
        _btnOrders = new List<int>();
        _itemCnt = 0;
        _itemExist = 0;
        for (int i = 0; i < 3; i++)
        {
            _btnOrders.Add(0);
        }
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
        _skillBtn_2.gameObject.SetActive(false);
        _skillBtn_3.gameObject.SetActive(false);
        _pause.SetActive(false);
        _blockPanel.SetActive(false);

        _IsLevelUp = false;
        _IsSkip = false;
        _IsPause = false;
    }

    void Start()
    {
        EPM = EnemyPoolManager.instance;
        SM = SkillManager.instance;
        PM = PlayerMove.instance;
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
                    if (i == 5 || SM._EXSkillAmounts[i] == 0)
                    {
                        StartCoroutine(SM.Delay_SkillActive(i, 1));
                    }
                    else
                    {
                        StartCoroutine(SM.Delay_EXSkillActive(i, 1));
                    }

                    _skillTimes[i] = 0;
                }
            }
        }

        if (_curTime > (_itemCnt + 1) * 60)
        {
            if (_itemExist < 5)
            {
                GameObject temp = Instantiate(_randomBox, _itemParent);
                temp.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, PM._resolutionX), Random.Range(0, PM._resolutionY), 0));
                temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, 0);
                _itemExist++;
            }
            _itemCnt++;
        }
    }

    public void LevelUp()
    {
        Time.timeScale = 0f;

        _IsLevelUp = true;

        _boxBtn.transform.parent.gameObject.SetActive(true);
        _boxBtn.SetActive(true);
        _skillBtn_1.gameObject.SetActive(true);
        _skillBtn_2.gameObject.SetActive(true);
        _skillBtn_3.gameObject.SetActive(true);
    }

    public void Btn_Box()
    {
        StartCoroutine(Delay_Box());

        _blockPanel.SetActive(true);
        _boxBtn.SetActive(false);
    }

    IEnumerator Delay_Box()
    {
        float deltaTime = 0;
        _IsSkip = false;

        int random1 = 0;
        int random2 = 0;
        int random3 = 0;

        while (deltaTime < 1.5f && !_IsSkip)
        {
            random1 = Random.Range(0, _skillImages.Count + _accImages.Count);
            random2 = Random.Range(0, _skillImages.Count + _accImages.Count);
            random3 = Random.Range(0, _skillImages.Count + _accImages.Count);
            while (random1 < 6 ? SM._skillAmounts[random1] >= 8 : SM._accAmounts[random1 % 6] >= 5)
            {
                random1 = Random.Range(0, _skillImages.Count + _accImages.Count);
            yield return new WaitForEndOfFrame();
            }
            while (random2 == random1 || (random2 < 6 ? SM._skillAmounts[random2] >= 8 : SM._accAmounts[random2 % 6] >= 5))
            {
                random2 = Random.Range(0, _skillImages.Count + _accImages.Count);
            yield return new WaitForEndOfFrame();
            }
            while (random3 == random1 || random3 == random2 || (random3 < 6 ? SM._skillAmounts[random3] >= 8 : SM._accAmounts[random3 % 6] >= 5))
            {
                random3 = Random.Range(0, _skillImages.Count + _accImages.Count);
            yield return new WaitForEndOfFrame();
            }
            _skillBtn_1.image.sprite = random1 < 6 ? _skillImages[random1] : _accImages[random1 % 6];
            _skillBtn_2.image.sprite = random2 < 6 ? _skillImages[random2] : _accImages[random2 % 6];
            _skillBtn_3.image.sprite = random3 < 6 ? _skillImages[random3] : _accImages[random3 % 6];
            deltaTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        bool IsEX = false;
        for (int i = 0; i < SM._skillAmounts.Count; i++)
        {
            if (SM._skillAmounts[i] == 8 && SM._accAmounts[i] > 0 && SM._EXSkillAmounts[i] < 1 && !IsEX)
            {
                random1 = i;
                _skillBtn_1.image.sprite = _EXSkillImages[i];
                IsEX = true;
            }
        }

        _btnOrders[0] = random1;
        _btnOrders[1] = random2;
        _btnOrders[2] = random3;

        _blockPanel.SetActive(false);
    }

    public void Btn_SelectSkill(int index)
    {
        int realIndex = _btnOrders[index];
        if (realIndex < 6)
        {
            if (SM._skillAmounts[realIndex] == 0)
            {
                bool IsDone = false;
                for (int i = 0; i < _skillSlots.Count; i++)
                {
                    if (_skillSlots[i].sprite == null && !IsDone)
                    {
                        _skillSlots[i].sprite = _skillImages[realIndex];
                        _skillOrders[realIndex] = i;
                        IsDone = true;
                    }
                }
            }

            if (SM._skillAmounts[realIndex] == 8 && SM._accAmounts[realIndex] > 0 && SM._EXSkillAmounts[realIndex] < 1)
            {
                for (int i = 0; i < _skillSlots[_skillOrders[realIndex]].transform.childCount; i++)
                {
                    _skillSlots[_skillOrders[realIndex]].transform.GetChild(i).gameObject.SetActive(false);
                }
                _skillSlots[_skillOrders[realIndex]].sprite = _EXSkillImages[realIndex];

                SM._EXSkillAmounts[realIndex]++;

                if (realIndex == 0 || realIndex == 3)
                {
                    _skillCools[realIndex] = 0.05f;
                }
                else if (realIndex == 2)
                {
                    SM._EXBook.SetActive(true);
                }
                else if (realIndex == 4)
                {
                    SM._garlic.SetActive(false);
                    SM._EXGarlic.SetActive(true);
                }

                _boxBtn.transform.parent.gameObject.SetActive(false);
                _skillBtn_1.gameObject.SetActive(false);
                _skillBtn_2.gameObject.SetActive(false);
                _skillBtn_3.gameObject.SetActive(false);
                _IsLevelUp = false;

                Time.timeScale = 1f;
            }
            else
            {
                _skillSlots[_skillOrders[realIndex]].transform.GetChild(SM._skillAmounts[realIndex]).GetComponent<Image>().color = Color.green;

                SM._skillAmounts[realIndex]++;

                if (realIndex == 4)
                {
                    SM._garlic.SetActive(true);
                }

                _skillTimes[realIndex] = 10;

                _boxBtn.transform.parent.gameObject.SetActive(false);
                _skillBtn_1.gameObject.SetActive(false);
                _skillBtn_2.gameObject.SetActive(false);
                _skillBtn_3.gameObject.SetActive(false);
                _IsLevelUp = false;

                Time.timeScale = 1f;
            }
        }
        else
        {
            realIndex -= 6;
            if (SM._accAmounts[realIndex] == 0)
            {
                bool IsDone = false;
                for (int i = 0; i < _accSlots.Count; i++)
                {
                    if (_accSlots[i].sprite == null && !IsDone)
                    {
                        _accSlots[i].sprite = _accImages[realIndex];
                        _accOrders[realIndex] = i;
                        IsDone = true;
                    }
                }
            }

            _accSlots[_accOrders[realIndex]].transform.GetChild(SM._accAmounts[realIndex]).GetComponent<Image>().color = Color.green;

            SM._accAmounts[realIndex]++;

            if (SM._garlic.TryGetComponent(out Skill_Garlic garlic))
            {
                garlic.SetScale();
            }

            if (SM._EXGarlic.TryGetComponent(out Skill_EX_Garlic EXGarlic))
            {
                EXGarlic.SetScale();
            }

            _boxBtn.transform.parent.gameObject.SetActive(false);
            _skillBtn_1.gameObject.SetActive(false);
            _skillBtn_2.gameObject.SetActive(false);
            _skillBtn_3.gameObject.SetActive(false);
            _IsLevelUp = false;

            Time.timeScale = 1f;
        }
    }
}
