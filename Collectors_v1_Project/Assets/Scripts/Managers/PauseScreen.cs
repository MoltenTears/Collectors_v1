using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utilities;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameOverOverlay myGameOverOverlay;
    [SerializeField] private float originalTimeScale;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private bool isPaused;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private SceneField titleScene;

    [Header("Text Elements")]
    [SerializeField] public TextMeshProUGUI satisfactionScore;
    [SerializeField] public TextMeshProUGUI wasteRemaining;
    [SerializeField] public TextMeshProUGUI wasteGenerated;
    [SerializeField] public TextMeshProUGUI wasteCollected;

    // Start is called before the first frame update
    void Start()
    {
        myGameOverOverlay = FindObjectOfType<GameOverOverlay>();
        pauseScreen = GameObject.FindGameObjectWithTag("PauseScreen");
        originalTimeScale = 1;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // or through the button function
        {
            isPaused = !isPaused;
        }

        ChangePauseState();
    }

    private void ChangePauseState()
    {
        if (isPaused)
        {
            // get figures to display
            UpdateStats();

            // set the new time to zero
            Time.timeScale = 0.0f;

            // activate the overlay
            pauseScreen.SetActive(true);

            // show the oppiste sprite
            pauseButton.image.sprite = playSprite;
        }
        else
        {
            // set the new time back to before being paused
            Time.timeScale = originalTimeScale;

            // Deactivate the overlay
            pauseScreen.SetActive(false);

            // show the oppiste sprite
            pauseButton.image.sprite = pauseSprite;
        }
    }

    public void ChangePauseScreen()
    {
        isPaused = !isPaused;
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene(titleScene.SceneName);
    }

    private void UpdateStats()
    {
        myGameOverOverlay.CollectStats();

        // Debug.Log($"Collect Stats run.");

        satisfactionScore.text = myGameOverOverlay.satisfactionPercentageInt.ToString();
        wasteRemaining.text = myGameOverOverlay.wasteRemainingPercentageInt.ToString();
        wasteGenerated.text = myGameOverOverlay.wasteGeneratedInt.ToString();
        wasteCollected.text = myGameOverOverlay.totalWasteCollectedInt.ToString();
    }
}
