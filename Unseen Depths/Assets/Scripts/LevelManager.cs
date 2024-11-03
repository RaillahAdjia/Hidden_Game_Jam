using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] GameObject[] level1Creatures;
    [SerializeField] GameObject[] level2Creatures;
    [SerializeField] GameObject[] level3Creatures;
    [SerializeField] GameObject[] level4Creatures;
    [SerializeField] int[] killRequirements; // Number of kills required for each level
    [SerializeField] Diver targetDiver;
    [SerializeField] GameObject diverPrefab;

    [Header("UI Elements")]
    [SerializeField] GameObject nextLevelPanel;
    [SerializeField] TMP_Text killCountText;
    [SerializeField] TMP_Text nextLevelButtonText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text descriptionText; // UI element for descriptions
    [SerializeField] TMP_Text timerText; // Timer UI element

    [Header("Spawn Settings")]
    [SerializeField] float spawnInterval = 2.0f;
    private bool playerDied = false;

    private int currentLevel = 1;
    private int currentKillCount = 0;
    private bool levelComplete = false;
    private float timer; // Timer variable

    void Start(){
        nextLevelPanel.SetActive(false);
        UpdateLevelUI();
        StartCoroutine(SpawnCreatures());
        timer = 0f; // Initialize timer
        UpdateDescriptionForGameMode(); // Set initial description based on game mode
    }

    void Update(){
        UpdateKillUI();
        CheckLevelCompletion();
        UpdateTimer(); // Update timer every frame
    }

    void UpdateTimer(){
        if (!levelComplete){
            timer += Time.deltaTime; // Increment timer
            timerText.text = "Time: " + Mathf.FloorToInt(timer).ToString() + " SECONDS"; // Update timer text
        }
    }

    IEnumerator SpawnCreatures(){
        if(GameManager.instance.currentGameMode == GameManager.GameMode.Story){
            while (!levelComplete){
            GameObject creatureToSpawn = GetCreatureForCurrentLevel();
            if (creatureToSpawn != null){
                SpawnCreature(creatureToSpawn);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        } else if(GameManager.instance.currentGameMode == GameManager.GameMode.Survival){
            yield return StartCoroutine(SpawnAllCreatures());
        }
    }

    GameObject GetCreatureForCurrentLevel(){
        switch (currentLevel){
            case 1:
                return GetRandomCreature(level1Creatures);
            case 2:
                return GetRandomCreature(level2Creatures);
            case 3:
                return GetRandomCreature(level3Creatures);
            case 4:
                return GetRandomCreature(level4Creatures);
            default:
                return null;
        }
    }

    GameObject GetRandomCreature(GameObject[] creatures){
        if (creatures.Length == 0) return null;
        int randomIndex = Random.Range(0, creatures.Length);
        return creatures[randomIndex];
    }

    void SpawnCreature(GameObject creaturePrefab){
        float xPosition = (Random.value > 0.5f) ? -12.0f : 12.0f;
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0);
        GameObject creature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        creature.GetComponent<CreatureAI>().targetDiver = targetDiver;
        creature.GetComponent<CreatureAI>().OnCreatureKilled += HandleCreatureKilled; // Subscribe to the kill event
    }

    void HandleCreatureKilled(){
        currentKillCount++;
        UpdateKillUI();
    }

    void CheckLevelCompletion(){
        if (currentKillCount >= killRequirements[currentLevel - 1] && !levelComplete && GameManager.instance.currentGameMode == GameManager.GameMode.Story){
            levelComplete = true;
            DestroyAllCreatures();
            ShowNextLevelPanel();
            UpdateDescriptionForNextLevel(); // Update description for the next level
            SetNextLevelButtonText();
        }
        else if (GameManager.instance.currentGameMode == GameManager.GameMode.Survival){

        }
    }

    void ShowNextLevelPanel(){
        nextLevelPanel.SetActive(true);
        StopCoroutine(SpawnCreatures());
    }

    IEnumerator SpawnAllCreatures(){
        // Spawn all creatures from level 4 at once
        foreach (var creaturePrefab in level4Creatures){
            SpawnCreature(creaturePrefab);
        }
        yield break; // Exit the coroutine after spawning all creatures
    }

    void DestroyAllCreatures(){
        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");
        foreach (GameObject creature in creatures){
            Destroy(creature);
        }
    }

    public void PlayerDied() {
        StopAllCoroutines(); // Stop spawning creatures
        levelComplete = true; // Set level as complete
        playerDied = true; // Mark that the player died

        DestroyAllCreatures();

        ShowNextLevelPanel(); // Show the panel
        SetNextLevelButtonText(); // Update the button text to "Play Again"

        UpdateLevelUI();
    }

    private void SpawnNewDiver(){
        Vector3 spawnPosition = new Vector3(0, -4, 0); // Set spawn position for the new diver
        GameObject newDiver = Instantiate(diverPrefab, spawnPosition, Quaternion.identity);
        targetDiver = newDiver.GetComponent<Diver>();
        // Get the Diver component from the newly instantiated diver
        Diver diverComponent = newDiver.GetComponent<Diver>();

        // Locate the PlayerInputHandler object and assign the Diver component to the player variable
        PlayerInputHandler inputHandler = FindObjectOfType<PlayerInputHandler>();
        if (inputHandler != null){
            inputHandler.player = diverComponent; // Assign the Diver component to the player variable
        }
    }

    void SetNextLevelButtonText(){
        if (playerDied) {
            // If the player died, show "Play Again"
            nextLevelButtonText.text = "Play Again";
        } else if (GameManager.instance.currentGameMode == GameManager.GameMode.Story){
            // If in Story mode and player completed the level, show "Next Level"
            nextLevelButtonText.text = "Next Level";
        } else{
            // In Survival mode, use "Play Again" for replaying
            nextLevelButtonText.text = "Play Again";
        }
    }

    public void OnNextLevelButton(){
        if (playerDied){
            // Replay the current level by resetting necessary variables
            playerDied = false;
            currentKillCount = 0;
            levelComplete = false;
            nextLevelPanel.SetActive(false);
            SpawnNewDiver();
            UpdateLevelUI();
            StartCoroutine(SpawnCreatures());
            timer = 0.0f; // Reset timer if necessary
        }
        else if (GameManager.instance.currentGameMode == GameManager.GameMode.Survival){
            // Restart Survival Mode
            RestartSurvivalMode();
        }
        else if (currentLevel >= 4){
            // Load the FinalLevel scene in Story mode
            SceneManager.LoadScene("FinalLevel");
        }
        else{
        // Move to the next level
            currentLevel++;
            currentKillCount = 0;
            levelComplete = false;
            nextLevelPanel.SetActive(false);
            UpdateLevelUI();
            StartCoroutine(SpawnCreatures());
            timer = 0.0f;
        }

        SetNextLevelButtonText(); // Ensure button text is updated based on new level state
    }

    void RestartSurvivalMode(){
        currentKillCount = 0;
        levelComplete = false;
        nextLevelPanel.SetActive(false);
        UpdateLevelUI();
        StartCoroutine(SpawnAllCreatures());
        timer = 0.0f; // Spawn all creatures immediately
    }

    void UpdateLevelUI(){
        if(playerDied){
            levelText.text = "YOU DIED";
        }
        else if (GameManager.instance.currentGameMode == GameManager.GameMode.Story){
            levelText.text = "Level " + currentLevel + " Complete";
        }
        else if(GameManager.instance.currentGameMode == GameManager.GameMode.Survival){
            levelText.text = "SURVIAL MODE"; //in Survival Mode
        }
    }

    void UpdateKillUI(){
        if (GameManager.instance.currentGameMode == GameManager.GameMode.Survival){
            killCountText.text = "Total Kills: " + currentKillCount; // Display death tally in Survival Mode
        }
        else{
            killCountText.text = "Kill Goal: " + currentKillCount + "/" + killRequirements[currentLevel - 1];
        }
    }

    void UpdateDescriptionForGameMode(){
        if (GameManager.instance.currentGameMode == GameManager.GameMode.Story){
            descriptionText.text = "There levels to this huh?";
        }
        else if (GameManager.instance.currentGameMode == GameManager.GameMode.Survival){
            descriptionText.text = "Dang they got your ass!";
            spawnInterval = 1.5f;
        }
    }

    void UpdateDescriptionForNextLevel(){
        switch (currentLevel){
            case 1:
                descriptionText.text = "The sky looks dark, you must be under a storm.";
                break;
            case 2:
                descriptionText.text = "We are right around the area, but something dosen't feel right.";
                break;
            case 3:
                descriptionText.text = "Saw something following the boat, becareful down there.";
                break;
            case 4:
                descriptionText.text = "Hurry grab the rope so we can pull you up and get out of here!";
                break;
            default:
                descriptionText.text = "";
                break;
        }
    }

    public void OpenSettings(){
        GameManager.instance.LoadSettingsScene();
    }

    public void QuitLevel(){
        GameManager.instance.LoadScene("MainMenu");
    }
}
