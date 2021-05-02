using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ClockManager : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI hours;
    [SerializeField] private TextMeshProUGUI period;
    [SerializeField] private TextMeshProUGUI days;
    [SerializeField] private TextMeshProUGUI daysText;
    [SerializeField] private TextMeshProUGUI remainsText;


    [Header("External References")]
    [SerializeField] private TimeManager myTimeManger;
    [SerializeField] private GameManager myGameManager;

    [Header("Local Variables")]
    [SerializeField] public bool isMorning;

    // Start is called before the first frame update
    void Start()
    {
        myTimeManger = FindObjectOfType<TimeManager>();
        myGameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GetHours();
        GetPeriod();
        GetDays();
        FixWords();
    }

    private void FixWords()
    {
        if (myGameManager.daysLeftToPlay == 1) // if exactly one day remains
        {
            daysText.text = "Day";
            remainsText.text = "Remains";
        }
        else
        {
            daysText.text = "Days";
            remainsText.text = "Remain";
        }
    }

    private void GetDays()
    {
        days.text = myTimeManger.days.ToString();
    }

    private void GetPeriod()
    {
       if (myTimeManger.isMorning)
        {
            period.text = "AM";
        }
       else
        {
            period.text = "PM";
        }
    }

    private void GetHours()
    {
        hours.text = myTimeManger.timeHours.ToString();
    }
}
