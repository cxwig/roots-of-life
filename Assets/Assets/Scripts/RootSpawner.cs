using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSpawner : MonoBehaviour
{
    public GameObject rootPrefab;
    // root prefabs, hexagonal prefabs(same as the hexagon spawner in the spawner script
    public GameObject hexagonalTilePrefab;

    public int numberOfTiles = 10;
    // we nned to somehow change the number of tiles with the fog of war instance
    public float tileRadius = 1.0f;
    // radius of the hexagon
    public float rootLength = 0.5f;
    // set the rool lenth by getting the transform scale of its gameobject
    public float tileSpacing = 0.1f;
    // i want to try 0.1f so its kinda on it(i dont know how this will work)
    public float angle;
    //oroginally set it to 45 degree
    private Vector2 spawnPosition;

    private GameObject currentTile;
    private GameObject previousTile;

    private void Start()
    {
        // i also think we can check and spwan all of it in uodate or lateupdate method
        angle = 45.0f;

        spawnPosition = Vector2.zero;

        currentTile = Instantiate(hexagonalTilePrefab, spawnPosition, Quaternion.identity);

        previousTile = currentTile;
        // switching
        for (int i = 1; i < numberOfTiles; i++)
        {
            // some spawn position i dont think its totally coreect
            spawnPosition = new Vector2(previousTile.transform.position.x + tileRadius * tileSpacing * Mathf.Cos(angle * Mathf.Deg2Rad),
                                        previousTile.transform.position.y + tileRadius * tileSpacing * Mathf.Sin(angle * Mathf.Deg2Rad));
            currentTile = Instantiate(hexagonalTilePrefab, spawnPosition, Quaternion.identity);
            GameObject root = Instantiate(rootPrefab, previousTile.transform.position, Quaternion.identity);
            // root placement
            root.transform.position = new Vector2(root.transform.position.x + rootLength * Mathf.Cos(angle * Mathf.Deg2Rad),
                                                  root.transform.position.y + rootLength * Mathf.Sin(angle * Mathf.Deg2Rad));
            previousTile = currentTile;
        }
    }
}
