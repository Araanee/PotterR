using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField roomInput;
    public GameObject matchmakingPanel;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined the room successfully");
        matchmakingPanel.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    private int ready = 0;
    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                ready += 1;
            if (ready == 1)
            {
                matchmakingPanel.SetActive(false);
                PhotonNetwork.LoadLevel("Multi");
                ready += 1;
            }
        }
    }

    public void GoBack()
    {
        PhotonNetwork.LoadLevel("Menu2");
    }
}
