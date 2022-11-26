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

    [HideInInspector] public float _curTime;
    float _roundTime1;
    float _roundTime2;
    float _roundTime3;
    float _enemySpawnCool;
    [SerializeField] List<float> _skillCools;
    [HideInInspector] public List<float> _skillTimes;
    [HideInInspector] public List<int> _enemySpawned;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _roundTime1 = 0;
        _roundTime2 = 600;
        _roundTime3 = 1200;
        _enemySpawnCool = 1f;
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

        _boxBtn.SetActive(false);
        _skillImage.gameObject.SetActive(false);
    }

    void Update()
    {
        TimeCheck();
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
            while (_enemySpawned[0] * _enemySpawnCool < _curTime)
            {
                EnemyPoolManager.instance.EnemyActive(0, 1);
            }
        }
        
        if (_curTime > _roundTime2)
        {
            while (_enemySpawned[1] * _enemySpawnCool < _curTime)
            {
                EnemyPoolManager.instance.EnemyActive(1, 1);
            }
        }
        
        if (_curTime > _roundTime3)
        {
            while (_enemySpawned[2] * _enemySpawnCool < _curTime)
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

        while (deltaTime < 1.5f)
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


        yield return new WaitForSecondsRealtime(1);
        _skillImage.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }
}
