using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Down,
        LeftRight
    }

    public RootNode parent = null;
    public List<RootNode> children = new List<RootNode>();
    //hexagon originTile;

    public Direction direction = Direction.Left;

    public Direction growthDirection = Direction.Right;

    public bool IsRootEnd() { return children.Count == 0; } //TODO evaluate if this function is needed/valid/accurate

    public uint directionStreakCounter = 0;
    public uint NaturalSplitLockout { get { return m_naturalSplitLockout; } protected set { m_naturalSplitLockout = value; } }
    protected uint m_naturalSplitLockout = 0;

    public void DecrementNaturalSplitLockout()
    {
        if( m_naturalSplitLockout > 0 )
        {
            m_naturalSplitLockout--;
        }
    }

    public Vector2Int startPos, endPos;
}
