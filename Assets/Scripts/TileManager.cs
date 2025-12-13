using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject potionPrefab; // Add this reference
    private List<GameObject> activeTiles;
    public Transform playerTransform;
    public float zspawn = 0.0f;
    public float tileLength = 286f;
    public int numberOfTiles = 6;
    public int totalNumOfTiles = 7;
    private int previousIndex;

    void Start()
    {
        //playerTransform = GameObject.FindGameObjectsWithTag("Player").transform;
        activeTiles = new List<GameObject>();
        SpawnTile(0);
        for (int i = 1; i < numberOfTiles; i++)
            SpawnTile(Random.Range(1, totalNumOfTiles));

    }

    void Update()
    {
       if(playerTransform.position.z-286  > zspawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(1,tilePrefabs.Length));
            DeleteTile();
        }
    }
    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex],transform.forward*zspawn,transform.rotation);
        activeTiles.Add(go);
        
        // Randomly spawn Potion (50% chance)
        if (potionPrefab != null && Random.value < 0.1f)
        {
            // Random Lane: -3, 0, or 3? (Assuming laneDistance is 3)
            float[] lanes = new float[] { -3f, 0f, 3f };
            float randomX = lanes[Random.Range(0, lanes.Length)];
            
            // Random Z offset within the tile (0 to tileLength)
            float randomZ = Random.Range(10f, tileLength - 10f); // keep away from edges
            
            // Increased Y from 1f to 2.5f to account for scale 10
            Vector3 potionPos = go.transform.position + new Vector3(randomX, 10f, randomZ); 
            
            GameObject potion = Instantiate(potionPrefab, potionPos, Quaternion.identity);
            potion.transform.localScale = Vector3.one * 10f; // Scale up by 5 (Adjust as needed)
            potion.transform.SetParent(go.transform); // Move with tile (parenting)
        }
        
        zspawn += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

}