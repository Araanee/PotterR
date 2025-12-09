using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

using narrenschlag;

public class HelloWordIn3Languages : MonoBehaviour
{
    public Text text;
    public int wordID;

    private void Start()
    {
        SetWord();
    }

    //Sets the language and then updates the text mesh
    public void SetLangue(int lang) //Set the language via int because buttons dont work with enums... -.- //English > 0    //German > 1    //Italian > 2
    {
        LanguageManager.singleton.SetLanguage((Language)lang);
        SetWord();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  //Use this if you update the language after a scene reload!
    }

    //Updates the text in the text mesh
    private void SetWord()
    {
        text.text = LanguageManager.singleton.GetWord(wordID);
    }
}
