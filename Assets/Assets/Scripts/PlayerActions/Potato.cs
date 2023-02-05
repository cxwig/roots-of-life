using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : PlayerActionBase
{
    public Potato()
    {
        m_cost = 60.0f;
    }

    public override bool CanUseActionOn(RootEnd target) { return !target.IsClaimed() && !target.AMPOTATO; }

    public override void UseActionOn(RootEnd target) 
    {
        //TODO potatofy
        target.MarkForPotatoDeath();
    }
}
