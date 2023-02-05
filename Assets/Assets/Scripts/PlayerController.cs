using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Actions
    {
        MoveLeft,
        MoveRight,
        MoveDown,
        Split
    }

    [SerializeField]
    ResourcePool m_energyPool;

    Dictionary<Actions, PlayerActionBase> m_actions;

    private PlayerActionBase m_selectedAction;

    // Start is called before the first frame update
    void Start()
    {
        m_actions = new Dictionary<Actions, PlayerActionBase>();
        m_actions.Add( Actions.MoveLeft, new MoveLeft() );
        m_actions.Add( Actions.MoveRight, new MoveRight() );
        m_actions.Add( Actions.MoveDown, new MoveDown() );
        m_actions.Add( Actions.Split, new Split() );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseActionOn( RootEnd target )
    {
        if( m_energyPool == null || m_selectedAction == null 
            || (!m_selectedAction.Forced && m_energyPool.Amount < m_selectedAction.Cost)
            || !m_selectedAction.CanUseActionOn(target) ) 
        {
            //TODO can play fail SFX if an action was selected
            return;
        }

        m_selectedAction.UseActionOn(target);
        m_energyPool.ModifyResource(m_selectedAction.Cost, true);
    }
}
