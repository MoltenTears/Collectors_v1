using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatisfactionManager : MonoBehaviour
{
    public enum SatisfactionLevel
    {
        NONE,
        LOW,
        MEDIUM,
        HIGH
    }
    
    
    
    [SerializeField] private HouseManager myHouseManager;
    [SerializeField] private GameManager myGameManager;


    [Header("Satisfaction Variables")]
    [SerializeField] public SatisfactionLevel mySatisfcationLevel;
    private float satisfactionTolleranceLow;
    private float satisfactionTolleranceMedium;
    private float satisfactionTolleranceHigh;

    [SerializeField] public float otherGarbageLevel;
    [SerializeField] public float maxOtherGarbageLevel;
    [SerializeField] public float otherGarbagePercentage;
    [SerializeField] private List<GarbageManager> otherGarbageManagers = new List<GarbageManager>();


    // Start is called before the first frame update
    void Start()
    {
        myHouseManager = GetComponentInParent<HouseManager>();
        myGameManager = FindObjectOfType<GameManager>();

        GetSatisfactionTollerance();
    }

    // Update is called once per frame
    void Update()
    {
        MaxGarbageLevel(); 
        UpdateGarbageLevel();
        UpdateSatisfactionLevel();
    }

    private void GetSatisfactionTollerance()
    {
        switch (myHouseManager.houseType)
        {
            case HouseManager.HouseType.SINGLE:
                {
                    satisfactionTolleranceLow = myGameManager.satisfactionTolleranceSingleLow;
                    satisfactionTolleranceMedium = myGameManager.satisfactionTolleranceSingleMedium;
                    satisfactionTolleranceHigh = myGameManager.satisfactionTolleranceSingleHigh;
                    break;
                }
            case HouseManager.HouseType.FAMILY:
                {
                    satisfactionTolleranceLow = myGameManager.satisfactionTolleranceFamilyLow;
                    satisfactionTolleranceMedium = myGameManager.satisfactionTolleranceFamilyMedium;
                    satisfactionTolleranceHigh = myGameManager.satisfactionTolleranceFamilyHigh;
                    break;
                }
            case HouseManager.HouseType.SHARE:
                {
                    satisfactionTolleranceLow = myGameManager.satisfactionTolleranceShareLow;
                    satisfactionTolleranceMedium = myGameManager.satisfactionTolleranceShareMedium;
                    satisfactionTolleranceHigh = myGameManager.satisfactionTolleranceShareHigh;
                    break;
                }
            case HouseManager.HouseType.NONE:
                {
                    Debug.LogWarning("SatisfactionManager.GetSatisfactionTollerance() housetype set to none, tollerances not set.");
                    break;
                }
            default:
                {
                    Debug.LogError("SatisfactionManager.GetSatisfactionTollerance() switch failed, review and correct.");
                    break;
                }
        }
    }

    private void UpdateSatisfactionLevel()
    {
        if ((otherGarbageLevel / maxOtherGarbageLevel) >= satisfactionTolleranceHigh)
        {
            // most problems
            mySatisfcationLevel = SatisfactionLevel.NONE;
        }
        else if ((otherGarbageLevel / maxOtherGarbageLevel) >= satisfactionTolleranceMedium)
        {
            // more problems
            mySatisfcationLevel = SatisfactionLevel.LOW;
        }
        else if ((otherGarbageLevel / maxOtherGarbageLevel) >= satisfactionTolleranceLow)
        {
            // some problems
            mySatisfcationLevel = SatisfactionLevel.MEDIUM;
        }
        else if ((otherGarbageLevel / maxOtherGarbageLevel) < satisfactionTolleranceLow)
        {
            // no problems
            mySatisfcationLevel = SatisfactionLevel.HIGH;
        }
    }

    private void UpdateGarbageLevel()
    {
        otherGarbageLevel = 0.0f;

        for (int i = 0; i < otherGarbageManagers.Count; i++)
        {
            otherGarbageLevel += otherGarbageManagers[i].garbageLevel;
        }

        if (otherGarbageLevel < maxOtherGarbageLevel)
        {
            otherGarbagePercentage = otherGarbageLevel / maxOtherGarbageLevel;
        }
        else
        {
            otherGarbageLevel = maxOtherGarbageLevel;
            otherGarbagePercentage = 1.0f;
        }
    }

    private void MaxGarbageLevel()
    {
        maxOtherGarbageLevel = 0.0f;

        for (int i = 0; i < otherGarbageManagers.Count; i++)
        {
            maxOtherGarbageLevel = otherGarbageManagers[i].maxGarbageLevel;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // if a new GarbageManager is inside my Satisfaction radius...
        if (other.gameObject.CompareTag("House") && other.GetComponentInParent<GarbageManager>())
        {   
            // ... add it to my list of Houses that affect my SatisfactionLevel
            UpdateBins(other.GetComponentInParent<GarbageManager>(), true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if a  GarbageManager  inside my Satisfaction radius leaves...
        if (other.gameObject.CompareTag("House") && other.GetComponentInParent<GarbageManager>())
        {
            // ... remnove it from my list of Houses that affect my SatisfactionLevel
            UpdateBins(other.GetComponentInParent<GarbageManager>(), false);
        }
    }


    private void UpdateBins(GarbageManager _garbageManager, bool addBins)
    {
        if (addBins)
        {
            otherGarbageManagers.Add(_garbageManager);
        }
        else
        {
            otherGarbageManagers.Remove(_garbageManager);
        }
    }
}
