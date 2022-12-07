using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Books : MonoBehaviour
{
    SkillManager SM;
    PlayerInfo PI;

    List<Transform> _books;
    Vector3 _originScale;
    int _skillAmount;
    float _speed = 120;
    float _skillTime = 3;
    bool _isFirst;

    void Awake()
    {
        _isFirst = true;
        SM = SkillManager.instance;
        PI = PlayerInfo.instance;

        _originScale = transform.localScale;
    }

    void OnEnable()
    {
        if (_isFirst)
        {
            _isFirst = false;
            return;
        }

        transform.localScale = _originScale * (1 + SM._accAmounts[1] * 0.1f);

        _books = new List<Transform>();
        GameObject pivot = transform.GetChild(0).gameObject;
        _skillAmount = SM._skillAmounts[2];
        for (int i = 0; i < _skillAmount; i++)
        {
            GameObject temp = Instantiate(pivot, transform);
            temp.transform.eulerAngles = Vector3.forward * (360 / _skillAmount * i);
            temp.transform.GetChild(0).transform.localScale = Vector3.zero;
            _books.Add(temp.transform.GetChild(0));
        }
        Destroy(pivot);
        StartCoroutine(ScaleUp());
        StartCoroutine(DisableSkill());
    }

    void Update()
    {
        transform.position = PI.transform.position;
        transform.eulerAngles += Vector3.back * (Time.deltaTime * _speed * (1 + SM._accAmounts[0] * 0.1f));
        for (int i = 0; i < _books.Count; i++)
        {
            _books[i].eulerAngles = Vector3.zero;
        }
    }

    IEnumerator ScaleUp()
    {
        while (_books[0].transform.localScale.z < 1)
        {
            for (int i = 0; i < _books.Count; i++)
            {
                _books[i].transform.localScale += new Vector3(0.3f, 0.4f, 1) * (Time.deltaTime * 2);
            }
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < _books.Count; i++)
        {
            _books[i].transform.localScale = new Vector3(0.3f, 0.4f, 1);
        }
    }

    IEnumerator DisableSkill()
    {
        yield return new WaitForSeconds(_skillTime);
        while (_books[0].transform.localScale.z > 0)
        {
            for (int i = 0; i < _books.Count; i++)
            {
                _books[i].transform.localScale -= new Vector3(0.3f, 0.4f, 1) * (Time.deltaTime * 2);
            }
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }
}
