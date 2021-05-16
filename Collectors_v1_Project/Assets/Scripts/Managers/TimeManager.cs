using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Clock Speed")]
    public float timeSpeed;
    private const int secondsPerMinute = 60;

    [Header("Current Time")]
    public int days;
    public int timeHours;
    public int timeMinutes;
    public float timeSeconds;
    public float totalSeconds;

    [Header("Maxium values")]
    [SerializeField] public bool isMorning = true;
    private const int timeHoursMax = 11;
    private const int timeMinutesMax = 59;
    private const int timeSecondsMax = 59;
    [HideInInspector] public const float totalSecondsMax = 86400;

    [Header("Starting Time")]
    [SerializeField] private int timeHoursStart = 5;
    [SerializeField] private int timeMinutesStart = 0;
    [SerializeField] private int timeSecondsStart = 0;

    [Header("External References")]
    [SerializeField] private GameManager myGameManger;
    [SerializeField] private ManageHubs myManagehubs;
    [SerializeField] private DifficultyNumber myDifficultyNumber;

    private void Awake()
    {
        myGameManger = FindObjectOfType<GameManager>();
        
        
        myDifficultyNumber = FindObjectOfType<DifficultyNumber>();
        if (myDifficultyNumber.difficultyNo == 1) myGameManger.difficultySetting = GameManager.DifficultySetting.EASY;
        else if (myDifficultyNumber.difficultyNo == 2) myGameManger.difficultySetting = GameManager.DifficultySetting.MEDIUM;
        else if (myDifficultyNumber.difficultyNo == 3) myGameManger.difficultySetting = GameManager.DifficultySetting.HARD;
        else myGameManger.difficultySetting = GameManager.DifficultySetting.MEDIUM;


    }

    // Start is called before the first frame update
    void Start()
    {
        // get references
        myManagehubs = FindObjectOfType<ManageHubs>();

        // set days
        
        SetStartTime();
    }


    // Update is called once per frame
    void Update()
    {
        days = myGameManger.daysToPlay;

        UpdateSeconds();

        ResetSeconds();
    }

    private void LateUpdate()
    {
        if (!myGameManger.isDifficultySet)
        {
            Debug.Log($"difficulty set: {myGameManger.isDifficultySet}.");

            // set the Zones again in the GameMAnager Singelton
            myGameManger.SetZones();

            // set the difficult variables
            myGameManger.SetDifficulty();

            // record the RoadHubs in the ManageHubs
            myManagehubs.ConfirmHubs();
        }
    }

    void UpdateSeconds()
    {
        // collect seconds since last system tick
        if (timeSpeed > 0)
        {
            timeSeconds += ((Time.deltaTime * secondsPerMinute) * timeSpeed);
            totalSeconds += ((Time.deltaTime * secondsPerMinute) * timeSpeed); // for the sun rotation
        }
        else if (timeSpeed <= 0 && !myGameManger.isGameOver)
            Debug.LogError("clockSpeed set to zero, provide speed greater than or equal to 1 in TimeManager.cs");

        // check if the time is still real (nothing above 59)
        if (timeSeconds > timeSecondsMax)
        {
            // reset the seconds counter (but don't trim the overrun)
            timeSeconds = 0.0f + (timeSeconds - 60);

            // update the hours
            UpdateMinutes();
        }
    }

    void UpdateMinutes()
    {
        // add a minute
        timeMinutes++;

        // check if the time is still real (nothing above 59)
        if (timeMinutes > timeMinutesMax)
        {
            // reset the minute counters
            timeMinutes = 0;

            // update the hours
            UpdateHours();
        }
    }

    void UpdateHours()
    {
        if (timeHours == 12)
        {
            // rollback the clock to 1 o'clock
            timeHours = 1;            
        }
        else
        {
            // add an hour
            timeHours++;
        }

        // check if the hour is still real (nothing above 12)
        if (timeHours > timeHoursMax)
        {
            // ...reset the hour counter
            timeHours = 12;

            // flip the period
            isMorning = !isMorning;

            // if it just became AM, change days
            if (isMorning) UpdateDays();
        }
    }

    private void UpdateDays()
    {
        // remove a day from the counter
        days--;

        // and the Game Manager
        myGameManger.daysLeftToPlay--;
    }

    private void SetStartTime()
    {
        // set the starting time
        timeHours = timeHoursStart;
        timeMinutes = timeMinutesStart;
        timeSeconds = timeSecondsStart;
    }

    void ResetSeconds()
    {
        if (totalSeconds >= totalSecondsMax)
        {
            totalSeconds = 0.0f;
        }
    }
}
