using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public void Btn_Play()
    {
        SceneManager.LoadScene(1);
    }
}
