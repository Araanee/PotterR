using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu2Controller : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null ;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f; 

    [Header("Menu Load")]
    public string menuscene ;

    [Header("shop Load")]
    public string shopscene ;

    [Header("scene Load")]
    public string _Gamescene ;
    [Header("multi Load")]
    public string _Roomscene ;
    [Header("XO Load")]
    public string _XOscene ;
    [Header("XO-AI Load")]
    public string _XO_AIscene ;

    public void SoloButton()
    {
        SceneManager.LoadScene(_Gamescene);
    }
     public void MultijoueurButton()
    {
        SceneManager.LoadScene(_Roomscene);
    }

    public void XOButton()
    {
        SceneManager.LoadScene(_XOscene);
    }

    public void XO_AIButton()
    {
        SceneManager.LoadScene(_XO_AIscene);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(menuscene);
    }


    public void shopButton()
    {
        SceneManager.LoadScene(shopscene);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume ;
        volumeTextValue.text = volume.ToString("0.0");
    }


    public void VolumeApply()
    {
        PlayerPrefs.SetFloat( "masterVolume", AudioListener.volume);
    
    }


    public void RestButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume ;
            volumeSlider.value = defaultVolume ;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
    }



}
