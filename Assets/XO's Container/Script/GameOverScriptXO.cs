using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement ;


public class GameOverScriptXO : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
