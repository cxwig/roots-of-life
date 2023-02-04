using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{
    public GameObject hexagonalTilePrefab;
    public int mapWidth = 10;
    public int mapHeight = 10;
    public float tileRadius = 1.0f;
    public float tileSpacing = 2.0f;
    private float SquareVar = Mathf.Sqrt(3);
    private Vector2[,] hexagonalTilePositions;

    private void Start()
    {
        
        hexagonalTilePositions = new Vector2[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float xPos = x * tileRadius * 1.5f;
                float yPos = y * tileRadius * SquareVar + (x % 2 == 0 ? 0 : tileRadius * SquareVar / 2);
                hexagonalTilePositions[x, y] = new Vector2(xPos, -yPos);
                GameObject hexagonalTile = Instantiate(hexagonalTilePrefab, hexagonalTilePositions[x, y], Quaternion.identity);
                hexagonalTile.transform.position = new Vector3(hexagonalTile.transform.position.x, hexagonalTile.transform.position.y, 0);
            }
        }
    }
}