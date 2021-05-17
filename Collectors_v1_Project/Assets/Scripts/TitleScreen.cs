using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] GameManager myGameManager;
    [SerializeField] DifficultyNumber myDifficultyNumber;

    [SerializeField] GameObject instructionsGO;
    [SerializeField] GameObject difficulyGO;
    [SerializeField] SceneField FirstScene;

    private void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();
        myDifficultyNumber = FindObjectOfType<DifficultyNumber>();

        // find instructionsGO and set it to false
        instructionsGO = GameObject.FindGameObjectWithTag("Instructions");
        //instructionsGO.SetActive(false);

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
        // set Game Manager
        myGameManager.difficultySetting = GameManager.DifficultySetting.EASY;

         myDifficultyNumber.difficultyNo = 1;

        // Start Game
        StartGame();
    }

    public void ChoosDifficultyMedium()
    {
        // set Game Manager
        myGameManager.difficultySetting = GameManager.DifficultySetting.MEDIUM;

        myDifficultyNumber.difficultyNo = 2;

        // Start Game
        StartGame();
    }

    public void ChoosDifficultyHard()
    {
        // set Game Manager
        myGameManager.difficultySetting = GameManager.DifficultySetting.HARD;

        myDifficultyNumber.difficultyNo = 3;

        // Start Game
        StartGame();
    }


    public void StartGame()
    {

        // Debug.Log($"StartGame() called with difficultly: {myGameManager.difficultySetting}.");
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
