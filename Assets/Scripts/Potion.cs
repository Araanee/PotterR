using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject priceTextObject; // Assign the GameObject here
    private int cost;

    void Start()
    {
        // Dynamic Cost based on Time
        // Low at start (e.g., 1-5), increases as game goes on
        float time = Time.timeSinceLevelLoad;
        
        int minCost = 1 + (int)(time / 20f); // Increases slowly (every 20s)
        int maxCost = 5 + (int)(time / 5f);  // Increases faster (every 5s)
        
        cost = Random.Range(minCost, maxCost);
        
        // Update 3D Text
        if (priceTextObject != null)
        {
            TextMesh tm = priceTextObject.GetComponent<TextMesh>();
            if (tm != null)
            {
                tm.text = cost.ToString();
                tm.characterSize = 0.1f; // Reduced from 0.5f
                tm.fontSize = 60; // Reduced from 200
                tm.color = Color.black;
                tm.anchor = TextAnchor.MiddleCenter;
                
                // FORCE POSITION AND ROTATION
                // Inside the potion (assuming pivot is bottom, 0.5f puts it roughly in middle vertically for standard shapes)
                priceTextObject.transform.localPosition = new Vector3(0, -0.5f, 0); 
                priceTextObject.transform.localRotation = Quaternion.Euler(0, 180, 0); 
                
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
