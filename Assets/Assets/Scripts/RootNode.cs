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

    bool IsRootEnd() { return children.Count == 0; }
}
