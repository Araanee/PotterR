using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public enum PotionType
    {
        Invincibility,
        Reverse
    }

    public PotionType potionType;
    public float effectDuration = 5f; // Duration of the effect
    private int cost;

    void Start()
    {
        // Set color based on potion type
        SetPotionColor();
    }

    private void SetPotionColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            switch (potionType)
            {
                case PotionType.Invincibility:
                    renderer.material.color = Color.cyan; // Blue for invincibility
                    break;
                case PotionType.Reverse:
                    renderer.material.color = Color.magenta; // Purple for reverse
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Apply effect based on potion type
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                switch (potionType)
                {
                    case PotionType.Invincibility:
                        player.ActivateInvincibility(effectDuration);
                        Debug.Log("Invincibility Potion Consumed! Duration: " + effectDuration + "s");
                        break;
                    case PotionType.Reverse:
                        player.ActivateReverse(effectDuration);
                        Debug.Log("Reverse Potion Consumed! Duration: " + effectDuration + "s");
                        break;
                }
            }

            Debug.Log("Potion Consumed! Cost: " + cost);
            Destroy(gameObject);
        }
    }
}