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
        EnergyCrystal,
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

    public Hexagon.Interactable GainResource()
    {
        if( !travelled )
        {
            return interactable;
        }
        return Interactable.None;
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
    [SerializeField] Dictionary<Hexagon.Interactable, float> rollSpawnChance;
    public List<GameObject> Interactables;
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

    public Hexagon.Interactable GetResource( Vector2Int pos )
    {
        return hexagonalTiles[pos.y][pos.x].GainResource();
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
        rollSpawnChance = new Dictionary<Hexagon.Interactable, float>();
        rollSpawnChance.Add(Hexagon.Interactable.None, 0.5f);
        rollSpawnChance.Add(Hexagon.Interactable.Rock, 0.2f);
        rollSpawnChance.Add(Hexagon.Interactable.EnergyCrystal, 0.1f);
        rollSpawnChance.Add(Hexagon.Interactable.FertilizedGround, 0.2f);

        hexagonalTiles = new List<List<Hexagon>>();
        for (int y = 0; y < mapHeight; y++)
        {
            hexagonalTiles.Add(new List<Hexagon>());
            for (int x = 0; x < mapWidth; x++)
            {
            
                float xPos = x * tileRadius * 1.5f;
                float yPos = y * tileRadius * SquareVar + (x % 2 == 0 ? 0 : tileRadius * SquareVar / 2);
                hexagonalTiles[y].Add(new Hexagon(xPos, -yPos));
                hexagonalTiles[y][x].interactable = rollInteractable();
                SpawnHexTile(hexagonalTiles[y][x].Position, hexagonalTiles[y][x].interactable);
            }
        }
        m_cameraMovement = FindObjectOfType<CameraMovement>();
        m_cameraMovement.bottomBound = -(mapHeight * tileRadius * SquareVar) + tileRadius * 5;
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
            var position = new Vector2(xPos, -yPos);
            hexagonalTiles[y].Add(new Hexagon(xPos, -yPos));
            hexagonalTiles[y][x].interactable = rollInteractable();

            SpawnHexTile(hexagonalTiles[y][x].Position, hexagonalTiles[y][x].interactable);
        }

        
        m_cameraMovement.bottomBound = -(mapHeight * tileRadius * SquareVar) + tileRadius * 5;
    }


    protected Hexagon.Interactable rollInteractable()
    {
        float randomizer = Random.Range(0.0f, 1.0f);
        foreach (var pair in rollSpawnChance)
        {
            randomizer -= pair.Value;
            if(randomizer <= 0)
            {
                return pair.Key;
            }
        }
        return Hexagon.Interactable.None;
    }
    protected void SpawnHexTile(Vector2 position, Hexagon.Interactable interact)
    {
        Vector3 interactablePos = position;
        interactablePos.z -= 1;
        switch (interact)
        {
            case Hexagon.Interactable.Rock:
                Instantiate(Interactables[0], interactablePos, Quaternion.identity);
                Debug.Log("spawning rock");
                break;
            case Hexagon.Interactable.EnergyCrystal:
                Instantiate(Interactables[1], interactablePos, Quaternion.identity);
                Debug.Log("spawning crystal");
                break;
            case Hexagon.Interactable.FertilizedGround:
                Instantiate(Interactables[2], interactablePos, Quaternion.identity);
                break;
        }
        GameObject hexagonalTile = Instantiate(hexagonalTilePrefab, position, Quaternion.identity);
        SpriteRenderer renderer = hexagonalTile.GetComponent<SpriteRenderer>();
        renderer.sprite = m_sprites[Random.Range(0, m_sprites.Count)];
        hexagonalTile.transform.localScale = new Vector3( 0.03f, 0.03f, 1.0f);
    }
}