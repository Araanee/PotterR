using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class MenuController : MonoBehaviour
{

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null ;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f; 

    
    [Header("scene Load")]
    public string _Gamescene ;
    [Header("multi Load")]
    public string _Roomscene ;

    public void SoloButton()
    {
        SceneManager.LoadScene("Level");
    }
     public void MultijoueurButton()
    {
        PhotonNetwork.LoadLevel("Lobby");
        //SceneManager.LoadScene("Loading");
    }
    

    public void ExitButton()
    {
        Application.Quit();
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
