using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField] public int garbageDivisor;

    [Header("Bins")]
    [SerializeField] public int binSizeSmall;
    [SerializeField] public int binSizeMedium;
    [SerializeField] public int binSizeLarge;

    [Header("Display Meter Levels")]
    [SerializeField] public float cityGarbageLevel;
    [SerializeField] public float maxCityGarbageLevel;
    [SerializeField] public float citySatisfactionLevel;

    [Header("House Type: SINGLE")]
    [SerializeField] public Material houseSingleMaterial;
    [SerializeField] public float garbageSpeedSingle;
    [SerializeField] public float maxGarbageSingle;
    [SerializeField] public float satisfactionRadiusSingle;
    [SerializeField] public float satisfactionTolleranceSingle;

    [Header("House Type: FAMILY")]
    [SerializeField] public Material houseFamilyMaterial;
    [SerializeField] public float garbageSpeedFamily;
    [SerializeField] public float maxGarbageFamily;
    [SerializeField] public float satisfactionRadiusFamily;
    [SerializeField] public float satisfactionTolleranceFamily;

    [Header("House Type: SHARE")]
    [SerializeField] public Material houseShareMaterial;
    [SerializeField] public float garbageSpeedShare;
    [SerializeField] public float maxGarbageShare;
    [SerializeField] public float satisfactionRadiusShare;
    [SerializeField] public float satisfactionTolleranceShare;

    [Header("Lists")]
    [SerializeField] public List<GarbageManager> houseGarbage = new List<GarbageManager>();
    [SerializeField] public List<SatisfactionManager> houseSatisfaction = new List<SatisfactionManager>();

    private void Update()
    {
        GetHouses();
        GetGarbage();
        GetSatisfaction();
    }

    private void GetHouses()
    {
        // flush the list
        houseGarbage.Clear();

        // find the houses
        GarbageManager[] garbage = GameObject.FindObjectsOfType<GarbageManager>();
        foreach (GarbageManager garbageManager in garbage)
        {
            // add them back into the list
            houseGarbage.Add(garbageManager);
        }
    }

    private void GetGarbage()
    {
        // reset the count
        cityGarbageLevel = 0.0f;
        maxCityGarbageLevel = 0.0f;

        // for each house...
        for (int i = 0; i < houseGarbage.Count; i++)
        {
            // ... add their garbage to the city value
            cityGarbageLevel += houseGarbage[i].garbageLevel;
            maxCityGarbageLevel += houseGarbage[i].maxGarbageLevel;
        }
    }

    private void GetSatisfaction()
    {
        // flush the list
        houseSatisfaction.Clear();

        // temp variables
        float totalSatisfactionPercentage = 0;

        // find the houses
        SatisfactionManager[] satisfaction = GameObject.FindObjectsOfType<SatisfactionManager>();
        foreach (SatisfactionManager satisfactionManager in satisfaction)
        {
            // add them back into the list
            houseSatisfaction.Add(satisfactionManager);
        
            // add their satisfactionPercentage to the sum total
            totalSatisfactionPercentage += satisfactionManager.satisfactionPercentage;
        }

        citySatisfactionLevel = totalSatisfactionPercentage / houseSatisfaction.Count;
    }
}
