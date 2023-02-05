using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderController : MonoBehaviour
{
    public float progress = 0.0f;
    public Slider slider;

    public void UpdateProgress(float perc)
    {
        slider.value = perc;
    }




}
