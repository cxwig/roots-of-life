using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    HashSet<RootNode> m_activeRoots = new HashSet<RootNode>();

    [SerializeField]
    GameObject m_leftPrefab, m_rightPrefab, m_downPrefab;

    [SerializeField]
    HexagonManager m_hexagonManager;

    [SerializeField]
    float m_rootSpawnTime = 10.0f;

    float m_curTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnInitialRoot( Vector2Int startPosition )
    {
        m_activeRoots = new HashSet<RootNode>();
        Vector3 spawnPosition = m_hexagonManager.GridToWorldPosition(startPosition);
        spawnPosition += (m_hexagonManager.GridToWorldPosition( startPosition.x, startPosition.y + 1) - spawnPosition) / 2.0f;
        spawnPosition.z = -1;
        GameObject newObj = Instantiate( m_downPrefab, spawnPosition, Quaternion.identity);
        RootNode curNode = newObj.GetComponent<RootNode>();
        curNode.startPos = startPosition;
        curNode.endPos = startPosition;
        curNode.endPos.y += 1;
        DetermineGrowthDirectionAndUpdateWeights(curNode);
        m_activeRoots.Add(curNode);
    }

    void DetermineGrowthDirectionAndUpdateWeights( RootNode node )
    {
        //TODO
        node.growthDirection = RootNode.Direction.Left;
    }

    RootNode SpawnRoot( RootNode parent, RootNode.Direction direction )
    {
        //TODO instantiate and set
        GameObject newObj = null;
        Vector3 spawnPosition;
        Vector2Int endPos = HexagonManager.INVALID_POSITION;
        GameObject prefabToSpawn = null;

        switch( direction )
        {
            case RootNode.Direction.Left:
                endPos = m_hexagonManager.GetDownLeftOf(parent.endPos);
                if( endPos == HexagonManager.INVALID_POSITION )
                {
                    return null;
                }
                prefabToSpawn = m_leftPrefab;
                break;
            case RootNode.Direction.Right:
                endPos = m_hexagonManager.GetDownRightOf(parent.endPos);
                if (endPos == HexagonManager.INVALID_POSITION)
                {
                    return null;
                }
                prefabToSpawn = m_rightPrefab;
                break;
            case RootNode.Direction.Down:
                endPos = parent.endPos;
                endPos.y += 1;
                prefabToSpawn = m_downPrefab;
                break;
        }

        if( prefabToSpawn == null )
        {
            return null;
        }

        spawnPosition = m_hexagonManager.GridToWorldPosition(parent.endPos);
        spawnPosition += (m_hexagonManager.GridToWorldPosition(endPos) - spawnPosition) / 2.0f;
        spawnPosition.z = -1;
        newObj = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        RootNode curNode = newObj.GetComponent<RootNode>();

        if ( curNode != null ) 
        {
            curNode.parent = parent;
            curNode.startPos = parent.endPos;
            curNode.endPos = endPos;
            if ( curNode.parent.direction == direction)
            {
                curNode.directionStreakCounter = curNode.parent.directionStreakCounter + 1;
            }

            //TODO if obstacle lookup collides into rck or something return null else

            DetermineGrowthDirectionAndUpdateWeights( curNode );

            return curNode;
        }

        return null;
    }

    void SpawnRoots()
    {
        HashSet<RootNode> newActiveRoots = new HashSet<RootNode> ();
        RootNode curNode = null;
        foreach (var root in m_activeRoots) 
        { 
            if( root.growthDirection == RootNode.Direction.LeftRight )
            {
                curNode = SpawnRoot( root, RootNode.Direction.Left );

                if(curNode != null )
                {
                    newActiveRoots.Add( curNode );
                }

                curNode = SpawnRoot( root, RootNode.Direction.Right );

                if (curNode != null)
                {
                    newActiveRoots.Add(curNode);
                }
            }
            else
            {
                curNode = SpawnRoot( root, root.growthDirection );

                if (curNode != null)
                {
                    newActiveRoots.Add(curNode);
                }
            }
        }
        m_activeRoots.Clear ();
        m_activeRoots = newActiveRoots;
    }
    bool spawnedFirst = false;
    // Update is called once per frame
    void Update()
    {
        if(!spawnedFirst)
        {
            spawnedFirst = true;
            SpawnInitialRoot(new Vector2Int(5, 0));
        }

        m_curTime -= Time.deltaTime;

        if( m_curTime <= 0.0f )
        {
            m_curTime = m_rootSpawnTime;
            SpawnRoots();
        }
    }
}
