using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private HouseManager myHouseManager;
    [SerializeField] private GameManager myGameManager;
    [SerializeField] private float satisfactionTollerance;
    [SerializeField] public float satisfactionPercentage;

    [SerializeField] public float otherGarbageLevel;
    [SerializeField] private List<GarbageManager> otherGarbageManagers = new List<GarbageManager>();


    // Start is called before the first frame update
    void Start()
    {
        myHouseManager = GetComponentInParent<HouseManager>();
        myGameManager = FindObjectOfType<GameManager>();

        SetMaxGarbageTollerance();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGarbageLevel();
        UpdateSatisfactionLevel();
    }

    private void UpdateSatisfactionLevel()
    {
        satisfactionPercentage = 100 - ((otherGarbageLevel / satisfactionTollerance) * 100);
    }

    private void SetMaxGarbageTollerance()
    {
        switch (myHouseManager.houseType)
        {
            case HouseManager.HouseType.SINGLE:
                {
                    satisfactionTollerance = myGameManager.satisfactionTolleranceSingle;
                    break;
                }
            case HouseManager.HouseType.FAMILY:
                {
                    satisfactionTollerance = myGameManager.satisfactionTolleranceFamily;
                    break;
                }
            case HouseManager.HouseType.SHARE:
                {
                    satisfactionTollerance = myGameManager.satisfactionTolleranceShare;
                    break;
                }
            case HouseManager.HouseType.NONE:
                {
                    Debug.LogWarning("SatisfactionManager.SetMaxGarbageTollerance() housetype set to NONE, tollerance not set.");
                    break;
                }
            default:
                {
                    Debug.LogError("SatisfactionManager.SetMaxGarbageTollerance() switch defaulted, review and correct.");
                    break;
                }
        }
    }

    private void UpdateGarbageLevel()
    {
        otherGarbageLevel = 0.0f;

        for (int i = 0; i < otherGarbageManagers.Count; i++)
        {
            otherGarbageLevel += otherGarbageManagers[i].garbageLevel;
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
