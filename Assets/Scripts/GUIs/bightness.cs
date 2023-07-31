using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class bightness : MonoBehaviour
{
    public Slider bightnessSlider;
    public PostProcessProfile brightness;
    public PostProcessLayer Layer;

    AutoExposure exposure;

     void Start()
    {
       
        brightness.TryGetSettings(out exposure);
        AdjustBrightness(bightnessSlider.value);
    }
    public void AdjustBrightness(float value)
    {
        if (value != 0)
        {
            exposure.keyValue.value = value;
        }
        else
        {
            exposure.keyValue.value = .05f;
        }
    }
}
