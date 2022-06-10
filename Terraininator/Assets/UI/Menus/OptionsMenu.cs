using UnityEngine;
using TMPro;
using System.Collections.Generic;

//This class is a collection of utility methods for use in the options menu.
//  At Start() it updates the resolution dropdown with a list of possible resolutions,
//      and selects a default one based on screen resolution.
public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        List<string> resOptions = new List<string>();
        int currentResolutionI = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resOptions.Add(resolutions[i].width + " x " + resolutions[i].height + 
                          " @ " + resolutions[i].refreshRate + " hz");

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionI = i;
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resOptions);

        resolutionDropdown.value = currentResolutionI;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
    }
}
