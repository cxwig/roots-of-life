using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Amount : MonoBehaviour
{
    [SerializeField]
    private bool DirtyBool;

    public int m_nowValue;
    public int m_maxAmount;


    void Start()
    {
        DirtyBool = false;
    }


    public void ChangeValue(int EnergyValue, int MaxAmount)
    {
        if(EnergyValue != m_nowValue|| m_maxAmount != MaxAmount)
        {
            DirtyBool = true;
            m_nowValue = EnergyValue;
            m_maxAmount = MaxAmount;
        }
    }

    void Update()
    {
        if (DirtyBool)
        {
            DirtyBool = false;
            // m_nowValue = 
            GetComponent<TextMeshProUGUI>().text = m_nowValue + "/" + m_maxAmount;
        }
    }
}
