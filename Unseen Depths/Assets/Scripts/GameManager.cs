using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public enum GameMode { Story, Survival }
    public GameMode currentGameMode { get; private set; }

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void SetGameMode(GameMode mode)
    {
        currentGameMode = mode;
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
    public void QuitGame(){
        Application.Quit();
    }
}
