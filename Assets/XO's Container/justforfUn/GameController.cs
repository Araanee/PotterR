using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;
using TMPro ;


[System.Serializable]
public class Player
{
    public Image panel ;
    public TextMeshProUGUI text ;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor ;
    public Color textColor ;
}


public class GameController : MonoBehaviour
{
    
    public Player playerX ;
    public Player playerY ;

    public PlayerColor activePlayerColor ;
    public PlayerColor inactivePlayerColor ;



    public GameObject gameOverPanel ;
    public TextMeshProUGUI gameOverText ;


    public GameObject restartButton ;


    public TextMeshProUGUI[] buttonList ;
    private string playerSide ;
    private int moveCount ;

    private void Awake()
    {
        playerSide = "X" ;
        SetPlayerColors(playerX,playerY);
        restartButton.SetActive(false);
        moveCount = 0 ;
        gameOverPanel.SetActive(false);
        SetControllerOnButtons();  
    }

    void SetControllerOnButtons()
    {
        for (int i = 0 ; i < buttonList.Length ; i ++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetController(this) ;
        }
    }

    public string GetPlayerSide()
    {
        return playerSide ; 
    }

    public void EndTurn()
    {
        moveCount ++ ;

        if ( buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide ||
            buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide ||
            buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide  ||
            

            buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide ||
            buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide ||


            buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide ||
            buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide ||
            buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide )
        {
            GameOver(playerSide) ;
        }
        else if (moveCount >= 9 )
        {
            //if (moveCount >= 9 )
            //{
              SetGameOverText("It's a Draw !");
              restartButton.SetActive(true) ; 
           // }
        }
        else 
        {
             ChangeSides();
        }
        //ChangeSides();
    }

    void GameOver(string winningPlayer)
    {
        for (int i = 0 ; i < buttonList.Length ; i ++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = false ;
        }
        if (winningPlayer == "IT's a Draw !")
        {
            SetGameOverText("It's a Draw !");
        }
        else 
        {
            SetGameOverText(playerSide + " Wins !") ;
        }
        restartButton.SetActive(true) ;
    }


    void ChangeSides()
    {
        playerSide = (playerSide == "X" ) ? "O" : "X" ; 
        if (playerSide == "X")
        {
            SetPlayerColors(playerX,playerY) ;
        }
        else 
        {
            SetPlayerColors(playerY,playerX) ;
        }
    }
  
    void SetGameOverText(string myText)
    {
        gameOverText.text = myText ; 
        gameOverPanel.SetActive(true) ;
    }


    public void RestartGame()
    {
        playerSide = "X" ;
        moveCount = 0 ;
        gameOverPanel.SetActive(false);

        for (int i = 0 ; i < buttonList.Length ; i ++)
        {
            buttonList[i].text = "" ;
        }
        SetPlayerColors(playerX,playerY) ;
        SetBoardInteractable(true) ;  
        restartButton.SetActive(false);

    }


    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0 ; i < buttonList.Length ; i ++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle ;
            
        } 
    }


    void SetPlayerColors(Player newPlayer , Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor ;
        newPlayer.text.color = activePlayerColor.textColor ;

        oldPlayer.panel.color =inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor ;
    }




}
