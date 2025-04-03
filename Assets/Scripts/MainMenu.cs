using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Main Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject audioSettingsPanel;
    public GameObject videoSettingsPanel;

    [Header("Audio Settings")]
    public Slider masterVolumeSlider;

    [Header("Video Settings")]
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        LoadSettings();

        // Setup resolution options
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string resOption = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(resOption);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    // ================= MAIN MENU =================
    public void PlayGame()
    {
        SceneManager.LoadScene("Test");
    }

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenAudioSettings()
    {
        audioSettingsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OpenVideoSettings()
    {
        videoSettingsPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    // ================= AUDIO SETTINGS =================
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void BackToOptionsFromAudio()
    {
        audioSettingsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // ================= VIDEO SETTINGS =================
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void SetResolution(int index)
    {
        if (resolutions == null || index < 0 || index >= resolutions.Length) return;

        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }

        public void BackToOptionsFromVideo()
    {
        videoSettingsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // ================= LOAD SAVED SETTINGS =================
    void LoadSettings()
    {
        // Load volume
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = volume;
        if (masterVolumeSlider) masterVolumeSlider.value = volume;

        // Load fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;
        if (fullscreenToggle) fullscreenToggle.isOn = isFullscreen;
    }
}
