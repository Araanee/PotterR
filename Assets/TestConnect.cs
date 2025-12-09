using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //MasterManager.DebugConsole.AddText("Connected to Photon.",this);
        Debug.Log("My nickname is " + PhotonNetwork.LocalPlayer.NickName,this);
        if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Failed to connect to photon:" + cause.ToString(),this);
    }

    public override void OnJoinedLobby()
    {
        print("Joined lobby");
    }

}