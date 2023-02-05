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
        Split,
        Potato
    }

    [SerializeField]
    ResourcePool m_energyPool;

    float m_nutrients, m_requiredNutrients = 100.0f;
    int m_nLeaves = 2;

    Dictionary<Actions, PlayerActionBase> m_actions;

    private Actions m_selectedAction;

    private const string LEAVES_MODIFIER_NAME = "leaves";

    GameObject m_energyText;
    GameObject m_leavesText;
    GameObject m_leavesBar;

    // Start is called before the first frame update
    void Start()
    {
        //Set actions
        m_actions = new Dictionary<Actions, PlayerActionBase>();
        m_actions.Add( Actions.MoveLeft, new MoveLeft() );
        m_actions.Add( Actions.MoveRight, new MoveRight() );
        m_actions.Add( Actions.MoveDown, new MoveDown() );
        m_actions.Add( Actions.Split, new Split() );
        //m_actions.Add( Actions.Potato, new Potato() );

        //Link energy pool listeners
        GameEvents.EnergyPoolChangedValue.AddListener( OnEnergyPoolChanged );
        GameEvents.EnergyPoolChangedPerc.AddListener(GameObject.Find("EnergyBar").GetComponent<SliderController>().UpdateProgress);
        m_energyPool.AddModifier(LEAVES_MODIFIER_NAME, new ResourcePoolModifier(0.1f * (float)m_nLeaves, 0.1f));
        m_energyPool.eventInvoker = () => 
        { 
            GameEvents.EnergyPoolChangedValue.Invoke(m_energyPool.Amount);
            GameEvents.EnergyPoolChangedPerc.Invoke(m_energyPool.Amount / m_energyPool.MaxAmount);
        };

        //Link reosurce collected listener
        GameEvents.ResourceCollected.AddListener(OnResourceCollected);
    }

    void OnEnergyPoolChanged( float value )
    {
        //TODO update ability interactivity
    }

    void OnResourceCollected(ResourceType type, float amount )
    {
        switch(type)
        {
            case ResourceType.Nutrients:
                AddNutrients(amount);
                break;
            case ResourceType.Energy:
                m_energyPool.ModifyResource(amount);
                break;
            case ResourceType.Potato:
                m_energyPool.SetMaxResource(m_energyPool.MaxAmount + amount);
                break;
            case ResourceType.Fertilizer:
                m_energyPool.SetModifierDelta(LEAVES_MODIFIER_NAME, m_energyPool.GetModifierDelta(LEAVES_MODIFIER_NAME) * 1.03f);
                break;
        }
    }

    void AddNutrients(float amount) 
    {
        m_nutrients += amount;
        if( m_nutrients >= m_requiredNutrients)
        {
            int addedLeaves = (int)( m_nutrients / m_requiredNutrients );
            m_nutrients -= m_requiredNutrients * (float)addedLeaves;
            AddLeaves(addedLeaves);
        }
    }

    void AddLeaves(int amount)
    {
        //TODO check if diff
        float old = m_energyPool.GetModifierDelta(LEAVES_MODIFIER_NAME);
        float energyPerLeaf = old / (float)m_nLeaves;
        m_nLeaves += amount;
        m_energyPool.SetModifierDelta(LEAVES_MODIFIER_NAME, energyPerLeaf * m_nLeaves);
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
        m_energyPool.ModifyResource(-selectedAction.Cost, true);
    }
}
