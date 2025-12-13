using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject priceTextObject; // Assign the GameObject here
    private int cost;

    void Start()
    {
        // Determine random cost on spawn
        cost = Random.Range(1, 31);
        
        // Update 3D Text
        if (priceTextObject != null)
        {
            TextMesh tm = priceTextObject.GetComponent<TextMesh>();
            if (tm != null)
            {
                tm.text = cost.ToString();
                tm.characterSize = 0.5f; 
                tm.fontSize = 200; 
                tm.color = Color.yellow; 
                tm.anchor = TextAnchor.MiddleCenter;
                
                // FORCE POSITION AND ROTATION
                priceTextObject.transform.localPosition = new Vector3(0, 2f, 0); // 2 units above center
                priceTextObject.transform.localRotation = Quaternion.Euler(0, 180, 0); // Face backwards (towards camera usually)
                
                Debug.Log("Potion Price set to: " + cost);
            }
            else
            {
                Debug.LogError("Potion: The object assigned to 'Price Text Object' does not have a 'TextMesh' component!");
            }
        }
        else
        {
             Debug.LogError("Potion: 'Price Text Object' is not assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Check if player has enough coins
            if (PlayerManager.numberofCoins >= cost)
            {
                // Deduct coins
                PlayerManager.numberofCoins -= cost;
                PlayerPrefs.SetInt("0", PlayerManager.numberofCoins);

                // Activate Invincibility (5 seconds)
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.ActivateInvincibility(5f);
                }

                Debug.Log("Potion Consumed! Cost: " + cost);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Not enough coins for Potion! Need: " + cost);
                // Optional: Play error sound or feedback here
            }
        }
    }
}
