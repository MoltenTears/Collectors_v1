using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] GameObject instructionsGO;
    [SerializeField] SceneField FirstScene;

    private void Start()
    {
        instructionsGO = GameObject.FindGameObjectWithTag("Instructions");
        instructionsGO.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // hard quick quit function
        {
            ExitButton();
        }
    }


    public void StartGameButton()
    {
        Debug.Log("Start Button Pressed.");

        SceneManager.LoadScene(FirstScene.SceneName);
    }

    public void InstructionsButton()
    {
        Debug.Log("Instructions Button Pressed.");

        instructionsGO.SetActive(true);
    }

    public void ExitButton()
    {
        Debug.Log("Exit Button Pressed.");

        Application.Quit();

    }

    public void ReturnButton()
    {
        Debug.Log("Return button pressed");

        instructionsGO.SetActive(false);
    }
}
