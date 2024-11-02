using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    //[SerializeField] Dropdown resolutionDropDown;
    [SerializeField] TMP_Dropdown resolutionDropDown;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] AudioClip gunshotClip;



    private int selectedResolutionIndex;
    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;


    private AudioSource sfxSource;  // AudioSource for playing sound effects

    private void Start()
    {
        // Initialize AudioSource for SFX playback
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; // Set output to SFX group
        sfxSource.playOnAwake = false;

        // Set up resolution dropdown options
        List<string> options = new List<string> { "Option A", "Option B", "Option C" };
        resolutionDropDown.ClearOptions();
        resolutionDropDown.AddOptions(options);

        // Load saved settings
        LoadSettings();
    }

    private void LoadSettings()
    {
        // Load saved resolution option and set the dropdown
        selectedResolutionIndex = PlayerPrefs.GetInt("ResolutionOption", 0);
        resolutionDropDown.value = selectedResolutionIndex;

        // Load saved volume levels and set sliders
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);  // Convert linear slider value to dB
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    public void SetResolution(int optionIndex)
    {
        selectedResolutionIndex = optionIndex;
    }

    public void ApplySettings()
    {
        // Apply resolution setting based on selected index
        switch (selectedResolutionIndex)
        {
            case 0: Screen.SetResolution(1280, 720, Screen.fullScreen); break;
            case 1: Screen.SetResolution(1920, 1080, Screen.fullScreen); break;
            case 2: Screen.SetResolution(2560, 1440, Screen.fullScreen); break;
        }

        // Save the settings
        PlayerPrefs.SetInt("ResolutionOption", selectedResolutionIndex);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    // Method to play the gunshot sound effect
    public void PlayGunshotSFX()
    {
        if (gunshotClip != null)
        {
            sfxSource.PlayOneShot(gunshotClip);
        }
        else
        {
            Debug.LogWarning("Gunshot clip not assigned!");
        }
    }

    public void CloseSettings()
    {
        Debug.Log("CloseSettings called");

        if (GameManager.instance != null)
        {
            GameManager.instance.UnloadSettingsScene();
        }
        else
        {
            Debug.LogWarning("SceneManager.instance is null. Make sure SceneManager is initialized in the scene.");
        }
    }
}
