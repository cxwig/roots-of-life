using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IncreserUpdater : MonoBehaviour
{
    public Slider slider;
    
    public float m_previousValue;

    void Start()
    {
        m_previousValue = slider.value;
    }

    void Update()
    {
        if (slider.value != m_previousValue)
        {
            float change = slider.value - m_previousValue;
            if (change > 0)
            {
                GetComponent<TextMeshProUGUI>().text = "+ " + change.ToString();
            }
            else
            {
                GetComponent<TextMeshProUGUI>().text = change.ToString();
            }
            m_previousValue = slider.value;
        }
    }
}
