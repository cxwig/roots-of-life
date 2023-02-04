using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolModifier
{
    public float Delta { get { return m_delta; } set { m_delta = value; } }

    float m_delta = 0.0f;
    float m_tickRate = 0.2f;
    float m_curTime = 0.0f;

    public ResourcePoolModifier( float delta, float tickRate = 0.2f )
    {
        m_delta = delta;
        m_tickRate = tickRate;
    }

    public float UpdateAndGetFrameModifier( float timeDelta )
    {
        m_curTime -= timeDelta;

        if( m_curTime < 0.0f )
        {
            m_curTime = m_tickRate;
            return m_delta;
        }

        return 0.0f;
    }
}

public class ResourcePool : MonoBehaviour
{
    [SerializeField]
    float m_initialAmount = 0.0f;

    float Amount { get { return m_amount; } }
    float m_amount;

    //Pool limits
    [SerializeField]
    float m_initialMinAmount = 0.0f;
    [SerializeField]
    float m_initialMaxAmount = 100.0f;

    float MinAmount { get { return m_minAmount; } }
    float MaxAmount { get { return m_maxAmount; } }

    float m_minAmount;
    float m_maxAmount;

    //Pool modifiers
    private Dictionary<string, ResourcePoolModifier> m_resourcePoolModifiers = new Dictionary<string, ResourcePoolModifier>();


    public ResourcePool( float initialAmount, float initialMinAmount, float initialMaxAmount )
    {
        m_initialAmount = initialAmount;
        m_initialMaxAmount = initialMaxAmount;
        m_initialMinAmount = initialMinAmount;
    }

    public bool AddModifier( string modifierName, float delta, float tickRate )
    {
        return AddModifier( modifierName, new ResourcePoolModifier( delta, tickRate ) );
    }

    public bool AddModifier( string modifierName, ResourcePoolModifier modifier )
    {
        if( modifierName == null )
        {
            return false;
        }

        if( m_resourcePoolModifiers.ContainsKey( modifierName ) ) 
        { 
            return false;
        }

        if( modifier.Delta == 0.0f )
        {
            return false;
        }

        m_resourcePoolModifiers.Add( modifierName, modifier );
        return true;
    }

    public bool RemoveModifier( string modifierName )
    {
        return m_resourcePoolModifiers.Remove( modifierName );
    }

    public bool SetModifierDelta( string modifierName, float newDelta )
    {
        if( newDelta == 0.0f )
        { 
            return false; 
        }

        if (modifierName == null)
        {
            return false;
        }

        if (!m_resourcePoolModifiers.ContainsKey(modifierName))
        {
            return false;
        }

        m_resourcePoolModifiers[modifierName].Delta = newDelta;
        return true;
    }

    public bool AdjustModifierDelta( string modifierName, float deltaDelta )
    {
        if (deltaDelta == 0.0f)
        {
            return false;
        }

        if (modifierName == null)
        {
            return false;
        }

        if (!m_resourcePoolModifiers.ContainsKey(modifierName))
        {
            return false;
        }

        m_resourcePoolModifiers[modifierName].Delta += deltaDelta;
        return true;
    }

    public bool ModifyResource( float delta, bool force = false )
    {
        if (!force)
        {
            float adjustedAmount = m_amount + delta;
            if (adjustedAmount < m_minAmount || adjustedAmount > m_maxAmount)
            {
                return false;
            }
            else
            {
                m_amount = adjustedAmount;
                return true;
            }
        }
        else
        {
            m_amount += delta;
            m_amount = Mathf.Min(Mathf.Max(m_amount, m_minAmount), m_maxAmount);
            return true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_amount = m_initialAmount;
        m_maxAmount = m_initialMaxAmount;
        m_minAmount = m_initialMinAmount;
    }

    // Update is called once per frame
    void Update()
    {
        foreach( var modifier in m_resourcePoolModifiers.Values ) 
        {
            m_amount += modifier.UpdateAndGetFrameModifier(Time.deltaTime);
        }
        
        m_amount = Mathf.Min(Mathf.Max(m_amount, m_minAmount), m_maxAmount);
    }
}
