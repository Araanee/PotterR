using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;


public class Events : MonoBehaviour
{
    private ScoreManager theScoreManager;
    private Coin Coins;

    public void Replay()
    {   
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();   
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            SceneManager.LoadScene("Level2");
        }
            
        
    }

    public void Quit()
    {
        if (PhotonNetwork.IsConnected)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach(var p in players)
                Destroy(p);

            PhotonNetwork.LeaveRoom();
            PhotonNetwork. Disconnect();
        }
        SceneManager.LoadScene("Menu2");
    }
}
