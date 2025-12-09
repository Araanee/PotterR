using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ButtonScript : MonoBehaviour
{
   
    public void Select()
    {
        transform.Find("Text").GetComponent<Text>().text = "X" ;   
        GetComponent<Button>().interactable = false ;
        int index = int.Parse(gameObject.name) ;

        GameObject.Find("Canvas").GetComponent<GameManagerXO>().matrix[index] = "X" ; 

        if( GameObject.Find("Canvas").GetComponent<GameManagerXO>().verification("X"))
        {
           GameObject.Find("Canvas").GetComponent<GameManagerXO>().ShowPanel("X") ;
        }
        else
        {
             GameObject.Find("Canvas").GetComponent<GameManagerXO>().ComputerPlay();
        }

       
    }

    
}
