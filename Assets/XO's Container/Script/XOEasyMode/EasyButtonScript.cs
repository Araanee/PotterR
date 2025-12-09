using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class EasyButtonScript : MonoBehaviour
{
    public void Select()
    {
        transform.Find("Text").GetComponent<Text>().text = "X" ;   
        GetComponent<Button>().interactable = false ;
        int index = int.Parse(gameObject.name) ;

        GameObject.Find("Canvas").GetComponent<GameManagerEasy>().matrix[index] = "X" ; 

        if( GameObject.Find("Canvas").GetComponent<GameManagerEasy>().verification("X"))
        {
           GameObject.Find("Canvas").GetComponent<GameManagerEasy>().ShowPanel("X") ;
        }
        else
        {
             GameObject.Find("Canvas").GetComponent<GameManagerEasy>().ComputerPlay();
        }

       
    }
}
