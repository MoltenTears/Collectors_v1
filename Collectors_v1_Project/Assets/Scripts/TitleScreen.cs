using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] GameManager myGameManager;

    [SerializeField] GameObject instructionsGO;
    [SerializeField] GameObject difficulyGO;
    [SerializeField] SceneField FirstScene;

    [Header("Difficultly - EASY")]
    [SerializeField] private int daysEasy;
    [SerializeField] private float garbageSpeedEasy;
    [SerializeField] private int startingCollectorsEasy;

    [Header("Difficultly - MEDIUM")]
    [SerializeField] private int daysMedium;
    [SerializeField] private float garbageSpeedMedium;
    [SerializeField] private int startingCollectorsMedium;

    [Header("Difficultly - HARD")]
    [SerializeField] private int daysHard;
    [SerializeField] private float garbageSpeedHard;
    [SerializeField] private int startingCollectorsHard;

    private void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();

        // find instructionsGO and set it to false
        instructionsGO = GameObject.FindGameObjectWithTag("Instructions");
        instructionsGO.SetActive(false);

        // find difficultyGO and set it to false
        difficulyGO = GameObject.FindGameObjectWithTag("Difficulty");
        difficulyGO.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // hard quick quit function
        {
            ExitButton();
        }
    }

    public void ChooseDifficulty()
    {
        difficulyGO.SetActive(true);
    }

    public void ChoosDifficultyEasy()
    {
        // set days to play
        myGameManager.daysToPlay = daysEasy;

        // set accumuilation speed
        myGameManager.garbageMultipler = garbageSpeedEasy;

        // set number of Collectors 
        myGameManager.startingBaseCollectors = startingCollectorsEasy;

        // Start Game
        StartGame();
    }

    public void ChoosDifficultyMedium()
    {
        // set days to play
        myGameManager.daysToPlay = daysMedium;

        // set accumuilation speed
        myGameManager.garbageMultipler = garbageSpeedMedium;

        // set number of Collectors 
        myGameManager.startingBaseCollectors = startingCollectorsMedium;

        // Start Game
        StartGame();
    }

    public void ChoosDifficultyHard()
    {
        // set days to play
        myGameManager.daysToPlay = daysHard;

        // set accumuilation speed
        myGameManager.garbageMultipler = garbageSpeedHard;

        // set number of Collectors 
        myGameManager.startingBaseCollectors = startingCollectorsHard;

        // Start Game
        StartGame();
    }


    public void StartGame()
    {
        SceneManager.LoadScene(FirstScene.SceneName);
    }

    public void InstructionsButton()
    {
        instructionsGO.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();

    }

    public void ReturnButton()
    {
        instructionsGO.SetActive(false);
        difficulyGO.SetActive(false);
    }
}
