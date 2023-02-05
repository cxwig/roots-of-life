using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    HashSet<RootNode> m_activeRoots = new HashSet<RootNode>();

    Dictionary<RootNode, RootEnd> m_activeRootEnds;

    [SerializeField]
    GameObject m_leftPrefab, m_rightPrefab, m_downPrefab;

    [SerializeField]
    GameObject m_rootEndPrefab;

    [SerializeField]
    HexagonManager m_hexagonManager;

    [SerializeField]
    PlayerController m_playerController;

    float RootSpawnTime { get { return m_rootSpawnTime; } set { m_rootSpawnTime = value; } }

    [SerializeField]
    float m_rootSpawnTime = 10.0f;

    float m_curTime = 0.0f;

    bool m_transitionLocked = false;
    bool spawnedFirst = false;


    public float GetLowest()
    {
        return Lowest;
    }
    private float Lowest = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public RootEnd CreateNewRootEnd( RootNode endNode )
    {
        GameObject obj = Instantiate( m_rootEndPrefab, m_hexagonManager.GridToWorldPosition(endNode.endPos), Quaternion.identity );
        RootEnd rootEnd = obj.GetComponent<RootEnd>();
        rootEnd.playerController = m_playerController;
        return rootEnd;
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

        m_activeRootEnds = new Dictionary<RootNode, RootEnd>();
        RootEnd rootEnd = CreateNewRootEnd(curNode);
        m_activeRootEnds.Add(curNode, rootEnd);
    }

    void DetermineGrowthDirectionAndUpdateWeights( RootNode node )
    {
        //TODO
        switch (Random.Range(0, 4) )
        {
            case 0:
                node.growthDirection = RootNode.Direction.Left;
                break;
            case 1:
                node.growthDirection = RootNode.Direction.Right;
                break;
            case 2:
                node.growthDirection = RootNode.Direction.Down;
                break;
            default:
                node.growthDirection = RootNode.Direction.LeftRight;
                break;
        }
    }

    RootNode SpawnRoot( RootNode parent, RootNode.Direction direction )
    {
        GameObject newObj = null;
        Vector3 spawnPosition;
        Vector2Int endPos = HexagonManager.INVALID_POSITION;
        GameObject prefabToSpawn = null;

        switch( direction )
        {
            case RootNode.Direction.Left:
                endPos = m_hexagonManager.GetDownLeftOf(parent.endPos);
                if (endPos != HexagonManager.INVALID_POSITION)
                {
                    prefabToSpawn = m_leftPrefab;
                }
                break;
            case RootNode.Direction.Right:
                endPos = m_hexagonManager.GetDownRightOf(parent.endPos);
                if (endPos != HexagonManager.INVALID_POSITION)
                {
                    prefabToSpawn = m_rightPrefab;
                }
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
            parent.children.Add( curNode );

            if(endPos.y >= m_hexagonManager.mapHeight - 3)
            {
                m_hexagonManager.SpawnRow();
            }

            if(Lowest > spawnPosition.y)
            {
                Lowest = spawnPosition.y;
            }

            return curNode;
        }

        return null;
    }

    void SpawnRoots()
    {
        HashSet<RootNode> newActiveRoots = new HashSet<RootNode> ();
        HashSet<Vector2Int> mergedRootCheck = new HashSet<Vector2Int>();
        RootNode curNode = null;
        foreach (var root in m_activeRoots) 
        {
            bool oneSucceeded = false;
            if ( root.growthDirection == RootNode.Direction.LeftRight )
            {
                curNode = SpawnRoot( root, RootNode.Direction.Left );

                if(curNode != null )
                {
                    if ( !m_hexagonManager.HasBeenTravelled(curNode.endPos) && mergedRootCheck.Add(curNode.endPos))
                    {
                        newActiveRoots.Add(curNode);
                    }
                    else
                    {
                        m_activeRootEnds[root].MarkForDelete();
                    }
                    oneSucceeded = true;
                }


                curNode = SpawnRoot( root, RootNode.Direction.Right );

                if (curNode != null)
                {
                    if (!m_hexagonManager.HasBeenTravelled(curNode.endPos) && mergedRootCheck.Add(curNode.endPos))
                    {
                        newActiveRoots.Add(curNode);
                    }
                    else
                    {
                        m_activeRootEnds[root].MarkForDelete();
                    }
                    oneSucceeded = true;
                }
            }
            else
            {
                curNode = SpawnRoot( root, root.growthDirection );

                if (curNode != null)
                {
                    if (!m_hexagonManager.HasBeenTravelled(curNode.endPos) && mergedRootCheck.Add(curNode.endPos))
                    {
                        newActiveRoots.Add(curNode);
                    }
                    else
                    {
                        m_activeRootEnds[root].MarkForDelete();
                    }
                    oneSucceeded = true;
                }
            }

            if (!oneSucceeded)
            {
                m_activeRootEnds[root].MarkForDelete();
            }
        }
        m_activeRoots.Clear();
        m_activeRoots = newActiveRoots;
        m_hexagonManager.MarkTilesAsTravelled(m_activeRoots);
    }

    void UpdateAndTransferActiveRootEnds()
    {
        Dictionary<RootNode, RootEnd> newActiveRootEnds = new Dictionary<RootNode, RootEnd>();

        foreach( var nodeEndPair in m_activeRootEnds )
        {
            if (nodeEndPair.Key.children.Count > 0)
            {
                foreach (var child in nodeEndPair.Key.children)
                {
                    if (nodeEndPair.Value.IsClaimed())
                    {
                        RootEnd rootEnd = CreateNewRootEnd(nodeEndPair.Key);
                        rootEnd.BeginTransition(m_hexagonManager.GridToWorldPosition(child.endPos), child);
                        newActiveRootEnds.Add(child, rootEnd);
                    }
                    else
                    {
                        newActiveRootEnds.Add(child, nodeEndPair.Value);
                        nodeEndPair.Value.BeginTransition(m_hexagonManager.GridToWorldPosition(child.endPos), child);
                    }
                }
            }
            else
            {
                //No children means hit edge or rock, begin transition to clean up
                nodeEndPair.Value.BeginTransition(m_hexagonManager.GetNextPositionAlongDirection(nodeEndPair.Key), null);

            }
        }

        m_activeRootEnds = newActiveRootEnds;
    }
    
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
            m_transitionLocked = true;
            SpawnRoots();
            UpdateAndTransferActiveRootEnds();
        }
    }
}
