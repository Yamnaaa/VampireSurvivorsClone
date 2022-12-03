using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPGainer : MonoBehaviour
{
    PlayerInfo PI;

    void Awake()
    {
        PI = PlayerInfo.instance;
    }

    void LateUpdate()
    {
        transform.position = PI.transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EXP"))
        {
            if (collision.TryGetComponent(out EXP exp))
            {
                exp.MoveToPlayer();
            }
        }
    }
}
