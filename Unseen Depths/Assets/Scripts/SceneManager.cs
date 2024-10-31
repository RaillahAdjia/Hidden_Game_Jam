using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName){
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void LoadSettingsScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void UnloadSettingsScene(){
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Settings");
    }

    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
