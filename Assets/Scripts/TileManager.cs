using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
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
        zspawn += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

}