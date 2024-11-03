using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject lorePanel;
    [SerializeField] private GameObject playPanel;
    public Button storyModeButton;
    public Button survivalModeButton;

    private void Start(){
        //
        lorePanel.SetActive(false);
        playPanel.SetActive(false);

        // Assign button click listeners
        storyModeButton.onClick.AddListener(OnStoryModeButtonClicked);
        survivalModeButton.onClick.AddListener(OnSurvivalModeButtonClicked);
    }

    void OnStoryModeButtonClicked(){
        GameManager.instance.SetGameMode(GameManager.GameMode.Story);
        GameManager.instance.LoadScene("GamePlay");
    }

    void OnSurvivalModeButtonClicked(){
        GameManager.instance.SetGameMode(GameManager.GameMode.Survival);
        GameManager.instance.LoadScene("GamePlay");
    }

    public void ShowLorePanel(){
        if(lorePanel !=null){
            lorePanel.SetActive(true);
            playPanel.SetActive(false);
        }
    }

    public void ShowPlayPanel(){
        if(playPanel !=null){
            playPanel.SetActive(true);
            lorePanel.SetActive(false);
        }
    }

    public void HideLorePanel(){
        if(lorePanel !=null){
            lorePanel.SetActive(false);
        }
    }

    public void HidePlayPanel(){
        if(playPanel !=null){
            playPanel.SetActive(false);
        }
    }

    public void LoadSettingsScene(){
        GameManager.instance.LoadSettingsScene();
    }

    public void QuitGame(){
        GameManager.instance.QuitGame();
    }
}
