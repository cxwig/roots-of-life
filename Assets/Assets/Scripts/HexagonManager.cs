using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Hexagon
{
    public enum Interactable
    {
        None,
        Rock,
        FertilizedGround,
        EnergyCrystal
    }
    public Vector2 Position { get { return m_position; } }
    private Vector2 m_position;

    public bool travelled;
    public Interactable interactable;

    public Hexagon(Vector2 position)
    {
        m_position = position;
        interactable = Interactable.None;
        travelled = false;
    }

    public Hexagon( float x, float y )
    {
        m_position = new Vector2(x, y );
        interactable = Interactable.None;
        travelled = false;
    }

    public void MarkAsTravelled()
    {
        travelled = true;
    }
}

public class HexagonManager : MonoBehaviour
{
    public static Vector2Int INVALID_POSITION = new Vector2Int(-1, -1);
    public GameObject hexagonalTilePrefab;
    public int mapWidth = 10;
    public int mapHeight = 10;
    public float tileRadius = 1.0f;
    public float tileSpacing = 2.0f;
    private float SquareVar = Mathf.Sqrt(3);
    private List<List<Hexagon>> hexagonalTiles;

    CameraMovement m_cameraMovement;

    [SerializeField]
    List<Sprite> m_sprites;

    public Vector3 GridToWorldPosition( Vector2Int gridPosition )
    {
        return hexagonalTiles[gridPosition.y][gridPosition.x].Position;
    }

    public Vector3 GridToWorldPosition( int x, int y )
    {
        return hexagonalTiles[y][x].Position;
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

    public bool HasBeenTravelled(int x, int y )
    {
        return hexagonalTiles[y][x].travelled;
    }

    public bool HasBeenTravelled(Vector2Int pos)
    {
        return hexagonalTiles[pos.y][pos.x].travelled;
    }

    public void MarkTilesAsTravelled(HashSet<RootNode> nodes)
    {
        foreach( var node in nodes)
        {
            hexagonalTiles[node.endPos.y][node.endPos.x].MarkAsTravelled();
        }
    }

    public Vector3 GetNextPositionAlongDirection( RootNode node )
    {
        Vector3 position = GridToWorldPosition( node.endPos );
        Vector2Int gridPos = node.endPos;
        switch( node.direction ) 
        {
            case RootNode.Direction.Right:
                gridPos = GetDownRightOf(node.endPos);
                position = new Vector3(gridPos.x * tileRadius * 1.5f,
                    -(gridPos.y * tileRadius * SquareVar + (gridPos.x % 2 == 0 ? 0 : tileRadius * SquareVar / 2)));
                break;
            case RootNode.Direction.Down:
                gridPos.y += 1;
                position.y = -( gridPos.y * tileRadius * SquareVar + (gridPos.x % 2 == 0 ? 0 : tileRadius * SquareVar / 2));
                break;
            default:
                gridPos = GetDownLeftOf(node.endPos);
                position = new Vector3(gridPos.x * tileRadius * 1.5f,
                    -(gridPos.y * tileRadius * SquareVar + (gridPos.x % 2 == 0 ? 0 : tileRadius * SquareVar / 2)));
                break;
        }
        return position;
    }

    private void Start()
    {
        hexagonalTiles = new List<List<Hexagon>>();
        for (int y = 0; y < mapHeight; y++)
        {
            hexagonalTiles.Add(new List<Hexagon>());
            for (int x = 0; x < mapWidth; x++)
            {
            
                float xPos = x * tileRadius * 1.5f;
                float yPos = y * tileRadius * SquareVar + (x % 2 == 0 ? 0 : tileRadius * SquareVar / 2);
                hexagonalTiles[y].Add(new Hexagon(xPos, -yPos));
                SpawnHexTile(hexagonalTiles[y][x].Position);
            }
        }
        m_cameraMovement = FindObjectOfType<CameraMovement>();
        m_cameraMovement.bottomBound = - ( mapHeight * tileRadius * SquareVar + tileRadius * SquareVar / 2 );   
    }

    public void SpawnRow()
    {
        int y = mapHeight;
        mapHeight++;
        hexagonalTiles.Add(new List<Hexagon>());
        for ( int x = 0; x < mapWidth; x++)
        {
            float xPos = x * tileRadius * 1.5f;
            float yPos = y * tileRadius * SquareVar + (x % 2 == 0 ? 0 : tileRadius * SquareVar / 2);
            hexagonalTiles[y].Add(new Hexagon(xPos, -yPos));
            SpawnHexTile(hexagonalTiles[y][x].Position);
        }

        m_cameraMovement.bottomBound = -(mapHeight * tileRadius * SquareVar + tileRadius * SquareVar / 2);
    }

    protected void SpawnHexTile( Vector2 position )
    {
        GameObject hexagonalTile = Instantiate(hexagonalTilePrefab, position, Quaternion.identity);
        SpriteRenderer renderer = hexagonalTile.GetComponent<SpriteRenderer>();
        renderer.sprite = m_sprites[Random.Range(0, m_sprites.Count)];
        hexagonalTile.transform.localScale = new Vector3( 0.03f, 0.03f, 1.0f);
    }
}