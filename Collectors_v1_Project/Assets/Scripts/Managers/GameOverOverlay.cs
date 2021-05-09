using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameOverOverlay : MonoBehaviour
{
    [Header("General References")]
    [SerializeField] private bool isFinished;
    [SerializeField] public bool playerWon = false;

    [Header("Outcome Variables")]
    [SerializeField] public int wasteGeneratedInt;
    [SerializeField] public int totalWasteCollectedInt;
    [SerializeField] public int wasteRemainingPercentageInt;
    [SerializeField] public int satisfactionPercentageInt;

    [Header("External References")]
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private WasteCenteManager myWasteCentreManager;
    [SerializeField] private GameObject overlayCanvas;
    [SerializeField] private TimeManager myTimeManager;

    [Header("Text Elements")]
    [SerializeField] public TextMeshProUGUI satisfactionScore;
    [SerializeField] public TextMeshProUGUI wasteRemaining;
    [SerializeField] public TextMeshProUGUI wasteGenerated;
    [SerializeField] public TextMeshProUGUI wasteCollected;
    [SerializeField] private TextMeshProUGUI outcome;


    private void Awake()
    {
        if (overlayCanvas != null) overlayCanvas.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();
        myWasteCentreManager = FindObjectOfType<WasteCenteManager>();
        myTimeManager = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
        {
            if (myGameManager != null)
            {
                if (myGameManager.isGameOver)
                {
                    overlayCanvas.SetActive(true); // bring up the overloay
                    isFinished = true; // run this only once
                    myTimeManager.timeSpeed = 0.0f; // stop time
                    CollectStats(); // get the stats
                }
            }
        }
    }

    public void CollectStats()
    {
        // satisfaction calculations
        GetSatisfactionFinal();

        // waste remaining in city calculations
        GetWasteRemainingFinal();

        // total waste generated calculations
        GetWasteGeneratedFinal();

        // total waste collected calculations
        GetWasteCollectedTotal();

        // advise the player of the outcome of play
        AdviseOutcome();
    }

    public void AdviseOutcome()
    {
        // if the player's score was higher than the required value
        if (satisfactionPercentageInt >= myGameManager.satisfactionToWin)
        {
            // they won
            playerWon = true;
            outcome.text = "WON!";
        }
        else
        {
            // they lost
            playerWon = false;
            outcome.text = "LOST!";
        }

    }

    public void GetWasteCollectedTotal()
    {
        // initalise temp variable for later use
        float garbageInCollectors = 0.0f;

        // find all the collectors in the city
        GarbagePickup[] collectors = FindObjectsOfType<GarbagePickup>();
        foreach (GarbagePickup garbageManager in collectors)
        {
            // accumulate their remaining garbage not yet delivered
            garbageInCollectors += garbageManager.garbageInCollector;
        }

        // add the collector garbage with the garbage at the Waste Centre
        float totalwasteCollected = myWasteCentreManager.generalWaste + garbageInCollectors;

        // convert to int
        totalWasteCollectedInt = Convert.ToInt32(totalwasteCollected);

        // send it to the UGUI
        wasteCollected.text = totalWasteCollectedInt.ToString();
    }

    public void GetWasteGeneratedFinal()
    {
        wasteGeneratedInt = Convert.ToInt32(myGameManager.totalWasteGenerated);
        wasteGenerated.text = wasteGeneratedInt.ToString();
    }

    public void GetWasteRemainingFinal()
    {
        float wasteRemainingPercentge = (myGameManager.cityGarbageLevel / myGameManager.maxCityGarbageLevel) * 100;
        wasteRemainingPercentageInt = Convert.ToInt32(wasteRemainingPercentge); // rounded for simpler score
        wasteRemaining.text = wasteRemainingPercentageInt.ToString();
    }

    public void GetSatisfactionFinal()
    {
        float satisfactionPercentage = (myGameManager.citySatisfactionLevel / myGameManager.maxCitySatisfactionLevel) * 100;
        satisfactionPercentageInt = Convert.ToInt32(satisfactionPercentage); // rounded for simpler score
        satisfactionScore.text = satisfactionPercentageInt.ToString();
    }
}
