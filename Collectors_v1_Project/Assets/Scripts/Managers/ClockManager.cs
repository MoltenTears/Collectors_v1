using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ClockManager : MonoBehaviour
{
    [Header("Text Fields")]
    public TextMeshProUGUI hours;
    public TextMeshProUGUI period;
    public TextMeshProUGUI days;

    [Header("External References")]
    [SerializeField] private TimeManager myTimeManger;

    [Header("Local Variables")]
    [SerializeField] public bool isMorning;

    // Start is called before the first frame update
    void Start()
    {
        myTimeManger = FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        GetHours();
        GetPeriod();
        GetDays();
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
