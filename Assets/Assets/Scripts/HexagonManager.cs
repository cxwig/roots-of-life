using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{
    public static Vector2Int INVALID_POSITION = new Vector2Int(-1, -1);
    public GameObject hexagonalTilePrefab;
    public int mapWidth = 10;
    public int mapHeight = 10;
    public float tileRadius = 1.0f;
    public float tileSpacing = 2.0f;
    private float SquareVar = Mathf.Sqrt(3);
    private Vector2[,] hexagonalTilePositions;

    public Vector3 GridToWorldPosition( Vector2Int gridPosition )
    {
        return hexagonalTilePositions[gridPosition.x, gridPosition.y];
    }

    public Vector3 GridToWorldPosition( int x, int y )
    {
        return hexagonalTilePositions[x,y];
    }

    public Vector2Int GetDownLeftOf( Vector2Int gridPosition )
    {
        return GetDownLeftOf( gridPosition.x, gridPosition.y );
    }

    public Vector2Int GetDownLeftOf( int x, int y ) 
    {
        if( x - 1 < 0 )
        {
            return INVALID_POSITION;
        }

        return new Vector2Int( x - 1, x % 2 == 0 ? y : y + 1 );
    }

    public Vector2Int GetDownRightOf(Vector2Int gridPosition)
    {
        return GetDownRightOf(gridPosition.x, gridPosition.y);
    }

    public Vector2Int GetDownRightOf(int x, int y)
    {
        if (x + 1 >= mapWidth)
        {
            return INVALID_POSITION;
        }

        return new Vector2Int(x + 1, x % 2 == 0 ? y : y + 1);
    }

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