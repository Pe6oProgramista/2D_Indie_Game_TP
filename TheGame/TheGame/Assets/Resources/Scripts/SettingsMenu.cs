using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Dropdown qualityDropdown;
    public Slider volumeSlider;
    public Toggle fpsToggle;
    public Slider fpsSlider;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        int currResolutionIndex = 0;

        List<string> resolutionsNames = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            resolutionsNames.Add(resolutions[i].width + " x " + resolutions[i].height);
            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionsNames);
        resolutionDropdown.value = currResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = Screen.fullScreen;

        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        float volume;
        audioMixer.GetFloat("volume", out volume);
        volumeSlider.value = volume;

        fpsToggle.isOn = ApplicationModel.fpsOn;
        fpsSlider.value = ApplicationModel.targetFrameRate;
    }

    public void Setvolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
	
	public void Setfps(float limit) {
		ApplicationModel.targetFrameRate = (int)limit;
	}

    public void Showfps(bool fpsOn)
    {
        ApplicationModel.fpsOn = fpsOn;
    }

    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }
}
