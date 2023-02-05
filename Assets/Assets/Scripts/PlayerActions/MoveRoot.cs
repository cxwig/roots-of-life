using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoot : PlayerActionBase
{
    protected RootNode.Direction m_direction = RootNode.Direction.Left;

    public override bool CanUseActionOn(RootEnd target)
    {
        return target.GetRootGrowthDirection() != m_direction;
    }

    public override void UseActionOn(RootEnd target)
    {
        target.ChangeDirection( m_direction );
    }
}
