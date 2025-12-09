using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BackMenuController : MonoBehaviour
{
    [Header("Menu Load")]
    public string _Menuscene ;

    public void BackButton()
    {
        SceneManager.LoadScene(_Menuscene);
    }
}
