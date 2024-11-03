using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to your Audio Mixer
    public Slider volumeSlider;   // Reference to your UI Slider

    private void Start()
    {
        // Initialize the slider to the current volume setting
        float currentVolume;
        audioMixer.GetFloat("musicVolume", out currentVolume);
        volumeSlider.value = Mathf.Pow(10, currentVolume / 20); // Convert to linear scale

        // Add listener to detect slider changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // This function will be called whenever the slider value changes
    public void SetVolume(float volume)
    {
        // Convert slider value (0 to 1) to a decibel scale (-80 dB to 0 dB)
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("musicVolume", dB);
    }

    public void UnloadSettingsScene(){
        GameManager.instance.UnloadSettingsScene();
    }
}