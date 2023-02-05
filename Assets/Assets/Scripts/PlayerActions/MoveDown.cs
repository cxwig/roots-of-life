using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MoveRoot
{
    public MoveDown()
    {
        m_direction = RootNode.Direction.Right;
        m_cost = 10.0f;
    }
}
