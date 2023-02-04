using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{
    public GameObject[] hexagonPrefabs;
    public float spawnTime = 1.0f;
    public float spawnX = 4.0f;
    public float hexagonSideLength = 4.0f;
 
    private float timeSincePreviousSpawn;
    private float spawnY = 0.0f;
    private int hexagonIndex;
    
    private void Update()
    {
        timeSincePreviousSpawn += Time.deltaTime;
        if (timeSincePreviousSpawn >= spawnTime)
        {
            timeSincePreviousSpawn = 0.0f;
            SpawnHexagon();
        }
    }

    private void SpawnHexagon()
    {
        hexagonIndex = Random.Range(0, hexagonPrefabs.Length);
        Instantiate(hexagonPrefabs[hexagonIndex], new Vector2(spawnX, spawnY), Quaternion.identity);
        spawnX += hexagonSideLength;
    }
}