using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public List<GameObject> _skills;
    List<List<GameObject>> _skillPool;
    [SerializeField] Transform _skillParent;
    public GameObject _garlic;
    [HideInInspector] public List<int> _skillAmounts;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _skillPool = new List<List<GameObject>>();

        _skillAmounts = new List<int>();

        for (int i = 0; i < _skills.Count; i++)
        {
            if (i != 4)
            {
                SkillGenerate(i, 100);
            }
            _skillAmounts.Add(0);
        }
        _skillAmounts[0] = 1;
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
                    _skillPool[skill][i].transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, PlayerMove.instance._resolutionX), Random.Range(0, PlayerMove.instance._resolutionY), 0));
                    _skillPool[skill][i].transform.position = new Vector3(_skillPool[skill][i].transform.position.x, _skillPool[skill][i].transform.position.y, 0);
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
                yield return new WaitForSeconds(0.1f);
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
}
