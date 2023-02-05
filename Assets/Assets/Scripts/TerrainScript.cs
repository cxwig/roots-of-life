using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    public GameObject prefab; // Drag the prefab you want to spawn in this slot in the inspector
    public int numberOfPrefabs = 100; // Number of prefabs to be spawned
    public float spawnInterval = 3f; // Time interval between spawning prefabs
    public float startY = 0f; // Starting position of the first prefab along the Y axis
    public float spawnYInterval = 1f; // Distance between each prefab along the Y axis

    private float elapsedTime = 0f;
    private int spawnedCount = 0;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnInterval && spawnedCount < numberOfPrefabs)
        {
            Vector3 spawnPosition = new Vector3(0, -(startY + (spawnedCount * spawnYInterval)), 0);
            Instantiate(prefab, spawnPosition, Quaternion.identity);
            spawnedCount++;
            elapsedTime = 0f;
        }
    }
}
