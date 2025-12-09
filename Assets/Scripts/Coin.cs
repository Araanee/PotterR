using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
     public bool coinsIncreasing;
    // Start is called before the first frame update
    void Start()
    {
        if ( PlayerPrefs.HasKey("0"))
        {
            PlayerManager.numberofCoins=PlayerPrefs.GetInt("0");
        }
    }

    // Update is called once per frame
    void Update()
    {
       transform.Rotate(0,0,50*Time.deltaTime); 

    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            PlayerManager.numberofCoins+=1; 
            Destroy(gameObject);
            PlayerPrefs.SetInt("0",PlayerManager.numberofCoins);
            
        }
    }
    
}
