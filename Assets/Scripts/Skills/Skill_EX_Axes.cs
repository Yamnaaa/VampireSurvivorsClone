using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EX_Axes : MonoBehaviour
{
    void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = Vector3.zero;
            transform.GetChild(i).localEulerAngles = Vector3.forward * (i * 24);
            transform.GetChild(i).GetChild(0).localEulerAngles = Vector3.zero;
            transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
    }

    IEnumerator Delay_Deactive()
    {
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
}
