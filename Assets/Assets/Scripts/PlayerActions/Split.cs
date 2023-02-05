using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MoveRoot
{
    public Split()
    {
        m_direction = RootNode.Direction.LeftRight;
        m_cost = 10.0f;
    }
}
