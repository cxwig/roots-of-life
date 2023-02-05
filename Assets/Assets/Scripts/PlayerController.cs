using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Actions
    {
        None,
        MoveLeft,
        MoveRight,
        MoveDown,
        Split
    }

    [SerializeField]
    ResourcePool m_energyPool;

    Dictionary<Actions, PlayerActionBase> m_actions;

    private Actions m_selectedAction;

    // Start is called before the first frame update
    void Start()
    {
        m_actions = new Dictionary<Actions, PlayerActionBase>();
        m_actions.Add( Actions.MoveLeft, new MoveLeft() );
        m_actions.Add( Actions.MoveRight, new MoveRight() );
        m_actions.Add( Actions.MoveDown, new MoveDown() );
        m_actions.Add( Actions.Split, new Split() );
        GameEvents.EnergyPoolChangedValue.AddListener( OnEnergyPoolChanged );
        GameEvents.EnergyPoolChangedPerc.AddListener(GameObject.Find("EnergyBar").GetComponent<SliderController>().UpdateProgress);
        m_energyPool.AddModifier("test", new ResourcePoolModifier(0.1f, 0.1f));
        m_energyPool.eventInvoker = () => 
        { 
            GameEvents.EnergyPoolChangedValue.Invoke(m_energyPool.Amount);
            GameEvents.EnergyPoolChangedPerc.Invoke(m_energyPool.Amount / m_energyPool.MaxAmount);
        };
        
    }

    void OnEnergyPoolChanged( float value )
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Actions SelectedAction()
    {
        return m_selectedAction;
    }

    public void SetSelectedAction(Actions action )
    {
        m_selectedAction = action;
    }

    public void UseActionOn( RootEnd target )
    {
        if(m_energyPool == null || m_selectedAction == Actions.None)
        {
            //Critical Fail
            return;
        }

        PlayerActionBase selectedAction = m_actions[m_selectedAction];

        if ( (!selectedAction.Forced && m_energyPool.Amount < selectedAction.Cost)
            || !selectedAction.CanUseActionOn(target) ) 
        {
            //TODO can play fail SFX if an action was selected
            return;
        }

        selectedAction.UseActionOn(target);
        m_energyPool.ModifyResource(selectedAction.Cost, true);
    }
}
