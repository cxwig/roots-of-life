using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionBase
{
    public float Cost { get { return m_cost; } }
    protected float m_cost = 99999.0f;

    public bool Forced { get { return m_forced; } }
    protected bool m_forced = false;

    public virtual bool CanUseActionOn( RootEnd target) { return false; }

    public virtual void UseActionOn(RootEnd target) { }
}
