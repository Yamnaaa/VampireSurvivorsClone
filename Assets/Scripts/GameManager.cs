using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] List<Sprite> _skillImages;
    [SerializeField] GameObject _boxBtn;
    [SerializeField] Image _skillImage;
    [SerializeField] List<Image> _skillSlots;
    public Text _killText;

    [HideInInspector] public float _curTime;
    float _roundTime1;
    float _roundTime2;
    float _roundTime3;
    [SerializeField] List<float> _skillCools;
    [HideInInspector] public List<float> _skillTimes;
    [HideInInspector] public List<int> _enemySpawned;

    bool _IsLevelUp;
    bool _IsSkip;
    bool _IsPause;

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
        for (int i = 0; i < _skillCools.Count; i++)
        {
            _skillTimes.Add(0);
        }
        _enemySpawned = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            _enemySpawned.Add(0);
        }

        _boxBtn.transform.parent.gameObject.SetActive(false);
        _boxBtn.SetActive(false);
        _skillImage.gameObject.SetActive(false);

        _IsLevelUp = false;
        _IsSkip = false;
        _IsPause = false;
    }

    void Update()
    {
        TimeCheck();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _IsSkip = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !_IsLevelUp)
        {
            if (_IsPause)
            {
                // UI 온오프
                Time.timeScale = 1f;
                _IsPause = false;
            }
            else
            {
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
                EnemyPoolManager.instance.EnemyActive(0, 1);
            }
        }
        
        if (_curTime > _roundTime2)
        {
            while (_enemySpawned[1] * (0.4f + 1.6f * _roundTime3 / (_roundTime3 + (_curTime - _roundTime2) * 3)) < _curTime - _roundTime2)
            {
                EnemyPoolManager.instance.EnemyActive(1, 1);
            }
        }
        
        if (_curTime > _roundTime3)
        {
            while (_enemySpawned[2] * (0.8f + 3.2f * 1800 / (1800 + (_curTime - _roundTime3) * 3)) < _curTime - _roundTime3)
            {
                EnemyPoolManager.instance.EnemyActive(2, 1);
            }
        }
        
        for (int i = 0; i < _skillCools.Count; i++)
        {
            if (i != 4)
            {
                if (_skillTimes[i] >= _skillCools[i])
                {
                    StartCoroutine(SkillManager.instance.Delay_SkillActive(i, 1));

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
        _skillImage.gameObject.SetActive(true);
    }

    public void Btn_Box()
    {
        int random = Random.Range(0, _skillImages.Count);

        while (SkillManager.instance._skillAmounts[random] >= 8)
        {
            random = Random.Range(0, _skillImages.Count);
        }

        StartCoroutine(Delay_Box(random));

        _boxBtn.SetActive(false);
    }

    IEnumerator Delay_Box(int index)
    {
        float deltaTime = 0;
        int order = 0;
        _IsSkip = false;

        while (deltaTime < 1.5f && !_IsSkip)
        {
            _skillImage.sprite = _skillImages[order%_skillImages.Count];
            order++;
            deltaTime += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (SkillManager.instance._skillAmounts[index] == 0)
        {
            bool IsDone = false;
            for (int i = 0; i < _skillSlots.Count; i++)
            {
                if (_skillSlots[i].sprite == null && !IsDone)
                {
                    _skillSlots[i].sprite = _skillImages[index];
                    IsDone = true;
                }
            }
        }
        else
        {
            //스킬 레벨 변경
        }

        _skillImage.sprite = _skillImages[index];
        SkillManager.instance._skillAmounts[index]++;

        if (index == 4)
        {
            SkillManager.instance._garlic.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(1);
        _boxBtn.transform.parent.gameObject.SetActive(false);
        _skillImage.gameObject.SetActive(false);
        _IsLevelUp = false;

        Time.timeScale = 1f;
    }
}
