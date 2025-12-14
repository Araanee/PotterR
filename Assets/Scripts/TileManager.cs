using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject invincibilityPotionPrefab; // Blue potion
    public GameObject reversePotionPrefab; // Purple potion

    private List<GameObject> activeTiles;
    public Transform playerTransform;
    public float zspawn = 0.0f;
    public float tileLength = 286f;
    public int numberOfTiles = 6;
    public int totalNumOfTiles = 7;
    private int previousIndex;

    void Start()
    {
        activeTiles = new List<GameObject>();
        SpawnTile(0);
        for (int i = 1; i < numberOfTiles; i++)
            SpawnTile(Random.Range(1, totalNumOfTiles));
    }

    void Update()
    {
        if (playerTransform.position.z - 286 > zspawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(1, tilePrefabs.Length));
            DeleteTile();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zspawn, transform.rotation);
        activeTiles.Add(go);

        // Randomly spawn Potions (30% chance)
        if ((invincibilityPotionPrefab != null || reversePotionPrefab != null) && Random.value < 0.3f)
        {
            // Random Lane: -3, 0, or 3
            float[] lanes = new float[] { -3f, 0f, 3f };
            float randomX = lanes[Random.Range(0, lanes.Length)];

            // Random Z offset within the tile
            float randomZ = Random.Range(10f, tileLength - 10f);

            Vector3 potionPos = go.transform.position + new Vector3(randomX, 10f, randomZ);

            // Randomly choose potion type
            GameObject potionPrefab = null;
            float randomValue = Random.value;

            if (randomValue < 0.5f && invincibilityPotionPrefab != null)
            {
                potionPrefab = invincibilityPotionPrefab;
                Debug.Log("Spawning Invincibility Potion at: " + potionPos);
            }
            else if (reversePotionPrefab != null)
            {
                potionPrefab = reversePotionPrefab;
                Debug.Log("Spawning Reverse Potion at: " + potionPos);
            }
            else if (invincibilityPotionPrefab != null)
            {
                potionPrefab = invincibilityPotionPrefab;
                Debug.Log("Spawning Invincibility Potion at: " + potionPos);
            }

            if (potionPrefab != null)
            {
                GameObject potion = Instantiate(potionPrefab, potionPos, Quaternion.identity);
                potion.transform.localScale = Vector3.one * 25f;
                potion.transform.SetParent(go.transform);
            }
        }

        zspawn += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}