using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    HashSet<RootNode> m_activeRoots = new HashSet<RootNode>();

    [SerializeField]
    float m_rootSpawnTime = 10.0f;

    float m_curTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    RootNode.Direction DetermineGrowthDirectionAndUpdateWeights()
    {
        //TODO
        return RootNode.Direction.Down;
    }

    RootNode SpawnRoot( RootNode parent, RootNode.Direction direction )
    {
        //TODO instantiate and set
        GameObject newObj = null;
        switch( direction )
        {
            case RootNode.Direction.Left:
                break;
            case RootNode.Direction.Right:
                break;
            case RootNode.Direction.Down:
                break;
        }

        RootNode curNode = newObj.GetComponent<RootNode>();

        if ( curNode != null ) 
        {
            curNode.growthDirection = DetermineGrowthDirectionAndUpdateWeights();
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
                SpawnRoot( root, root.growthDirection );

                if (curNode != null)
                {
                    newActiveRoots.Add(curNode);
                }
            }
        }
        m_activeRoots.Clear ();
        m_activeRoots = newActiveRoots;
    }

    // Update is called once per frame
    void Update()
    {
        m_curTime -= Time.deltaTime;

        if( m_curTime <= 0.0f )
        {
            m_curTime = m_rootSpawnTime;
            SpawnRoots();
        }
    }
}
