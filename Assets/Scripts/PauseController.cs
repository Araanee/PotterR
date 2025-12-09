using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseController : MonoBehaviour
{
  public static bool isGamePaused = false ;
  [SerializeField] GameObject pauseMenu ;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           if (isGamePaused)
           {
               ResumeGame();
           }
           else
           {
               PauseGame();
           }

        }
    }

    public void ResumeGame()
    {
       pauseMenu.SetActive(false);
       Time.timeScale = 1f ;
       isGamePaused = false ;
    }

    public void PauseGame()
    {
       pauseMenu.SetActive(false);
       Time.timeScale = 0f ;
       isGamePaused = true ;
    }


}
