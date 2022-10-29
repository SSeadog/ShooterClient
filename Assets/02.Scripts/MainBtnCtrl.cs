using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainBtnCtrl : MonoBehaviour
{
    public void StartBtnClickHandler()
    {
        SceneManager.LoadScene("scPlay");
    }
}
