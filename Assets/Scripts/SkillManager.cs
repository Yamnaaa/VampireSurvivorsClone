using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    PlayerMove PM;

    public List<GameObject> _skills;
    public List<GameObject> _EXSkills;
    List<List<GameObject>> _skillPool;
    [SerializeField] Transform _skillParent;
    public GameObject _garlic;
    public GameObject _EXGarlic;
    public GameObject _EXBook;
    [HideInInspector] public List<float> _damages;
    [HideInInspector] public List<int> _skillAmounts;
    public List<int> _accAmounts;
    [HideInInspector] public List<int> _EXSkillAmounts;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        PM = PlayerMove.instance;

        _skillPool = new List<List<GameObject>>();

        _damages = new List<float>();
        _skillAmounts = new List<int>();
        _accAmounts = new List<int>();
        _EXSkillAmounts = new List<int>();

        for (int i = 0; i < _skills.Count + _EXSkills.Count; i++)
        {
            _damages.Add(0);
        }

        for (int i = 0; i < _skills.Count; i++)
        {
            _accAmounts.Add(0);
        }

        for (int i = 0; i < _skills.Count; i++)
        {
            _skillAmounts.Add(0);
            if (i != 4)
            {
                SkillGenerate(i, 100);
            }
        }
        _skillAmounts[0] = 1;

        for (int i = 0; i < _EXSkills.Count; i++)
        {
            _EXSkillAmounts.Add(0);
            if (i != 2 || i != 4)
            {
                EXSkillGenerate(i, 100);
            }
            else
            {
                _skillPool.Add(new List<GameObject>());
                _skillPool[_skills.Count + i].Add(Instantiate(new GameObject("temp")));
            }
        }
    }

    public IEnumerator Delay_SkillActive(int skill, int amount)
    {
        int actived = 0;

        if (_skillAmounts[skill] == 0)
        {
            goto Point1;
        }

        for (int i = 0; i < _skillPool[skill].Count; i++)
        {
            if (!_skillPool[skill][i].activeSelf)
            {
                if (skill == 0)
                {
                    float random = Random.Range(-0.2f, 0.2f);
                    _skillPool[skill][i].transform.position = transform.position + transform.right * random;
                }
                else if (skill == 2)
                {
                    _skillPool[skill][i].SetActive(true);
                    actived++;
                    goto Point1;
                }
                else if (skill == 4)
                {
                    if (_garlic.activeSelf)
                    {
                        actived++;
                        goto Point1;
                    }
                    else
                    {
                        _garlic.SetActive(true);
                        actived++;
                        goto Point1;
                    }
                }
                else if (skill == 5)
                {
                    _skillPool[skill][i].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, PM._resolutionX), Random.Range(0, PM._resolutionY), 0));
                    _skillPool[skill][i].transform.position = new Vector3(_skillPool[skill][i].transform.position.x, _skillPool[skill][i].transform.position.y, 0);
                    if (_EXSkillAmounts[skill] > 0)
                    {
                        StartCoroutine(Delay_EXLightning(_skillPool[skill][i].transform));
                    }
                }
                else
                {
                    _skillPool[skill][i].transform.position = transform.position;
                }
                _skillPool[skill][i].SetActive(true);
                actived++;
                if (actived >= amount * _skillAmounts[skill])
                {
                    goto Point1;
                }
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForEndOfFrame();
        }

    Point1:

        if (actived < amount * _skillAmounts[skill] && skill != 2)
        {
            SkillGenerate(skill, amount * _skillAmounts[skill]);
            StartCoroutine(Delay_SkillActive(skill, amount * _skillAmounts[skill]));
        }
    }

    void SkillGenerate(int skill, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject temp = Instantiate(_skills[skill], _skillParent);
            temp.SetActive(false);
            while (_skillPool.Count <= skill)
            {
                _skillPool.Add(new List<GameObject>());
            }
            _skillPool[skill].Add(temp);
        }
    }

    public IEnumerator Delay_EXSkillActive(int skill, int amount)
    {
        int actived = 0;

        if (_EXSkillAmounts[skill] == 0)
        {
            goto Point1;
        }

        for (int i = 0; i < _skillPool[skill + _skills.Count].Count; i++)
        {
            if (!_skillPool[skill + _skills.Count][i].activeSelf)
            {
                if (skill == 0)
                {
                    float random = Random.Range(-0.2f, 0.2f);
                    _skillPool[skill + _skills.Count][i].transform.position = transform.position + transform.right * random;
                }
                else if (skill == 2)
                {
                    _EXBook.SetActive(true);
                    actived++;
                    goto Point1;
                }
                else if (skill == 4)
                {
                    if (_garlic.activeSelf)
                    {
                        actived++;
                        goto Point1;
                    }
                    else
                    {
                        _garlic.SetActive(true);
                        actived++;
                        goto Point1;
                    }
                }
                else if (skill == 5)
                {
                    _skillPool[skill + _skills.Count][i].SetActive(true);
                    actived++;
                    goto Point1;
                }
                else
                {
                    _skillPool[skill + _skills.Count][i].transform.position = transform.position;
                }
                _skillPool[skill + _skills.Count][i].SetActive(true);
                actived++;
                if (actived >= amount * _EXSkillAmounts[skill])
                {
                    goto Point1;
                }
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForEndOfFrame();
        }

    Point1:

        if (actived < amount * _EXSkillAmounts[skill] && skill != 2)
        {
            EXSkillGenerate(skill, amount * _EXSkillAmounts[skill]);
            StartCoroutine(Delay_EXSkillActive(skill, amount * _EXSkillAmounts[skill]));
        }
    }

    void EXSkillGenerate(int skill, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject temp = Instantiate(_EXSkills[skill], _skillParent);
            temp.SetActive(false);
            while (_skillPool.Count <= skill + _skills.Count)
            {
                _skillPool.Add(new List<GameObject>());
            }
            _skillPool[skill + _skills.Count].Add(temp);
        }
    }

    IEnumerator Delay_EXLightning(Transform transform)
    {
        yield return new WaitForSeconds(0.5f);
        bool IsDone = false;
        for (int i = 0; i < _skillPool[5 + _skills.Count].Count; i++)
        {
            if (!IsDone && !_skillPool[5 + _skills.Count][i].activeSelf)
            {
                _skillPool[5 + _skills.Count][i].transform.position = transform.position;
                _skillPool[5 + _skills.Count][i].SetActive(true);
                IsDone = true;
            }
        }
    }
}
