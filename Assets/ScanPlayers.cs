using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanPlayers : MonoBehaviour
{
    private List<GameObject> currentPlayers;
    public bool playersReady;
    
    void Start()
    {
        currentPlayers = new List<GameObject>();
        playersReady = false;
    }

    void Update()
    {
        var myPlayer = GameObject.FindWithTag("Player");
        currentPlayers.Add(myPlayer);

        if (currentPlayers.Count == 2)
            playersReady = true;
    }
}
