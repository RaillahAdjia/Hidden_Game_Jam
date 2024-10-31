using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerControl : MonoBehaviour
{
    private void Awake()
    {
        // Find all AudioListeners in the scene
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

        // If there is more than one AudioListener, destroy the current GameObject
        if (audioListeners.Length > 1)
        {
            Destroy(gameObject); // This will destroy the GameObject the script is attached to
        }
    }
}