using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingdisplay : MonoBehaviour
{
    public void setfullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
