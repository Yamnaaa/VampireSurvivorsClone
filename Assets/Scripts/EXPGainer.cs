using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPGainer : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = PlayerInfo.instance.transform.position;
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
