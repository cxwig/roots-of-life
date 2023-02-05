using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : MoveRoot
{
    public MoveRight()
    {
        m_direction = RootNode.Direction.Right;
        m_cost = 10.0f;
    }
}
